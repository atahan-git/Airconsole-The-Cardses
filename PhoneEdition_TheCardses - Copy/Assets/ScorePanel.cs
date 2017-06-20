using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ScorePanel : NetworkBehaviour {

	[SyncVar(hook = "UpdateId")]
	public int playerid = 0;
	[SyncVar(hook = "UpdateScore")]
	public int scoreVal = 0;
	public string playerName = "NO PLAYER";

	public Text myName;
	public Text score;

	public Image myImage;
	public Color[] colors;
	// Use this for initialization
	/*void Awake () {
		if (DataHandler.s != null) {
			//UpdateName (1);
		} else {
			if (playerid == 2 || playerid == 3) {
				//Destroy (gameObject);
				myName.text = "NO PLAYER";
			}
		}
		myName.text = playerName;
		score.text = "0";
	}*/

	float bigSize = 65f;

	void Start () {
		SetDirtyBit (int.MaxValue);

		myName.text = playerid.ToString ();

		RectTransform panelParent = GameObject.Find ("LeftPanel").GetComponent<RectTransform> ();

		gameObject.GetComponent<RectTransform> ().parent = panelParent;
		gameObject.GetComponent<RectTransform> ().localScale = panelParent.localScale;

		UpdateId (playerid);
		//Invoke ("CheckIt", 0.5f);
	}


	public void CheckYourself () {
		print ("Score Panel Checked Values - " + playerid);

		if (GameObject.Find ("Local Player").GetComponent<PlayerScript> ().id != playerid) {

			GameObject mainPlayer = GameObject.Find ("Score Panel Main Player");

			if (mainPlayer != null) {
				mainPlayer.GetComponent<RectTransform> ().SetAsLastSibling ();
			}
			GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();


		} else {

			gameObject.GetComponent<LayoutElement> ().minHeight = bigSize;
			gameObject.GetComponent<RectTransform> ().SetAsLastSibling ();
			GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();
			gameObject.gameObject.name = "Score Panel Main Player";
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void UpdateValues(){
		score.text = CalculateScore (ScoreKeeper.s.players[playerid]).ToString ();
	}

	void UpdateScore (int value) {
		scoreVal = value;
		score.text = scoreVal.ToString();
		//print ("Score Updated");
	}

	void UpdateId (int value) {
		playerid = value;
		myName.text = playerid.ToString();
		if (playerid >= 0) {
			myImage.color = colors [playerid];
			ScoreBoardManager.s.scoreBoards [playerid] = gameObject;
		}
	}

	int CalculateScore(ScoreKeeper.Score scoreClass){
		int myScore = 0;

		for (int i = 1; i < 8; i++) {

			myScore += scoreClass.Scores [i];
		}

		return myScore;
	}

	/*void UpdateName(int rand){
		try {
			playerName = AirConsole.instance.GetNickname (AirConsole.instance.ConvertPlayerNumberToDeviceId (playerid));
			myName.text = playerName;
		} catch {
			myName.text = "NO PLAYER";
			//Destroy (gameObject);
		}

		if (playerName == "Guest 0") {
			//Destroy (gameObject);
			myName.text = "NO PLAYER";
		}

		if (playerid >= AirConsole.instance.GetActivePlayerDeviceIds.Count) {
			myName.text = "NO PLAYER";
		}
	}*/
}
