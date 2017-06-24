using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour {

	public static LocalPlayerController s;

	public float checkSpeed = 0.35f;


	CardHandler cardHand;
	PowerUpManager powerUp;


	public bool canSelect = true;
	bool isPowerUp = false;

	GameObject myEffect;
	IndividualCard[] mem_Card = new IndividualCard[2];

	void Start () {
		s = this;

		switch (DataHandler.s.myPlayerIdentifier) {
		case DataHandler.p_blue:
			myEffect = EnemyPlayerHandler.s.blueCardSelect;
			break;
		case DataHandler.p_red:
			myEffect = EnemyPlayerHandler.s.redCardSelect;
			break;
		case DataHandler.p_green:
			myEffect = EnemyPlayerHandler.s.greenCardSelect;
			break;
		case DataHandler.p_yellow:
			myEffect = EnemyPlayerHandler.s.yellowCardSelect;
			break;
		default:
			myEffect = EnemyPlayerHandler.s.blueCardSelect;
			break;
		}

	}


	void Update () {
		if (Input.GetMouseButtonDown (0) && canSelect /*&& Input.mousePosition.x > Screen.width / 6.6f*/) {

			RaycastHit hit = new RaycastHit ();

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {

				//print (myHet.collider.transform.parent.gameObject.name);

				IndividualCard myCardS = hit.collider.gameObject.GetComponentInParent<IndividualCard> ();

				if (myCardS != null) {
					if (myCardS.isSelectable) {
						canSelect = false;

						if (!isPowerUp) {
							SelectCard (myCardS);
						} else {
							PowerUpSelect (myCardS);
						}
					}
				}
			}
		}
	}


	void SelectCard(IndividualCard myCardS){
		if (mem_Card [0] == null) {
			mem_Card [0] = myCardS;
			mem_Card [0].selectedEffect = (GameObject)Instantiate (myEffect, mem_Card [0].transform.position, Quaternion.identity);
			mem_Card [0].SelectCard ();
			DataHandler.s.SendPlayerAction (mem_Card [0].x, mem_Card [0].y, CardHandler.CardActions.Select);
			canSelect = true;
		} else {
			mem_Card [1] = myCardS;
			mem_Card [1].selectedEffect = (GameObject)Instantiate (myEffect, mem_Card [1].transform.position, Quaternion.identity);
			mem_Card [1].SelectCard ();
			DataHandler.s.SendPlayerAction (mem_Card [1].x, mem_Card [1].y, CardHandler.CardActions.Select);
			canSelect = false;
			Invoke ("CheckCards", checkSpeed);
		}
	}

	void CheckCards () {
		canSelect = true;
		CardChecker.s.CheckNormal (mem_Card);
	}

	public void PowerUpMode (bool state, hook myHook){
		powerUpHook = myHook;
		isPowerUp = state;
		if (mem_Card [1] == null && mem_Card [0] != null) {
			mem_Card [0].UnSelectCard ();
			DataHandler.s.SendPlayerAction (mem_Card [0].x, mem_Card [0].y, CardHandler.CardActions.Select);
		}
	}

	public delegate void hook (IndividualCard card);
	public hook powerUpHook;

	void PowerUpSelect(IndividualCard myCardS){
		if (powerUpHook != null) {
			powerUpHook.Invoke (myCardS);
		}
	}
}
