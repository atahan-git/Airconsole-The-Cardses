using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	//public Score playerBlue = new Score (0);
	//public Score playerRed = new Score (1);

	public Score[] players = new Score[8];

	public static ScoreKeeper s;

	ScorePanel[] panels;

	public ScoreBoardManager man;

	// Use this for initialization
	void Awake () {
		s = this;

		for (int i = 0; i < players.Length; i++) {
			players [i] = new Score ();
		}
	}

	void Start(){
		Invoke ("GetPanels", 0.1f);
	}
	void GetPanels(){
		panels = GameObject.FindObjectsOfType<ScorePanel> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.H)) {
			for (int i = 0; i < players.Length; i++) {
				for (int m = 0; m < 15; m++) {
					AddScore (i, m, 1);
				}
			}
		}
	
	}
		
	public void AddScore(int playerId, int cardId, int toAdd){

		/*print (playerId + "   -   " + cardId);
		print (players [playerId]);
		print (players [playerId].Scores [cardId]);*/
		players [playerId].Scores [cardId] += toAdd;

		/*foreach (ScorePanel pan in panels) {
			pan.BroadcastMessage ("UpdateValues");
		}*/

		man.UpdateScores ();
		if (cardId > 7)
			man.RpcCheckAddDragon (playerId, cardId, players [playerId].Scores [cardId]);

	}

	[System.Serializable]
	public class Score{

		//public int playerid = 0;

		//1-7 normal cards
		//8-14 dragons
		public int[] Scores = new int[15];

		public Score(){
			Scores = new int[15];
		}

		public void ResetScore(){
			Scores = new int[15];
		}
	}
}
