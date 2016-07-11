using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour {

	public int cardType = 0;

	Image image;
	Text text;

	ScorePanel sp;

	// Use this for initialization
	void Start () {
		sp = GetComponentInParent<ScorePanel> ();
		Image[] imageTemp = GetComponentsInChildren<Image> ();
		foreach (Image img in imageTemp) {
			if(img != GetComponent<Image>()){
				image = img;
				break;
			}
		}

		text = GetComponentInChildren<Text> ();

		image.sprite = AllCardSprites.s.fronts [cardType];
		UpdateValues ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateValues(){
		text.text = ScoreKeeper.s.players[sp.playerid].Scores [cardType].ToString ();
	}
}
