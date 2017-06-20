using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobyController : NetworkBehaviour {


	public int playerCount = 2;
	public Text textPlayer;

	NetworkLobbyManager manager;

	public GameObject startGUI;
	public GameObject lobbyGUI;

	// Use this for initialization
	void Start () {
		manager = GetComponent<NetworkLobbyManager> ();
		lobbyGUI.SetActive (false);
		ChangePlayerCount (0);
	}

	NetworkLobbyPlayer[] oldSlots;
	GameObject[] myPanels;
	// Update is called once per frame
	void Update () {

		if (SceneManager.GetActiveScene ().buildIndex == 0) {
			if (textPlayer == null) {
				textPlayer = GameObject.Find ("PlayerText").GetComponent<Text> ();
				startGUI = GameObject.Find ("Start GUI");
				lobbyGUI = GameObject.Find ("Lobby GUI");
				GameObject.Find ("ButtonUp").GetComponent<Button> ().onClick.AddListener (IncreasePlayerCount);
				GameObject.Find ("ButtonDown").GetComponent<Button> ().onClick.AddListener (DecreasePlayerCount);
				GameObject.Find ("Host").GetComponent<Button> ().onClick.AddListener (HostaGame);
				GameObject.Find ("Join").GetComponent<Button> ().onClick.AddListener (JoinaGame);
				GameObject.Find ("Back").GetComponent<Button> ().onClick.AddListener (ExitaGame);
				lobbyGUI.SetActive (false);
				startGUI.SetActive (true);
			}
				
		}

		/*if (oldSlots != manager.lobbySlots) {

			foreach (GameObject gam in myPanels) {
				if (gam != null)
					Destroy (gam.gameObject);
			}

			myPanels = new GameObject[manager.lobbySlots.Length];
			int n = 0;
			foreach (NetworkLobbyPlayer myPLayer in manager.lobbySlots) {

				GameObject thisPanel = (GameObject)Instantiate (panelPrefab, panelParent.transform);
				myPanels [n] = thisPanel;
				n++;

				thisPanel.GetComponent<LobyPlayerPanel> ().playerid = myPLayer.slot;
				thisPanel.GetComponent<LobyPlayerPanel> ().playerState = myPLayer.readyToBegin;
				//if()

			}
		}

		oldSlots = manager.lobbySlots;*/
	}

	public void IncreasePlayerCount () {
		ChangePlayerCount (1);

	}

	public void DecreasePlayerCount () {
		ChangePlayerCount (-1);
	}

	void  ChangePlayerCount (int amount){

		playerCount += amount;
		playerCount = Mathf.Clamp (playerCount, 1, 4);
		textPlayer.text = playerCount.ToString ();

		manager.maxPlayers = playerCount;
		manager.minPlayers = playerCount;
	}

	public bool isHost = false;

	public void HostaGame () {

		startGUI.SetActive (false);
		lobbyGUI.SetActive (true);

		manager.StartHost ();
		isHost = true;
	}

	public void JoinaGame () {

		startGUI.SetActive (false);
		lobbyGUI.SetActive (true);

		manager.StartClient ();
		isHost = false;
	}

	public void ExitaGame () {

		startGUI.SetActive (true);
		lobbyGUI.SetActive (false);

		if (isHost) {
			manager.StopHost ();
		} else {
			manager.StopClient ();
		}

		//UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

}
