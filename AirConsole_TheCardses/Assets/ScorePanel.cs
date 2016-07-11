using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour {

	public int playerid = 0;
	public string playerName = "Blue";

	public Text myName;
	public Text score;

	// Use this for initialization
	void Start () {
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
}
