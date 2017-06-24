using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DataHandler : MonoBehaviour {

	public static DataHandler s;

	public char myPlayerIdentifier = 'B';
	public int myPlayerinteger = 0;

	public const char p_blue = 'B';
	public const char p_red = 'R';
	public const char p_green = 'G';
	public const char p_yellow = 'Y';


	const char a_player = 'A';
	const char a_cardtype = 'C';
	const char a_power = 'P';
	const char a_score = 'S';
	const char a_def = 'D';

	DataLogger logText;
	// Use this for initialization
	void Awake () {
		s = this;

		logText = GameObject.FindGameObjectWithTag ("LogText").GetComponent<DataLogger> ();
		//logText.text = "getting started";

		GetPlayerIdentity ();
		//logText.text = "identity received " + myPlayerIdentifier.ToString();
		GoogleAPI.s.myReceiver = ReceiveData;

	}

	void GetPlayerIdentity (){

		if (GoogleAPI.s == null) {
			GoogleAPI.s = GameObject.FindObjectOfType<GoogleAPI> ();
		}

		myPlayerinteger = GoogleAPI.s.participants.IndexOf (GoogleAPI.s.GetSelf ());

		logText.LogMessage (myPlayerinteger.ToString());

		switch (myPlayerinteger) {
		case 0:
			myPlayerIdentifier = p_blue;
			break;
		case 1:
			myPlayerIdentifier = p_red;
			break;
		case 2:
			myPlayerIdentifier = p_green;
			break;
		case 3:
			myPlayerIdentifier = p_yellow;
			break;
		}
	}

	//----------------------------------------------------------Send Data Commands

	const char a_player_select = 'S';	//CardHandler.CardActions.Select;
	const char a_player_unselect = 'U';	//CardHandler.CardActions.UnSelect;
	const char a_player_match = 'M';	//CardHandler.CardActions.Match;
	const char a_player_reopen = 'R';	//CardHandler.CardActions.ReOpen;
	public void SendPlayerAction (int x, int y, CardHandler.CardActions action) {

		List<byte> toSend = new List<byte>();

		toSend.AddRange(System.BitConverter.GetBytes (x));
		toSend.AddRange(System.BitConverter.GetBytes (y));

		switch (action) {
		case CardHandler.CardActions.Select:
			toSend.Add((byte) a_player_select);
			break;
		case CardHandler.CardActions.UnSelect:
			toSend.Add((byte) a_player_unselect);
			break;
		case CardHandler.CardActions.Match:
			toSend.Add((byte) a_player_match);
			break;
		case CardHandler.CardActions.ReOpen:
			toSend.Add((byte) a_player_reopen);
			break;
		default:
			toSend.Add((byte) a_def);
			break;
		}


		SendData (a_player, toSend.ToArray ());
	}

	public void SendCardType (int x, int y, int type){

		//logText.LogMessage("Sending card type initiate");

		List<byte> toSend = new List<byte>();
		toSend.AddRange(System.BitConverter.GetBytes (x));
		toSend.AddRange(System.BitConverter.GetBytes (y));
		toSend.AddRange(System.BitConverter.GetBytes (type));


		//logText.LogMessage("Sending card type list made");
		SendData (a_cardtype, toSend.ToArray ());
	}

	const char a_power_earth 	= 'E';	//PowerUpManager.PowerUpType.Earth;
	const char a_power_fire 	= 'F';	//PowerUpManager.PowerUpType.Fire;
	const char a_power_ice 		= 'I';	//PowerUpManager.PowerUpType.Ice;
	const char a_power_light 	= 'L';		//PowerUpManager.PowerUpType.Light;
	const char a_power_nether 	= 'N';	//PowerUpManager.PowerUpType.Nether;
	const char a_power_poison 	= 'P';	//PowerUpManager.PowerUpType.Poison;
	const char a_power_shadow 	= 'S';	//PowerUpManager.PowerUpType.Shadow;

	const char a_power_activate = 'A';	//PowerUpManager.ActionType.Activate;
	const char a_power_disable 	= 'D';	//PowerUpManager.ActionType.Disable;
	const char a_power_enable 	= 'Y';	//PowerUpManager.ActionType.Enable;
	const char a_power_select 	= 'T';	//PowerUpManager.ActionType.SelectCard;
	public void SendPowerUpAction (int x, int y, PowerUpManager.PowerUpType type, PowerUpManager.ActionType action){

		List<byte> toSend = new List<byte>();

		toSend.AddRange(System.BitConverter.GetBytes (x));
		toSend.AddRange(System.BitConverter.GetBytes (y));

		switch (type) {
		case PowerUpManager.PowerUpType.Earth:
			toSend.Add((byte) a_power_earth);
			break;
		case PowerUpManager.PowerUpType.Fire:
			toSend.Add((byte) a_power_fire);
			break;
		case PowerUpManager.PowerUpType.Ice:
			toSend.Add((byte) a_power_ice);
			break;
		case PowerUpManager.PowerUpType.Light:
			toSend.Add((byte) a_power_light);
			break;
		case PowerUpManager.PowerUpType.Nether:
			toSend.Add((byte) a_power_nether);
			break;
		case PowerUpManager.PowerUpType.Poison:
			toSend.Add((byte) a_power_poison);
			break;
		case PowerUpManager.PowerUpType.Shadow:
			toSend.Add((byte) a_power_shadow);
			break;
		default:
			toSend.Add((byte) a_def);
			break;
		}

		switch (action) {
		case PowerUpManager.ActionType.Activate:
			toSend.Add((byte) a_power_activate);
			break;
		case PowerUpManager.ActionType.Disable:
			toSend.Add((byte) a_power_disable);
			break;
		case PowerUpManager.ActionType.Enable:
			toSend.Add((byte) a_power_enable);
			break;
		case PowerUpManager.ActionType.SelectCard:
			toSend.Add((byte) a_power_select);
			break;
		default:
			toSend.Add((byte) a_def);
			break;
		}



		SendData (a_player, toSend.ToArray ());
	}

	public void SendScore (char player, int scoreType, int toAdd){

		List<byte> toSend = new List<byte>();
		toSend.Add ((byte)player);
		toSend.Add ((byte)scoreType);
		toSend.Add ((byte)toAdd);

		GoogleAPI.s.SendMessage (toSend.ToArray());
	}

	public void SendData (char command, byte[] data){

		List<byte> toSend = new List<byte>();
		toSend.Add ((byte)command);
		toSend.Add ((byte)myPlayerIdentifier);
		foreach (byte b in data) {
			toSend.Add (b);
		}

		//logText.LogMessage("Sending data " + command.ToString ());
		GoogleAPI.s.SendMessage (toSend.ToArray());
	}


	//----------------------------------------------------------Receive Data Handlers

	public void ReceiveData (byte[] data){
		/*if (secP != null) {
			char Tip = (char)data [0];
			switch (Tip) {
			case 'P':
				Vector3 ikiP;

				float posX = System.BitConverter.ToSingle (data, 1);
				float posY = System.BitConverter.ToSingle (data, 4 + 1);
				float posZ = System.BitConverter.ToSingle (data, 4 + 4 + 1);

				break
		}*/

		//logText.LogMessage ("Data is being processed ");

		char myCommand = (char)data [0];

		try {
			switch (myCommand) {
			case a_player:
				ReceivePlayerAction (data);
				break;
			case a_cardtype:
				ReceiveCardType (data);
				break;
			case a_power:
				ReceivePowerUpAction (data);
				break;
			case a_score:
				ReceiveScore (data);
				break;
			case a_def:
				logText.LogMessage ("Unknown command",true);
				break;
			}

			//logText.LogMessage ("Data processing done " + myCommand.ToString ());
		} catch {
			logText.LogMessage ("Data processing failed " + myCommand.ToString (),true);
		}
	}

	void ReceivePlayerAction (byte[] data){
		char player;
		int x;
		int y;
		CardHandler.CardActions action;

		player = (char)data [1];
		x = System.BitConverter.ToInt32 (data, 2);
		y = System.BitConverter.ToInt32 (data, 2 + 4);
		char cardAction = (char)data [2 + 4 + 4];

		switch (cardAction) {
		case a_player_select:
			action = CardHandler.CardActions.Select;
			break;
		case a_player_unselect:
			action = CardHandler.CardActions.UnSelect;
			break;
		case a_player_match:
			action = CardHandler.CardActions.Match;
			break;
		case a_player_reopen:
			action = CardHandler.CardActions.ReOpen;
			break;
		default:
			logText.LogMessage ("Unknown card action", true);
			action = CardHandler.CardActions.UnSelect;
			break;
		}

		EnemyPlayerHandler.s.ReceiveAction (player, x, y, action);
	}

	void ReceiveCardType (byte[] data){
		int x;
		int y;
		int type;

		x = System.BitConverter.ToInt32 (data, 2);
		y = System.BitConverter.ToInt32 (data, 2 + 4);
		type = System.BitConverter.ToInt32 (data, 2 + 4 + 4);


		CardHandler.s.UpdateCardType (x, y, type);
	}

	void ReceivePowerUpAction (byte[] data){
		int player;
		int x;
		int y;
		PowerUpManager.PowerUpType type;
		PowerUpManager.ActionType action;


		player = (char)data [1];
		x = System.BitConverter.ToInt32 (data, 2);
		y = System.BitConverter.ToInt32 (data, 2 + 4);
		char pType = (char)data [2 + 4 + 4];
		char pAction = (char)data [2 + 4 + 4 + 1];

		switch (pType) {
		case a_power_earth:
			type = PowerUpManager.PowerUpType.Earth;
			break;
		case a_power_fire:
			type = PowerUpManager.PowerUpType.Fire;
			break;
		case a_power_ice:
			type = PowerUpManager.PowerUpType.Ice;
			break;
		case a_power_light:
			type = PowerUpManager.PowerUpType.Light;
			break;
		case a_power_nether:
			type = PowerUpManager.PowerUpType.Nether;
			break;
		case a_power_poison:
			type = PowerUpManager.PowerUpType.Poison;
			break;
		case a_power_shadow:
			type = PowerUpManager.PowerUpType.Shadow;
			break;
		default:
			logText.LogMessage ("Unknown power up action",true);
			type = PowerUpManager.PowerUpType.Fire;
			break;
		}

		switch (pAction) {
		case a_power_activate:
			action = PowerUpManager.ActionType.Activate;
			break;
		case a_power_disable:
			action = PowerUpManager.ActionType.Disable;
			break;
		case a_power_enable:
			action = PowerUpManager.ActionType.Enable;
			break;
		case a_power_select:
			action = PowerUpManager.ActionType.SelectCard;
			break;
		default:
			//Error
			action = PowerUpManager.ActionType.Disable;
			break;
		}

		PowerUpManager.s.ReceiveEnemyPowerUpActions (player, x, y, type, action);
	}

	public void ReceiveScore (byte[] data){
		char player;
		int scoreType;
		int toAdd;

		player = (char)data [1];
		scoreType = System.BitConverter.ToInt32 (data, 2);
		toAdd = System.BitConverter.ToInt32 (data, 2 + 4);

		ScoreBoardManager.s.ReceiveScore (player, scoreType, toAdd);
	}
}
