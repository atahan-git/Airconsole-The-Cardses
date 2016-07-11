using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class PlayerAssigner : MonoBehaviour {

	public GameObject PlayerRed;
	public GameObject PlayerBlue;
	public Text uiText;
	public int scorePlayerRed = 0;
	public int scorePlayerBlue = 0;

	void Awake () {
		//AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

	/// <summary>
	/// We start the game if 2 players are connected and the game is not already running (activePlayers == null).
	/// 
	/// NOTE: We store the controller device_ids of the active players. We do not hardcode player device_ids 1 and 2,
	///       because the two controllers that are connected can have other device_ids e.g. 3 and 7.
	///       For more information read: http://developers.airconsole.com/#/guides/device_ids_and_states
	/// 
	/// </summary>
	/// <param name="device_id">The device_id that connected</param>
	void OnConnect (int device_id) {
		if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0) {
			if (AirConsole.instance.GetControllerDeviceIds ().Count >= 2) {
				StartGame ();
			} else {
				uiText.text = "NEED MORE PLAYERS";
			}
		}
	}

	/// <summary>
	/// If the game is running and one of the active players leaves, we reset the game.
	/// </summary>
	/// <param name="device_id">The device_id that has left.</param>
	void OnDisconnect (int device_id) {
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if (active_player != -1) {
			if (AirConsole.instance.GetControllerDeviceIds ().Count >= 2) {
				StartGame ();
			} else {
				AirConsole.instance.SetActivePlayers (0);
				ResetGame ();
				uiText.text = "PLAYER LEFT - NEED MORE PLAYERS";
			}
		}
	}

	/// <summary>
	/// We check which one of the active players has moved the paddle.
	/// </summary>
	/// <param name="from">From.</param>
	/// <param name="data">Data.</param>
	/*void OnMessage (int device_id, JToken data) {
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if (active_player != -1) {
			if (active_player == 0) {
				this.racketLeft.velocity = Vector3.up * (float)data ["move"];
			}
			if (active_player == 1) {
				this.racketRight.velocity = Vector3.up * (float)data ["move"];
			}
		}
	}*/

	void StartGame () {
		AirConsole.instance.SetActivePlayers (2);
		ResetGame ();
		scorePlayerRed = 0;
		scorePlayerBlue = 0;
		UpdateScoreUI ();
	}

	void ResetGame () {

		print ("add reset game code");
	}

	public void UpdateScoreUI () {
		// update text canvas
		uiText.text = scorePlayerBlue + ":" + scorePlayerRed;
	}

	void FixedUpdate () {

		// check if ball reached one of the ends
		/*if (this.ball.position.x < -9f) {
			scoreRacketRight++;
			UpdateScoreUI ();
			ResetBall (true);
		}

		if (this.ball.position.x > 9f) {
			scoreRacketLeft++;
			UpdateScoreUI ();
			ResetBall (true);
		}*/
	}

	/*void OnDestroy () {

		// unregister airconsole events on scene change
		if (AirConsole.instance != null) {
			AirConsole.instance.onMessage -= OnMessage;
		}
	}*/
}
