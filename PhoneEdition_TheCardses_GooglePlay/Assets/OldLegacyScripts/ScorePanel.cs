using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScorePanel : MonoBehaviour {


	public int playerid = 0;
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

		myName.text = playerid.ToString ();

		RectTransform panelParent = GameObject.Find ("LeftPanel").GetComponent<RectTransform> ();

		gameObject.GetComponent<RectTransform> ().parent = panelParent;
		gameObject.GetComponent<RectTransform> ().localScale = panelParent.localScale;

		UpdateId (playerid);
		//Invoke ("CheckIt", 0.5f);
	}


	public void SetValues (int id, string name, bool isMainPlayer) {
		print ("Score Panel Checked Values - " + playerid);
		playerName = name;
		UpdateId (id);

		if (!isMainPlayer) {

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

	public void UpdateScore (int value) {
		scoreVal = value;
		score.text = scoreVal.ToString();
		//print ("Score Updated");
	}

	void UpdateId (int value) {
		playerid = value;
		myName.text = playerName;
		if (playerid >= 0) {
			myImage.color = colors [playerid];
			//ScoreBoardManager.s.scoreBoards [playerid] = gameObject;
		}
	}
		
}
