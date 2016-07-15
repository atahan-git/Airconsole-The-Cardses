using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ScorePanel : MonoBehaviour {

	public int playerid = 0;
	public string playerName = "NO PLAYER";

	public Text myName;
	public Text score;

	// Use this for initialization
	void Awake () {
		if (DataHandler.s != null) {
			UpdateName (1);
		} else {
			if (playerid == 2 || playerid == 3) {
				Destroy (gameObject);
			}
		}
		myName.text = playerName;
		score.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void UpdateValues(){
		score.text = CalculateScore (ScoreKeeper.s.players[playerid]).ToString ();
	}

	int CalculateScore(ScoreKeeper.Score scoreClass){
		int myScore = 0;

		for (int i = 1; i < 8; i++) {

			myScore += scoreClass.Scores [i];
		}

		return myScore;
	}

	void UpdateName(int rand){
		try {
			playerName = AirConsole.instance.GetNickname (AirConsole.instance.ConvertPlayerNumberToDeviceId (playerid));
			myName.text = playerName;
		} catch {
			myName.text = "NO PLAYER";
			Destroy (gameObject);
		}

		if (playerName == "Guest 0")
			Destroy (gameObject);
	}
}
