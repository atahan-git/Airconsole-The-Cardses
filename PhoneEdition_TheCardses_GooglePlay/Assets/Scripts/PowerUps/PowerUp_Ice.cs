using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Ice : MonoBehaviour {

	public float activeTime = 10f;
	public float checkSpeed = 0.5f;

	[Space]

	public GameObject selectPrefab;
	public GameObject activatePrefab;
	public GameObject scoreboardPrefab;
	public GameObject indicatorPrefab;
	GameObject indicator;

	//-----------------------------------------------------------------------------------------------Main Functions

	public void Enable () {
		
		return;

		SendAction (-1, -1, PowerUpManager.ActionType.Enable);
		/*indicator = (GameObject)Instantiate (indicatorPrefab, ScoreBoardManager.s.indicatorParent);
		indicator.transform.ResetTransformation ();*/
		Activate ();
		Invoke ("Disable", activeTime);
	}


	public void Activate () {
		SendAction (-1, -1, PowerUpManager.ActionType.Activate);

		//we are the player so we dont freeze but others do

		//spawn freeze effect for the frozen players
	}

	public void Disable (){
		SendAction (-1, -1, PowerUpManager.ActionType.Disable);
		Destroy (indicator);
		indicator = null;
	}



	//-----------------------------------------------------------------------------------------------Networking

	GameObject[] network_scoreboard;
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
				Destroy (network_scoreboard [player]);
			network_scoreboard [player] = null;
			break;
		default:

			break;
		}

	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Ice, action);
	}
}