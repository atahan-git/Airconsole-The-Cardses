using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {

	public static PowerUpManager s;


	public enum PowerUpType{Earth,Fire,Ice,Light,Nether,Poison,Shadow}

	PowerUp_Earth pEarth;
	PowerUp_Fire pFire;
	PowerUp_Ice pIce;
	PowerUp_Light pLight;
	PowerUp_Nether pNether;
	PowerUp_Poison pPoison;
	PowerUp_Shadow pShadows;

	public GameObject powerUpMenuPrefab;
	[HideInInspector]
	public GameObject[] powerUpMenuObjects = new GameObject[7];
	public Transform menuParent;

	[HideInInspector]
	public bool canActivatePowerUp = true;

	// Use this for initialization
	void Start () {
		s = this;
		pEarth = GetComponentInChildren<PowerUp_Earth> ();
		pFire = GetComponentInChildren<PowerUp_Fire> ();
		pIce = GetComponentInChildren<PowerUp_Ice> ();
		pLight = GetComponentInChildren<PowerUp_Light> ();
		pNether = GetComponentInChildren<PowerUp_Nether> ();
		pPoison = GetComponentInChildren<PowerUp_Poison> ();
		pShadows = GetComponentInChildren<PowerUp_Shadow> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PowerUpMenuEnable (int type, int curAmount){
		if (curAmount > 0) {
			if (powerUpMenuObjects [type - 8] == null) {
				powerUpMenuObjects [type - 8] = (GameObject)Instantiate (powerUpMenuPrefab, menuParent);
				powerUpMenuObjects [type - 8].transform.ResetTransformation ();

				powerUpMenuObjects [type - 8].GetComponent<DragonPanel> ().type = type;
				powerUpMenuObjects [type - 8].GetComponent<DragonPanel> ().count = curAmount;
			} else {
				powerUpMenuObjects [type - 8].GetComponent<DragonPanel> ().count = curAmount;
			}
		} else {
			if (powerUpMenuObjects [type - 8] != null) {
				Destroy (powerUpMenuObjects [type - 8]);
				powerUpMenuObjects [type - 8] = null;
			}
		}
	}


	public void PowerUpAction (PowerUpType type, bool isActive) {

		switch (type) {
		case PowerUpType.Earth:
			pEarth.Enable ();
			break;
		case PowerUpType.Fire:
			pFire.Enable ();
			break;
		case PowerUpType.Ice:
			pIce.Enable ();
			break;
		case PowerUpType.Light:
			pLight.Enable ();
			break;
		case PowerUpType.Nether:
			pNether.Enable ();
			break;
		case PowerUpType.Poison:
			pPoison.Enable ();
			break;
		case PowerUpType.Shadow:
			pShadows.Enable ();
			break;
		default:
			DataLogger.s.LogMessage ("Unrecognized power up type PUM", true);
			break;
		}
	}

	public void PowerUpButton (int type){
		try {
			if (LocalPlayerController.s.canSelect == false)
				return;
			if(canActivatePowerUp == false)
				return;
			switch (type) {
			case 8:
				PowerUpAction (PowerUpType.Earth, true);
				break;
			case 9:
				PowerUpAction (PowerUpType.Fire, true);
				break;
			case 10:
				PowerUpAction (PowerUpType.Ice, true);
				break;
			case 11:
				PowerUpAction (PowerUpType.Light, true);
				break;
			case 12:
				PowerUpAction (PowerUpType.Nether, true);
				break;
			case 13:
				PowerUpAction (PowerUpType.Poison, true);
				break;
			case 14:
				PowerUpAction (PowerUpType.Shadow, true);
				break;
			default:
				DataLogger.s.LogMessage ("Unrecognized power up button PUM", true);
				break;
			}
			ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerIdentifier, type, -1);
		} catch (System.Exception e) {
			DataLogger.s.LogMessage (e.StackTrace, true);
		}
	}

	public enum ActionType {Enable, Activate, SelectCard, Disable}

	public void ReceiveEnemyPowerUpActions (int player, int x, int y, PowerUpType type, ActionType action) {
		switch (type) {
		case PowerUpType.Earth:
			pEarth.ReceiveAction (player, x, y, action);
			break;
		case PowerUpType.Fire:
			pFire.ReceiveAction (player, x, y, action);
			break;
		case PowerUpType.Ice:
			pIce.ReceiveAction (player, x, y, action);
			break;
		case PowerUpType.Light:
			pLight.ReceiveAction (player, x, y, action);
			break;
		case PowerUpType.Nether:
			pNether.ReceiveAction (player, x, y, action);
			break;
		case PowerUpType.Poison:
			pPoison.ReceiveAction (player, x, y, action);
			break;
		case PowerUpType.Shadow:
			pShadows.ReceiveAction (player, x, y, action);
			break;
		default:
			break;
		}
	}

	public void ChoosePoisonCard (IndividualCard myCard){
		pPoison.ChoosePoisonCard (myCard);
	}
}
