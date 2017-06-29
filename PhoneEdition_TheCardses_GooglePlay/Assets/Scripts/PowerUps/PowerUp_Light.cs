﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Light : MonoBehaviour {


	public GameObject cantSelectPrefab;
	public GameObject affectedPrefab;
	public GameObject affectorPrefab;

	//-----------------------------------------------------------------------------------------------Main Functions

	GameObject[] scoreBoardObjects = new GameObject[4];
	public void Enable () {
		try {
			SendAction (-1, -1, PowerUpManager.ActionType.Enable);
			Invoke ("Disable", GS.a.light_activeTime);


			//we are the player who activated the effect so we dont freeze but others do

			//spawn freeze effect for the frozen players
			for (int i = 0; i < ScoreBoardManager.s.scoreBoards.Length; i++) {
				if (i != DataHandler.s.myPlayerinteger) {
					if (scoreBoardObjects [i] != null) {
						Destroy (scoreBoardObjects [i]);
						scoreBoardObjects [i] = null;
					}
					if (ScoreBoardManager.s.scoreBoards [i]) {
						scoreBoardObjects [i] = (GameObject)Instantiate (affectedPrefab, ScoreBoardManager.s.scoreBoards [i].transform);
						scoreBoardObjects [i].transform.ResetTransformation ();
					}
				}
			}
		} catch (System.Exception e) {
			DataLogger.s.LogMessage (e.StackTrace, true);
		}
	}

	public void Disable (){
		try {
			SendAction (-1, -1, PowerUpManager.ActionType.Disable);
			for (int i = 0; i < scoreBoardObjects.Length; i++) {
				if (scoreBoardObjects [i] != null) {
					Destroy (scoreBoardObjects [i]);
					scoreBoardObjects [i] = null;
				}
			}
		} catch (System.Exception e) {
			DataLogger.s.LogMessage (e.StackTrace, true);
		}
	}



	//-----------------------------------------------------------------------------------------------Networking

	GameObject[] network_scoreboard_or = new GameObject[4];
	GameObject[] network_scoreboard_ed = new GameObject[4];
	GameObject cantSelectObj;
	public void ReceiveAction (int player, int x, int y, PowerUpManager.ActionType action) {
		try {
			switch (action) {
			case PowerUpManager.ActionType.Enable:
				PowerUpManager.s.canActivatePowerUp = false;
				for (int i = 0; i < ScoreBoardManager.s.scoreBoards.Length; i++) {
					if (i != player) {
						if (network_scoreboard_ed [i] != null) {
							Destroy (network_scoreboard_ed [i]);
							network_scoreboard_ed [i] = null;
						}
						if (ScoreBoardManager.s.scoreBoards [i]) {
							network_scoreboard_ed [i] = (GameObject)Instantiate (affectedPrefab, ScoreBoardManager.s.scoreBoards [i].transform);
							network_scoreboard_ed [i].transform.ResetTransformation ();
						}
					}
				}
				network_scoreboard_or [player] = (GameObject)Instantiate (affectorPrefab, ScoreBoardManager.s.scoreBoards [player].transform);
				network_scoreboard_or [player].transform.ResetTransformation ();

				if (cantSelectObj != null) {
					Destroy (cantSelectObj);
					cantSelectObj = null;
				}
				cantSelectObj = (GameObject)Instantiate (cantSelectPrefab, cantSelectPrefab.transform.position, cantSelectPrefab.transform.rotation);

				break;
			case PowerUpManager.ActionType.Disable:
				PowerUpManager.s.canActivatePowerUp = true;
				for (int i = 0; i < network_scoreboard_ed.Length; i++) {
					if (network_scoreboard_ed [i] != null) {
						Destroy (network_scoreboard_ed [i]);
						network_scoreboard_ed [i] = null;
					}
				}
				for (int i = 0; i < network_scoreboard_or.Length; i++) {
					if (network_scoreboard_or [i] != null) {
						DataLogger.s.LogMessage("Destroying a light element " + i.ToString());
						Destroy (network_scoreboard_or [i]);
						network_scoreboard_or [i] = null;
					}
				}

				if (cantSelectObj != null) {
					Destroy (cantSelectObj);
					cantSelectObj = null;
				}
				break;
			default:
				DataLogger.s.LogMessage ("Unrecognized power up action PUL", true);
				break;
			}
		} catch (System.Exception e) {
			DataLogger.s.LogMessage (e.StackTrace, true);
		}
	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Light, action);
	}
}