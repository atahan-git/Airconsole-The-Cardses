using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerHandler : MonoBehaviour {

	public static EnemyPlayerHandler s;

	public GameObject blueCardSelect;
	public GameObject redCardSelect;
	public GameObject greenCardSelect;
	public GameObject yellowCardSelect;

	// Use this for initialization
	void Awake () {
		s = this;
	}


	public void ReceiveAction (char player, int x, int y, CardHandler.CardActions action){
		int i = -1;
		IndividualCard myCard;
		GameObject myEffect;

		switch (player) {
		case 'B':
			i = 0;
			myEffect = blueCardSelect;
			break;
		case 'R':
			i = 1;
			myEffect = redCardSelect;
			break;
		case 'G':
			i = 2;
			myEffect = greenCardSelect;
			break;
		case 'Y':
			i = 3;
			myEffect = yellowCardSelect;
			break;
		default:
			myEffect = blueCardSelect;
			break;
		}

		myCard = CardHandler.s.allCards [x, y].GetComponent<IndividualCard>();


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
	}
}
