using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerHandler : MonoBehaviour {

	public static EnemyPlayerHandler s;


	// Use this for initialization
	void Awake () {
		s = this;
	}


	public void ReceiveAction (char player, int x, int y, CardHandler.CardActions action){
		try {

			int i = -1;
			IndividualCard myCard;
			GameObject myEffect;

			switch (player) {
			case 'B':
				i = 0;
				myEffect = GS.a.blueCardSelect;
				break;
			case 'R':
				i = 1;
				myEffect = GS.a.redCardSelect;
				break;
			case 'G':
				i = 2;
				myEffect = GS.a.greenCardSelect;
				break;
			case 'Y':
				i = 3;
				myEffect = GS.a.yellowCardSelect;
				break;
			default:
				myEffect = GS.a.blueCardSelect;
				break;
			}

			myCard = CardHandler.s.allCards [x, y].GetComponent<IndividualCard> ();


			switch (action) {
			case CardHandler.CardActions.Select:
				myCard.selectedEffect = (GameObject)Instantiate (myEffect, myCard.transform.position, Quaternion.identity);
				myCard.SelectCard ();
				break;
			case CardHandler.CardActions.UnSelect:
				myCard.UnSelectCard ();
				break;
			case CardHandler.CardActions.Match:
				myCard.MatchCard ();
				break;
			case CardHandler.CardActions.ReOpen:
				myCard.ReOpenCard ();
				break;
			default:
				break;
			}
		} catch {
			//logText.LogMessage ("Data processing failed " + myCommand.ToString (), true);
			DataLogger.s.LogMessage (x.ToString() + "-" + y.ToString() + " receive enemy action", true);
		}
	}
}
