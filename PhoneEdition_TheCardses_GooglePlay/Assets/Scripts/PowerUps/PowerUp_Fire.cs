using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Fire : MonoBehaviour {

	public GameObject activatePrefab;
	public GameObject indicatorPrefab;
	public GameObject scoreboardPrefab;
	GameObject indicator;

	//-----------------------------------------------------------------------------------------------Main Functions

	public void Enable () {
		SendAction (-1, -1, PowerUpManager.ActionType.Enable);
		indicator = (GameObject)Instantiate (indicatorPrefab, ScoreBoardManager.s.indicatorParent);
		indicator.transform.ResetTransformation ();
		LocalPlayerController.s.PowerUpMode (true, Activate);
		PowerUpManager.s.canActivatePowerUp = false;
	}


	IndividualCard[] mem_Cards = new IndividualCard[9];
	public void Activate (IndividualCard myCard) {
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.Activate);
		LocalPlayerController.s.canSelect = false;
		StartCoroutine (SelectSquareCards (myCard));
	}
		

	public void Disable (){
		SendAction (-1, -1, PowerUpManager.ActionType.Disable);
		indicator.GetComponent<DisableAndDestroy> ().Engage ();
		indicator = null;
		LocalPlayerController.s.PowerUpMode (false, Activate);
		PowerUpManager.s.canActivatePowerUp = true;
		LocalPlayerController.s.canSelect = true;
	}


	//-----------------------------------------------------------------------------------------------Helper Functions

	IEnumerator SelectSquareCards (IndividualCard center){


		Instantiate (activatePrefab, center.transform.position, Quaternion.identity);

		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);


		//get cards
		int leftLimit  = (int)Mathf.Clamp (center.x - 1, 0, gridSizeX - 1);
		int rightLimit = (int)Mathf.Clamp (center.x + 1, 0, gridSizeX - 1);
		int downLimit  = (int)Mathf.Clamp (center.y - 1, 0, gridSizeY - 1);
		int upLimit    = (int)Mathf.Clamp (center.y + 1, 0, gridSizeY - 1);

		int n = 0;
		for (int i = leftLimit; i <= rightLimit; i++) {
			for (int m = downLimit; m <= upLimit; m++) {

				IndividualCard myCardS = CardHandler.s.allCards [i, m].GetComponent<IndividualCard> ();

				if (myCardS.cardType != 0) {
					if (myCardS.isSelectable) {

						SelectCard (myCardS, n);
						n++;

						yield return new WaitForSeconds (0.05f);
					}
				}
			}
		}

		yield return new WaitForSeconds (0.3f);

		CheckCards ();
	}

	void SelectCard (IndividualCard myCard, int i){
		myCard.SelectCard ();
		mem_Cards[i] = myCard;
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.SelectCard);
	}

	void CheckCards (){
		CardChecker.s.CheckPowerUp (mem_Cards, 2, CallBack);
	}

	public void CallBack (){
		Disable ();
	}



	//-----------------------------------------------------------------------------------------------Networking

	GameObject[] network_scoreboard = new GameObject[4];
	public void ReceiveAction (int player, int x, int y, PowerUpManager.ActionType action) {

		try {
			switch (action) {
			case PowerUpManager.ActionType.Enable:
				network_scoreboard [player] = (GameObject)Instantiate (scoreboardPrefab, ScoreBoardManager.s.scoreBoards [player].transform);
				network_scoreboard [player].transform.ResetTransformation ();
				break;
			case PowerUpManager.ActionType.Activate:
				IndividualCard myCard = CardHandler.s.allCards [x, y].GetComponent<IndividualCard> ();
				Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);
				break;
			case PowerUpManager.ActionType.SelectCard:
				IndividualCard myCard2 = CardHandler.s.allCards [x, y].GetComponent<IndividualCard> ();
				myCard2.SelectCard ();
				break;
			case PowerUpManager.ActionType.Disable:
				if (network_scoreboard [player] != null)
					network_scoreboard [player].GetComponent<DisableAndDestroy> ().Engage ();
				network_scoreboard [player] = null;
				break;
			default:
				DataLogger.s.LogMessage ("Unrecognized power up action PUF", true);
				break;
			}
		} catch (System.Exception e) {
			DataLogger.s.LogMessage (e.StackTrace + " -*- " + player.ToString() + " -*- " + x.ToString() + " -*- " + y.ToString() + " -*- " + action.ToString(), true);
		}
	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Fire, action);
	}
}