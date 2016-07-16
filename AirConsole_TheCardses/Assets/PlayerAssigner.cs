﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System;

public class PlayerAssigner : MonoBehaviour {

	//public GameObject PlayerRed;
	//public GameObject PlayerBlue;
	//public Text uiText;
	public Text gameMode;
	public Text gameSetting;
	//public int scorePlayerRed = 0;
	//public int scorePlayerBlue = 0;

	public GameObject winnerPanel;
	public Text winnerText;
	public Text winnerScoreText;

	bool isTimeAt = false;
	int timeRemaining = 200;
	float timeStartPoint = 0;

	bool isGameFinished = false;

	public GameObject gameEndEffect;

	void Start () {
		Debug.Log ("Started");
		winnerPanel.SetActive (false);

		//AirConsole.instance.onMessage += OnMessage;
		//uiText.text = "";
		//uiText.text = "NEED MORE PLAYERS";
		//AirConsole.instance.onConnect += OnConnect;
		//AirConsole.instance.onDisconnect += OnDisconnect;
		if (DataHandler.s == null) {
			gameMode.text = "Free Play";
			gameSetting.text = "Testing!";
		} else {
			if (DataHandler.isTimeAttack) {
				gameMode.text = "Time Attack";
				timeRemaining = DataHandler.gameSetting;
				gameSetting.text = ConverIntToTime(timeRemaining);
				isTimeAt = true;
				timeStartPoint = Time.realtimeSinceStartup;
			} else {
				gameMode.text = "First to Reach";
				gameSetting.text = DataHandler.gameSetting.ToString();
			}
		}
	}

	void Update (){
		if (isGameFinished)
			return;
		if (isTimeAt) {
			int timeSinceBegin = (int)(Time.realtimeSinceStartup - timeStartPoint);

			gameSetting.text = ConverIntToTime (timeRemaining - timeSinceBegin);

			if (timeRemaining - timeSinceBegin <= 0) {
				EndGame ();
			}
		} else {
			
			for (int id = 0; id < 4; id++) {
				int score = 0;
				for (int i = 1; i <= 7; i++) {
					score += ScoreKeeper.s.players [id].Scores [i];
				}

				if (score >= DataHandler.gameSetting) {
					EndGame ();
				}
			}
		}
	}

	void EndGame (){
		isGameFinished = true;

		Instantiate (gameEndEffect, transform.position, transform.rotation);

		PlayerScript[] myPlayers = GameObject.FindObjectsOfType<PlayerScript> ();

		foreach (PlayerScript player in myPlayers) {
			player.isActive = false;
		}

		winnerPanel.SetActive (true);
	
		int[] scores = new int[4];
		string[] names = new string[4];

		for (int id = 0; id < 4; id++) {
			int score = 0;
			for (int i = 1; i <= 7; i++) {
				score += ScoreKeeper.s.players [id].Scores [i];
			}

			scores [id] = score;
			names [id] = AirConsole.instance.GetNickname (AirConsole.instance.ConvertPlayerNumberToDeviceId (id));
		}
			
		//int[] one = { 5, 78, 68, 987, 5, 3, 6, 4 };
		Array.Sort(scores);

		//display winner
		winnerScoreText.text = scores [0] + "\n" + scores [1] + "\n" + scores [2] + "\n" + scores [3];
		winnerText.text = names [0] + "\n" + names [1] + "\n" + names [2] + "\n" + names [3];


	}

	public void BackToMenu(){
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
		/*AirConsole.instance.SetActivePlayers (1);
		return;*/
		if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0) {
			if (AirConsole.instance.GetControllerDeviceIds ().Count >= 2) {
				//StartGame ();
			} else {
				//uiText.text = "NEED MORE PLAYERS";
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
				//StartGame ();
			} else {
				AirConsole.instance.SetActivePlayers (0);
				ResetGame ();
				//uiText.text = "PLAYER LEFT - NEED MORE PLAYERS";
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

	void ResetGame () {

		print ("add reset game code");
	}

	public void UpdateScoreUI () {
		// update text canvas
		//uiText.text = scorePlayerBlue + ":" + scorePlayerRed;
		//uiText.text = "";
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

	string ConverIntToTime (int time) {

		int minuteCount = (int)(time / 60);
		int secondCount = (int)(time - (minuteCount * 60));
		if (secondCount < 10) {
			return minuteCount.ToString () + ":0" + secondCount.ToString ();
		} else {
			return minuteCount.ToString () + ":" + secondCount.ToString ();
		}

	}
}
