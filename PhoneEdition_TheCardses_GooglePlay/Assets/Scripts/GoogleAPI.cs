using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GoogleAPI : MonoBehaviour, RealTimeMultiplayerListener {

	public int playerCount = 2;


	public static GoogleAPI s;
	public List<Participant> participants = new List<Participant>();

	public DataLogger logText;

	public bool canPlay = false;

	// Use this for initialization
	void Awake () 
	{
		if (s != null) {
			Destroy (this.gameObject);
			return;
		} else {
			s = this;
		}

		Application.targetFrameRate = 30;

		DontDestroyOnLoad (this.gameObject);
	}

	void Start () 
	{	
		logText = DataLogger.s;

		//RTClient = PlayGamesPlatform.Instance.RealTime;
		//lobbyGUI.SetActive (false);
		//ChangePlayerCount (0);

		logText.LogMessage("Initialising");
		try{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		//logText.LogMessage("debug enabled");
		PlayGamesPlatform.Activate();

		//GameObject.FindObjectOfType<MPMenu> ().GetComponent<MPMenu> ().GirisYapildiMi (Social.localUser.authenticated);
		}catch{
			logText.LogMessage ("Initialization Failure, PLease Restart the Game");
		}
		logText.LogMessage("Initialization Successful");

		if (Application.internetReachability != NetworkReachability.NotReachable) {
			if (!PlayGamesPlatform.Instance.localUser.authenticated) {
				Login ();
			}
		}
	}


	public List<Participant> GetParticipants()
	{
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants ();
	}


	public Participant GetPbyID (string participantID) 
	{
		return PlayGamesPlatform.Instance.RealTime.GetParticipant (participantID);
	}

	public Participant GetSelf ()
	{
		return PlayGamesPlatform.Instance.RealTime.GetSelf ();
	}

	int n= 1;
	public void Login () {
		
		PlayGamesPlatform.Instance.Authenticate ((bool success) => {
			if (success) {
				logText.LogMessage ("Login Successful");
				canPlay = true;
				MenuManager.s.UpdateMenu ();
			} else {
				if (Application.internetReachability != NetworkReachability.NotReachable) {
					logText.LogMessage ("Login attempt " + n.ToString ());
					n++;
					//Invoke ("Login", 0.5f);
				} else {
					logText.LogMessage ("No Internet Access");
				}
				isOnline = false;
			}
		}, false);
	}

	public void GetQuickMatch ()
	{
		isOnline = true;
		canPlay = false;
		logText.LogMessage("Initiating Search");
		//PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI ();
		//sInstance = new MultiPlayerConnect();
		//const int MinOpponents = 1, MaxOpponents = 1;
		int GameVariant = 0;
		try{
			PlayGamesPlatform.Instance.RealTime.CreateQuickGame((uint)(playerCount - 1), (uint)(playerCount - 1), (uint)GameVariant, this);
		}catch{
			logText.LogMessage("Game Search Failed");
		}
		//logText.LogMessage("Searching for Room";
	}

	void Update () {
		if (logText == null) {
			logText = DataLogger.s;
		}
		//logText.LogMessage(GetSelf ().ParticipantId.ToString();
	}

	public void OnRoomConnected(bool success) {
		logText.LogMessage("OnRoomConnected");
		if (success) 
		{
			logText.LogMessage("Room Connection Successful");
			participants = GetParticipants ();
			SceneMaster.s.LoadMultiplayer ();
		} else 
		{
			logText.LogMessage("Room Connection Failure");
			PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
		}
	}

	private bool showingWaitingRoom = false;

	public void OnRoomSetupProgress(float progress) {
		// show the default waiting room.
		if (!showingWaitingRoom) {
			showingWaitingRoom = true;
			//PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
		}

		if (progress == 20) {
			logText.LogMessage("Searching for enemy");
		} else {
			logText.LogMessage("Connecting " + ((int)progress).ToString() + "%");
		}
	}

	public void OnParticipantLeft (Participant participant) {
		logText.LogMessage(participant.DisplayName + " Left");
		PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
	}

	public void OnPeersConnected(string[] participantIds) {
		foreach (string participant in participantIds) {
			logText.LogMessage(participant + " Joined");
		}
		PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
	}

	public void OnPeersDisconnected(string[] participantIds) {
		foreach (string participant in participantIds) {
			logText.LogMessage(participant + " Disconnected");
		}
		PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
	}

	public void Exit () {
		PlayGamesPlatform.Instance.SignOut();
	}

	public void OnLeftRoom() {
		// display error message and go back to the menu screen

		// (do NOT call PlayGamesPlatform.Instance.RealTime.LeaveRoom() here --
		// you have already left the room!)
		SceneMaster.s.LoadMenu();
	}




	public delegate void ReceiveMessage (byte[]data);
	public ReceiveMessage myReceiver;

	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data) {
		logText.LogMessage("Data received " + ((char)data [0]).ToString ());
		try{
			DataHandler.s.ReceiveData(data);
			//logText.LogMessage("Data processing begun " + ((char)data [0]).ToString());
		}catch{
			logText.LogMessage("Data process failed " + ((char)data [0]).ToString ());
		}
	}

	public bool isOnline = false;
	public void SendMessage (byte[] data){
		if (isOnline) {
			try {
				PlayGamesPlatform.Instance.RealTime.SendMessageToAll (true, data);
				logText.LogMessage ("Data send " + ((char)data [0]).ToString ());
			} catch (System.Exception e) {
				logText.LogMessage ("Data failed to send " + ((char)data [0]).ToString () + " - " + e.StackTrace);
			}
		} else {
			logText.LogMessage ("We are offline",true);
		}
	}
}
