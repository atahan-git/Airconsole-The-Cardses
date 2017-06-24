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

	//public MPSecondPlayer secP;

	// Use this for initialization
	void Awake () 
	{
		logText = GameObject.FindGameObjectWithTag ("LogText").GetComponent<DataLogger>();
		if (s != null) {
			Destroy (this);
			return;
		}
		DontDestroyOnLoad (this.gameObject);
		s = this;
	}

	void Start () 
	{	
		logText = GameObject.FindGameObjectWithTag ("LogText").GetComponent<DataLogger>();

		//RTClient = PlayGamesPlatform.Instance.RealTime;
		//lobbyGUI.SetActive (false);
		//ChangePlayerCount (0);

		logText.LogMessage("Initialising");

		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
		logText.LogMessage("config set");

		PlayGamesPlatform.InitializeInstance(config);
		logText.LogMessage("Initialized instance");
		PlayGamesPlatform.DebugLogEnabled = true;
		//logText.LogMessage("debug enabled");
		PlayGamesPlatform.Activate();
		logText.LogMessage("Activated");

		//GameObject.FindObjectOfType<MPMenu> ().GetComponent<MPMenu> ().GirisYapildiMi (Social.localUser.authenticated);
		if (!PlayGamesPlatform.Instance.localUser.authenticated) {
			Login ();
		}


		logText.LogMessage("Initialization Successful");
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

	public void Login ()
	{
		
		PlayGamesPlatform.Instance.Authenticate((bool success) => 
			{
				if(success)
				{
					logText.LogMessage("Login Successful");
				}
				else
				{
					logText.LogMessage("Login Failure");
					//Login();
				}
			},false);
	}

	public void GetQuickMatch ()
	{
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
			logText = GameObject.FindGameObjectWithTag ("LogText").GetComponent<DataLogger>();
		}
		//logText.LogMessage(GetSelf ().ParticipantId.ToString();
	}

	public void OnRoomConnected(bool success) {
		logText.LogMessage("OnRoomConnected");
		if (success) 
		{
			logText.LogMessage("Room Connection Successful");
			participants = GetParticipants ();
			SceneManager.LoadScene (1);
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
		SceneManager.LoadScene (0);
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

	public void SendMessage (byte[] data){
		try{
			PlayGamesPlatform.Instance.RealTime.SendMessageToAll (true, data);
			logText.LogMessage("Data send " + ((char)data [0]).ToString());
		}catch{
			logText.LogMessage("Data failed to send " + ((char)data [0]).ToString ());
		}
	}
}
