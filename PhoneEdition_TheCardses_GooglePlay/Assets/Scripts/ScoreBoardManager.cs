using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour {

	public static ScoreBoardManager s;

	int[] s_Blue 	= new int[15];	//0
	int[] s_Red 	= new int[15];	//1
	int[] s_Green	= new int[15];	//2
	int[] s_Yellow 	= new int[15];	//3

	public GameObject scoreboardPrefab;
	public Transform scoreboardParent;
	[HideInInspector]
	public GameObject[] scoreBoards = new GameObject[4];

	public Transform indicatorParent;

	DataLogger logText;
	// Use this for initialization
	void Start () {
		s = this;

		logText = GameObject.FindGameObjectWithTag ("LogText").GetComponent<DataLogger> ();
		try{
		SetUpPlayerScoreboardPanels ();
		}catch{
			logText.LogMessage ("Scoreboard creation failed",true);
		}
		UpdatePanels ();
	}
	
	void SetUpPlayerScoreboardPanels(){
		int i = 0;

		//logText.text = "Spawning panels";

		scoreBoards [DataHandler.s.myPlayerinteger] = (GameObject)Instantiate (scoreboardPrefab, scoreboardParent);
		scoreBoards [DataHandler.s.myPlayerinteger].transform.ResetTransformation ();
		scoreBoards [DataHandler.s.myPlayerinteger].GetComponent<ScorePanel> ().SetValues (DataHandler.s.myPlayerinteger, GoogleAPI.s.GetSelf().DisplayName, true);

		logText.LogMessage("PLayer scoreboard succesfuly created");
			
		foreach (GooglePlayGames.BasicApi.Multiplayer.Participant part in GoogleAPI.s.participants) {
			if (i != DataHandler.s.myPlayerinteger) {
				logText.LogMessage("Creating " + i.ToString() + " scoreboard");
				scoreBoards [i] = (GameObject)Instantiate (scoreboardPrefab, scoreboardParent);
				scoreBoards [i].transform.ResetTransformation ();
				scoreBoards [i].GetComponent<ScorePanel> ().SetValues (i, part.DisplayName,false);
				logText.LogMessage(i.ToString() + " scoreboard created");
			}
			i++;
		}
	}

	void UpdatePanels (){
		int i = 0;
		foreach (GameObject sBoard in scoreBoards) {
			if (sBoard != null) {
				int[] myArray = new int[15];
				switch (i) {
				case 0:
					myArray = s_Blue;
					break;
				case 1:
					myArray = s_Red;
					break;
				case 2:
					myArray = s_Green;
					break;
				case 3:
					myArray = s_Yellow;
					break;
				default:
					GoogleAPI.s.logText.LogMessage ("Unknown player type, scorepanel", true);
					break;
				}


				try {
					sBoard.gameObject.GetComponent<ScorePanel> ().UpdateScore (myArray [14]);
				} catch {
					try {
						if (sBoard.GetComponent<ScorePanel> () == null)
							GoogleAPI.s.logText.LogMessage ("cant find scorepanel script" + myArray [14].ToString (), true);
					} catch {
						GoogleAPI.s.logText.LogMessage ("what the fuck", true);
					}
					GoogleAPI.s.logText.LogMessage ("sth is wrong with the array " + myArray [14].ToString (), true);
				}
				i++;
			}
		}
	}

	public void AddScore (char player, int scoreType, int toAdd){
		int[] myArray = new int[15];
		try {
			switch (player) {
			case DataHandler.p_blue:
				myArray = s_Blue;
				break;
			case DataHandler.p_red:
				myArray = s_Red;
				break;
			case DataHandler.p_green:
				myArray = s_Green;
				break;
			case DataHandler.p_yellow:
				myArray = s_Yellow;
				break;
			default:
				GoogleAPI.s.logText.LogMessage ("Unknown player type", true);
				break;
			}
		} catch {
			GoogleAPI.s.logText.LogMessage ("player identification error", true);
		}

		myArray [scoreType] += toAdd;
		try {
			DataHandler.s.SendScore (player, scoreType, toAdd);
		} catch {
			GoogleAPI.s.logText.LogMessage ("sending score error", true);
		}

		try {
		if (scoreType > 7) {
			PowerUpManager.s.PowerUpMenuEnable (scoreType, myArray [scoreType]);
		} else {
			myArray [14] += toAdd;
		}
		} catch {
			GoogleAPI.s.logText.LogMessage ("scoretype handling error " + scoreType, true);
		}

		try{
		UpdatePanels ();
		} catch {
			GoogleAPI.s.logText.LogMessage ("panel update error", true);
		}
	}

	public void ReceiveScore (char player, int scoreType, int toAdd){
		GoogleAPI.s.logText.LogMessage ("score received");
		int[] myArray = new int[15];
		try {
			switch (player) {
			case DataHandler.p_blue:
				myArray = s_Blue;
				break;
			case DataHandler.p_red:
				myArray = s_Red;
				break;
			case DataHandler.p_green:
				myArray = s_Green;
				break;
			case DataHandler.p_yellow:
				myArray = s_Yellow;
				break;
			default:
				GoogleAPI.s.logText.LogMessage ("SR Unknown player type", true);
				break;
			}
		} catch {
			GoogleAPI.s.logText.LogMessage ("SR player identification error", true);
		}

		myArray [scoreType] += toAdd;
		if (scoreType > 7) {
		} else {
			myArray [15] += toAdd;
		}

		UpdatePanels ();
	}
}
