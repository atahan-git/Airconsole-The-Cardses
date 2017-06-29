using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Poison : MonoBehaviour {


	public GameObject activatePrefab;
	public GameObject indicatorPrefab;
	public GameObject scoreboardPrefab;
	public GameObject selectPrefab;
	public GameObject scoreLowerExplosionPrefab;
	GameObject indicator;

	//-----------------------------------------------------------------------------------------------Main Functions

	public void Enable () {
		SendAction (-1, -1, PowerUpManager.ActionType.Enable);
		indicator = (GameObject)Instantiate (indicatorPrefab, ScoreBoardManager.s.indicatorParent);
		indicator.transform.ResetTransformation ();
		LocalPlayerController.s.PowerUpMode (true, Activate);
		PowerUpManager.s.canActivatePowerUp = false;
	}
		
	public void Activate (IndividualCard myCard) {
		StartCoroutine (_Activate (myCard));
	}

	IEnumerator _Activate (IndividualCard myCard){
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.Activate);

		myCard.SelectCard ();
		myCard.cardType = 15;
		myCard.isPoison = true;
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);


		indicator.GetComponent<DisableAndDestroy> ().Engage ();
		indicator = null;
		LocalPlayerController.s.PowerUpMode (false, Activate);
		PowerUpManager.s.canActivatePowerUp = true;

		yield return new WaitForSeconds (GS.a.poison_activeTime);

		LocalPlayerController.s.canSelect = true;
		myCard.UnSelectCard ();
	}
		

	public void ChoosePoisonCard (IndividualCard myCard){
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.Disable);
		StartCoroutine (_ChoosePoisonCard (myCard));
	}
		
	IEnumerator _ChoosePoisonCard (IndividualCard myCard){
		myCard.SelectCard ();
		Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);

		yield return new WaitForSeconds (GS.a.poison_checkSpeed /4f);

		for (int i = 0; i < GS.a.poison_damage; i++) {
			ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerIdentifier, 0, -1);
			GameObject exp = (GameObject)Instantiate (scoreLowerExplosionPrefab, ScoreBoardManager.s.scoreBoards [DataHandler.s.myPlayerinteger].transform);
			exp.transform.ResetTransformation ();

			yield return new WaitForSeconds ((GS.a.poison_checkSpeed / 2f) / (float)GS.a.poison_damage);
		}

		yield return new WaitForSeconds (GS.a.poison_checkSpeed /4f);

		LocalPlayerController.s.canSelect = true;
		myCard.PoisonMatch ();
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
			IndividualCard myCard = CardHandler.s.allCards [x, y].GetComponent<IndividualCard> ();
			StartCoroutine (NetworkActivate (player, myCard));
			break;
		case PowerUpManager.ActionType.Disable:
			IndividualCard myCard2 = CardHandler.s.allCards [x, y].GetComponent<IndividualCard> ();
			StartCoroutine (NetworkDisable (player, myCard2));
			break;
		default:
			DataLogger.s.LogMessage ("Unrecognized power up action PUP", true);
			break;
		}

	}

	IEnumerator NetworkActivate (int player, IndividualCard myCard){
		myCard.SelectCard ();
		myCard.cardType = 15;
		myCard.isPoison = true;
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);

		yield return new WaitForSeconds (GS.a.poison_activeTime);


		if (network_scoreboard [player] != null)
			network_scoreboard [player].GetComponent<DisableAndDestroy> ().Engage ();
		network_scoreboard [player] = null;

		myCard.UnSelectCard ();
	}

	IEnumerator NetworkDisable (int player, IndividualCard myCard){
		myCard.SelectCard ();
		Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);

		yield return new WaitForSeconds (GS.a.poison_checkSpeed /4f);

		for (int i = 0; i < GS.a.poison_damage; i++) {
			GameObject exp = (GameObject)Instantiate (scoreLowerExplosionPrefab, ScoreBoardManager.s.scoreBoards [player].transform);
			exp.transform.ResetTransformation ();

			yield return new WaitForSeconds ((GS.a.poison_checkSpeed / 2f) / (float)GS.a.poison_damage);
		}

		yield return new WaitForSeconds (GS.a.poison_checkSpeed / 4f);

		myCard.PoisonMatch ();

	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Poison, action);
	}
}