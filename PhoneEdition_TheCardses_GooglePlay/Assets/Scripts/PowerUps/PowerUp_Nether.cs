using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Nether : MonoBehaviour {

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
		SendAction (-1, -1, PowerUpManager.ActionType.Enable);
		indicator = (GameObject)Instantiate (indicatorPrefab, ScoreBoardManager.s.indicatorParent);
		indicator.transform.ResetTransformation ();
		LocalPlayerController.s.canSelect = false;
		Activate ();
	}


	public void Activate () {
		SendAction (-1, -1, PowerUpManager.ActionType.Activate);
		StartCoroutine (NetherRoutine());
	}

	public void Disable (){
		SendAction (-1, -1, PowerUpManager.ActionType.Disable);
		Destroy (indicator);
		indicator = null;
		LocalPlayerController.s.canSelect = true;
	}


	//-----------------------------------------------------------------------------------------------Helper Functions

	int netherCount = 0;
	IEnumerator NetherRoutine(){
		
		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);
		netherCount = 0;

		for (int y = 0; y < gridSizeY; y++) {

			IndividualCard NetherPos = CardHandler.s.allCards [0, y].GetComponent<IndividualCard> ();
			GameObject netherEffect = (GameObject)Instantiate (activatePrefab, NetherPos.transform.position, Quaternion.identity);

			for (int x = 0; x < gridSizeX; x++) {

				IndividualCard myCardS =  CardHandler.s.allCards [x, y].GetComponent<IndividualCard> ();

				if (myCardS.isSelectable) {
					if (myCardS.cardType == 0) {

						netherCount++;
						if (netherCount >= 2) {
							ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerIdentifier, 5, 1);
							netherCount = 0;
						}

						GameObject netherGetEffect = (GameObject)Instantiate (selectPrefab, myCardS.transform.position - Vector3.forward, Quaternion.identity);
						//myCardS.cardType = 5;
						//myCardS.cardType = 0;
						yield return new WaitForSeconds (0.01f);

					} else {
						//myCardS.JustRotate ();
						myCardS.NetherMatch();
					}
				}
				yield return new WaitForSeconds(0.005f);
			}
			yield return new WaitForSeconds(0.05f);
		}
		netherCount = 0;
		yield return null;
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
			StartCoroutine (NetworkNetherRoutine ());
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
		
	IEnumerator NetworkNetherRoutine(){

		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);

		for (int y = 0; y < gridSizeY; y++) {

			IndividualCard NetherPos = CardHandler.s.allCards [0, y].GetComponent<IndividualCard> ();
			GameObject netherEffect = (GameObject)Instantiate (activatePrefab, NetherPos.transform.position, Quaternion.identity);

			for (int x = 0; x < gridSizeX; x++) {

				IndividualCard myCardS =  CardHandler.s.allCards [x, y].GetComponent<IndividualCard> ();

				if (myCardS.isSelectable) {
					if (myCardS.cardType == 0) {

						GameObject netherGetEffect = (GameObject)Instantiate (selectPrefab, myCardS.transform.position - Vector3.forward, Quaternion.identity);
						//myCardS.cardType = 5;
						//myCardS.cardType = 0;
						yield return new WaitForSeconds (0.01f);

					} else {
						//myCardS.JustRotate ();
						myCardS.NetherMatch();
					}
				}
				yield return new WaitForSeconds(0.005f);
			}
			yield return new WaitForSeconds(0.05f);
		}
		yield return null;
	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Nether, action);
	}
}