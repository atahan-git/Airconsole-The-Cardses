using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Shadow : MonoBehaviour {


	public GameObject activatePrefab;
	public GameObject indicatorPrefab;
	public GameObject scoreboardPrefab;
	public GameObject selectPrefab;
	GameObject indicator;

	//-----------------------------------------------------------------------------------------------Main Functions

	public void Enable () {
		mem_Cards.Clear ();

		SendAction (-1, -1, PowerUpManager.ActionType.Enable);
		indicator = (GameObject)Instantiate (indicatorPrefab, ScoreBoardManager.s.indicatorParent);
		indicator.transform.ResetTransformation ();
		LocalPlayerController.s.PowerUpMode (true, Activate);
		PowerUpManager.s.canActivatePowerUp = false;
		Invoke ("Disable", GS.a.shadow_activeTime);
	}

	List<IndividualCard> mem_Cards = new List<IndividualCard>();
	public void Activate (IndividualCard myCard) {

		myCard.SelectCard ();
		mem_Cards.Add(myCard);
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);
		Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.Activate);
		LocalPlayerController.s.canSelect = true;
	}

	public void Disable (){
		SendAction (-1, -1, PowerUpManager.ActionType.Disable);
		CheckCards ();
		indicator.GetComponent<DisableAndDestroy> ().Engage ();
		indicator = null;
		LocalPlayerController.s.PowerUpMode (false, Activate);
		PowerUpManager.s.canActivatePowerUp = true;
	}


	//-----------------------------------------------------------------------------------------------Helper Functions


	void CheckCards () {
		CardChecker.s.CheckCards (mem_Cards.ToArray(), 7);
	}



	//-----------------------------------------------------------------------------------------------Networking

	GameObject[] network_scoreboard = new GameObject[4];
	public void ReceiveAction (int player, int x, int y, PowerUpManager.ActionType action) {
		switch (action) {
		case PowerUpManager.ActionType.Enable:
			network_scoreboard [player] = (GameObject)Instantiate (scoreboardPrefab, ScoreBoardManager.s.scoreBoards [player].transform);
			network_scoreboard [player].transform.ResetTransformation ();
			break;
		case PowerUpManager.ActionType.Activate:
			IndividualCard myCard = CardHandler.s.allCards [x, y].GetComponent<IndividualCard>();
			myCard.SelectCard ();
			myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);
			break;
		case PowerUpManager.ActionType.Disable:
			if (network_scoreboard [player] != null)
				network_scoreboard [player].GetComponent<DisableAndDestroy> ().Engage ();
			network_scoreboard [player] = null;
			break;
		default:
			DataLogger.s.LogMessage ("Unrecognized power up action PUS", true);
			break;
		}

	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Shadow, action);
	}
}