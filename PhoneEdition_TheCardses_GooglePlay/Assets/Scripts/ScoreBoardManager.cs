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

	int[] s_NPC 	= new int[15];	//4 


	public GameObject scoreboardPrefab;
	public Transform scoreboardParent;
	[HideInInspector]
	public GameObject[] scoreBoards = new GameObject[5];

	public Transform indicatorParent;

	DataLogger logText;
	// Use this for initialization
	void Start () {
		s = this;

		logText = DataLogger.s;
		try{
			scoreBoards = new GameObject[5];
		SetUpPlayerScoreboardPanels ();
		}catch{
			logText.LogMessage ("Scoreboard creation failed",true);
			print ("Scoreboard creation failed");
		}
		UpdatePanels ();
	}

	void Update (){
		if (Input.touchCount > 2 || Input.GetKeyDown(KeyCode.H)) {
			for (int i = 0; i <= 14; i++) {
				AddScore (DataHandler.s.myPlayerIdentifier, i, 1);
			}
		}
	}
	
	void SetUpPlayerScoreboardPanels(){
		int i = 0;

		//logText.text = "Spawning panels";

		print("spawning player panel");

		scoreBoards [DataHandler.s.myPlayerinteger] = (GameObject)Instantiate (scoreboardPrefab, scoreboardParent);
		scoreBoards [DataHandler.s.myPlayerinteger].transform.ResetTransformation ();
		try {
			scoreBoards [DataHandler.s.myPlayerinteger].GetComponent<ScorePanel> ().SetValues (DataHandler.s.myPlayerinteger, GoogleAPI.s.GetSelf ().DisplayName, true);
		} catch {
			scoreBoards [DataHandler.s.myPlayerinteger].GetComponent<ScorePanel> ().SetValues (DataHandler.s.myPlayerinteger, "PLayer", true);
		}

		logText.LogMessage ("PLayer scoreboard succesfuly created");
		print("PLayer scoreboard succesfuly created");
			
		if (!GS.a.isNPC) {
			print("fuck me");
			foreach (GooglePlayGames.BasicApi.Multiplayer.Participant part in GoogleAPI.s.participants) {
				if (i != DataHandler.s.myPlayerinteger) {
					logText.LogMessage ("Creating " + i.ToString () + " scoreboard");
					scoreBoards [i] = (GameObject)Instantiate (scoreboardPrefab, scoreboardParent);
					scoreBoards [i].transform.ResetTransformation ();
					scoreBoards [i].GetComponent<ScorePanel> ().SetValues (i, part.DisplayName, false);
					logText.LogMessage (i.ToString () + " scoreboard created");
				}
				i++;
			}
		}
		if (GS.a.isNPC) {
			print ("Spawning npc scorboard panel");
			scoreBoards[4] = (GameObject)Instantiate (GS.a.npcScoreboard, scoreboardParent);
			scoreBoards[4].GetComponent<ScorePanel> ().SetValues (GS.a.npcId, GS.a.npcName,false);
			logText.LogMessage(GS.a.npcName + " scoreboard created");
		}
	}

	void UpdatePanels (){
		
		for (int i = 0; i < 5; i++) {
			GameObject sBoard = scoreBoards [i];
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
				case 4:
					myArray = s_NPC;
					break;
				default:
					DataLogger.s.LogMessage ("Unknown player type, scorepanel", true);
					break;
				}


				try {
					sBoard.gameObject.GetComponent<ScorePanel> ().UpdateScore (myArray [0]);
				} catch {
					try {
						if (sBoard.GetComponent<ScorePanel> () == null)
							DataLogger.s.LogMessage ("cant find scorepanel script" + myArray [0].ToString (), true);
					} catch {
						DataLogger.s.LogMessage ("what the fuck", true);
					}
					DataLogger.s.LogMessage ("sth is wrong with the array " + myArray [0].ToString (), true);
				}
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
			case DataHandler.p_NPC:
				myArray = s_NPC;
				break;
			default:
				DataLogger.s.LogMessage ("Unknown player type", true);
				break;
			}
		} catch {
			DataLogger.s.LogMessage ("player identification error", true);
		}

		myArray [scoreType] += toAdd;
		try {
			DataHandler.s.SendScore (player, scoreType, toAdd);
		} catch {
			DataLogger.s.LogMessage ("sending score error", true);
		}

		try {
			if (scoreType > 7) {
				PowerUpManager.s.PowerUpMenuEnable (scoreType, myArray [scoreType]);
			} else {
				myArray [0] += toAdd;
				myArray [0] = (int)Mathf.Clamp (myArray [0], 0, Mathf.Infinity);
			}
		} catch {
			DataLogger.s.LogMessage ("scoretype handling error " + scoreType, true);
		}

		try {
			UpdatePanels ();
		} catch {
			DataLogger.s.LogMessage ("panel update error", true);
		}
	}

	public void ReceiveScore (char player, int scoreType, int toAdd){
		DataLogger.s.LogMessage ("score received");
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
				DataLogger.s.LogMessage ("SR Unknown player type", true);
				break;
			}
		} catch {
			DataLogger.s.LogMessage ("SR player identification error", true);
		}

		myArray [scoreType] += toAdd;
		if (scoreType > 7) {
		} else {
			myArray [0] += toAdd;
		}

		UpdatePanels ();
	}
}
