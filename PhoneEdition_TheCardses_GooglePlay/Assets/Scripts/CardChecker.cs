using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChecker : MonoBehaviour {

	public static CardChecker s;

	// Use this for initialization
	void Start () {
		s = this;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public delegate void Callback();

	public void CheckPowerUp (IndividualCard[] cardsToCheck, int cardType, Callback myCallBack){
		StartCoroutine (CheckCardsCOROT (cardsToCheck, cardType, myCallBack));
	}

	public void CheckNormal (IndividualCard[] cardsToCheck){
		DataLogger.s.LogMessage ("Checking match");
		int scoreType = -1;
		if (cardsToCheck [0].cardType == cardsToCheck [1].cardType) {
			DataLogger.s.LogMessage ("Cards Matched");
			scoreType = cardsToCheck [0].cardType;
			cardsToCheck [0].MatchCard ();
			cardsToCheck [1].MatchCard ();
			DataHandler.s.SendPlayerAction (cardsToCheck [0].x, cardsToCheck [0].y, CardHandler.CardActions.Match);
			DataHandler.s.SendPlayerAction (cardsToCheck [1].x, cardsToCheck [1].y, CardHandler.CardActions.Match);
		} else {
			DataLogger.s.LogMessage ("Cards unselected");
			cardsToCheck [0].UnSelectCard ();
			cardsToCheck [1].UnSelectCard ();
			DataHandler.s.SendPlayerAction (cardsToCheck [0].x, cardsToCheck [0].y, CardHandler.CardActions.UnSelect);
			DataHandler.s.SendPlayerAction (cardsToCheck [1].x, cardsToCheck [1].y, CardHandler.CardActions.UnSelect);
		}

		if (scoreType != -1) {
			DataLogger.s.LogMessage ("sending score " + scoreType.ToString());
			ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerIdentifier, scoreType, 1);
		}

		DataLogger.s.LogMessage ("Score type is " + scoreType.ToString());
		//empty our array because no need left
		EmptyArray(cardsToCheck);
		DataLogger.s.LogMessage ("array emptied");
	}
		
	IEnumerator CheckCardsCOROT(IndividualCard[] cardsToCheck, int cardType, Callback myCallBack){

		//check Cards
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cardType != 0) {

					for (int k = 1; k < cardsToCheck.Length; k++) {
						if (cardsToCheck [k] != null && cardsToCheck [l] != null) {
							if (cardsToCheck [k].cardType != 0) {
								if (k != l) {

									if (cardsToCheck [k].cardType == cardsToCheck [l].cardType) {

										int myCardType = cardsToCheck [k].cardType;
										//check if it is dragon
										if (myCardType < 8)
											myCardType = cardType;
										else
											myCardType = cardType + 7;

										cardsToCheck [k].MatchCard ();
										cardsToCheck [l].MatchCard ();
										DataHandler.s.SendPlayerAction (cardsToCheck [k].x, cardsToCheck [k].y, CardHandler.CardActions.Match);
										DataHandler.s.SendPlayerAction (cardsToCheck [l].x, cardsToCheck [l].y, CardHandler.CardActions.Match);

										ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerIdentifier, myCardType, 1);

										yield return new WaitForSeconds (0.1f);
									}
								}
							}
						}
					}
				}
			}
		}

		yield return new WaitForSeconds (0.5f);

		//Rotate unused cards
		UnSelectCards(cardsToCheck);
		//empty our array because no need left
		EmptyArray(cardsToCheck);

		myCallBack.Invoke ();
	}

	public void CheckCards(IndividualCard[] cardsToCheck, int cardType){
		//check Cards
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cardType != 0) {

					for (int k = 1; k < cardsToCheck.Length; k++) {
						if (cardsToCheck [k] != null && cardsToCheck [l] != null) {
							if (cardsToCheck [k].cardType != 0) {
								if (k != l) {

									if (cardsToCheck [k].cardType == cardsToCheck [l].cardType) {

										int myCardType = cardsToCheck [k].cardType;
										//check if it is dragon
										if (myCardType < 8)
											myCardType = cardType;
										else
											myCardType = cardType + 7;

										cardsToCheck [k].MatchCard ();
										cardsToCheck [l].MatchCard ();
										DataHandler.s.SendPlayerAction (cardsToCheck [k].x, cardsToCheck [k].y, CardHandler.CardActions.Match);
										DataHandler.s.SendPlayerAction (cardsToCheck [l].x, cardsToCheck [l].y, CardHandler.CardActions.Match);

										ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerIdentifier, myCardType, 1);
									}
								}
							}
						}
					}
				}
			}
		}


		//Rotate unused cards
		UnSelectCards(cardsToCheck);
		//empty our array because no need left
		EmptyArray(cardsToCheck);

	}

	public void UnSelectCards (IndividualCard[] cardsToCheck){
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cardType != 0) {
					cardsToCheck [l].UnSelectCard ();
					DataHandler.s.SendPlayerAction (cardsToCheck [l].x, cardsToCheck [l].y, CardHandler.CardActions.UnSelect);
				}
			}
		}
	}

	public void EmptyArray (IndividualCard[] cardsToCheck){
		for (int i = 0; i < cardsToCheck.Length; i++) {
			cardsToCheck[i] = null;
		}
	}
}
