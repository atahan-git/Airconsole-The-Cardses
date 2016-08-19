using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {

	public Cursor cursor;

	[Header("Main Manu")]	//0
	public GameObject mainMenu;
	public MenuItem start;		//0
	public MenuItem tutorial;	//-1

	[Header("Tutorial")]	//1
	public GameObject tutorialMenu;
	public MenuItem back; 		//0

	[Header("Start")]		//2
	public GameObject startMenu;
	public MenuItem timeAttack;	//3  =  1
	public MenuItem score;		//3  =  0
	public MenuItem number;		//2  =  0
	public MenuItem addLow;		//2  =  1
	public MenuItem addHigh;	//2  =  2
	public MenuItem remLow;		//2  = -1
	public MenuItem remHigh;	//2  = -2
	public MenuItem small;		//1  =  0
	public MenuItem big;		//1  =  1
	public MenuItem start2;		//0  =  0
	public MenuItem back2; 		//0  =  1

	public int x = 0;
	public int y = 0;
	public int curMenu = 0;

	int myDeviceId = -1;
	MenuItem curItem;

	/*	^
	 * 	|
	 * 	x
	 * 	|
	 * 	|
	 * 	|
	 * 	0-----------y->
	 */


	public Vector2 sizeSmall = new Vector2 (12, 4);
	public Vector2 sizeBig = new Vector2 (14, 5);
	public Text textValue;
	public bool isTimeAttack = false;
	public int modeValue = 30;

	// Use this for initialization

	void Awake () {
		AirConsole.instance.onMessage += OnMessage;
		DataHandler.gridSizeX = (int)sizeSmall.x;
		DataHandler.gridSizeY = (int)sizeSmall.y;
		/*AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;*/
	}


	void OnDestroy(){

		AirConsole.instance.onMessage -= OnMessage;
	}

	void Start () {
		cursor.goToPos = start.pos;
		cursor.instaGo ();
		cursor.size = start.size;
		curItem = start;
		ChangeGameMode (0);
	}


	
	// Update is called once per frame
	void Update () {
	
	}

	/*void OnConnect (int device_id){
		if (myDeviceId == -1)
			myDeviceId == device_id;
	}

	void OnDisconnect (int device_id){

	}*/


	void OnMessage (int device_id, JToken data) {

		//int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		//if (active_player != -1) {
			//print ("player is legit");
			//if (active_player == id) {
				//print ("player is us " + id);
				//print ("got something4");
				if (data ["swipedigital-left"] != null) {
					//print ("TryMove");

					Move (data);

				} else if (data ["Select"] != null) {
					if ((bool)data ["Select"] ["pressed"]) {

						PressSelect ();
					}
				}
			//}
		//}
	}

	Coroutine moveRoutine;

	void Move(JToken data){

		if ((bool)data ["swipedigital-left"] ["pressed"]) {
			//print ("got something5");

			if ((bool)data ["swipedigital-left"] ["message"] ["right"])
				curItem = MoveAround (0, 1);
			if((bool)data ["swipedigital-left"] ["message"] ["left"])
				curItem = MoveAround (0, -1);
			if((bool)data ["swipedigital-left"] ["message"] ["up"])
				curItem = MoveAround (1, 0);
			if((bool)data ["swipedigital-left"] ["message"] ["down"])
				curItem = MoveAround (-1, 0);

			cursor.goToPos = curItem.pos;
			cursor.size = curItem.size;
		}
			
	}

	void PressSelect () {
		if (curItem.functionToCall != null) {
			curItem.functionToCall.Invoke ();
		}
	}
		

	MenuItem MoveAround(int moveX, int moveY){

		switch (curMenu) {
		//-------------------------------------------------------------Main Menu
		case 0:
			
			if (moveX == 1 && x == -1) {
				x = 0;
				return start;
			}
			if (moveX == -1 && x == 0) {
				x = -1;
				return tutorial;
			}
			return curItem;

		//-------------------------------------------------------------Tutorial
			break;
		case 1:

			return back;


			//-------------------------------------------------------------Start
			//start 2 =>
			break;
		case 2:

			//-----------------------------------------------start2 from and to
			if (x == 0 && moveX == 1) {
				x = 1;
				y = 0;
				return small;
			}

			if (x == 0 && moveY == 1 && y == 0) {
				x = 0;
				y = 1;
				return back2;
			}
			if (x == 0 && moveY == -1 && y == 1) {
				x = 0;
				y = 0;
				return start2;
			}

			if (x == 1 && moveX == -1) {
				x = 0;
				y = 0;
				return start2;
			}

			//-----------------------------------------------size options

			if (x == 1 && moveY == 1 && y == 0) {
				x = 1;
				y = 1;
				return big;
			}

			if (x == 1 && moveY == -1 && y == 1) {
				x = 1;
				y = 0;
				return small;
			}

			if (x == 1 && moveX == 1) {
				x = 2;
				y = 0;
				return number;
			}

			if (x == 2 && moveX == -1) {
				x = 1;
				y = 0;
				return small;
			}

			//-----------------------------------------------Numbers

			if (x == 2 && moveY == 1 && y == 0) {
				x = 2;
				y = 1;
				return addLow;
			}
			if (x == 2 && moveY == 1 && y == 1) {
				x = 2;
				y = 2;
				return addHigh;
			}
			if (x == 2 && moveY == -1 && y == 2) {
				x = 2;
				y = 1;
				return addLow;
			}
			if (x == 2 && moveY == -1 && y == 1) {
				x = 2;
				y = 0;
				return number;
			}
			if (x == 2 && moveY == -1 && y == 0) {
				x = 2;
				y = -1;
				return remLow;
			}
			if (x == 2 && moveY == -1 && y == -1) {
				x = 2;
				y = -2;
				return remHigh;
			}
			if (x == 2 && moveY == 1 && y == -2) {
				x = 2;
				y = -1;
				return remLow;
			}
			if (x == 2 && moveY == 1 && y == -1) {
				x = 2;
				y = 0;
				return number;
			}

			if (x == 2 && moveX == 1) {
				x = 3;
				y = 0;
				return score;
			}
			if (x == 3 && moveX == -1) {
				x = 2;
				y = 0;
				return number;
			}

			//-----------------------------------------------GameModes

			if (x == 3 && moveY == 1 && y == 0) {
				x = 3;
				y = 0;
				return timeAttack;
			}
			if (x == 3 && moveY == -1 && y == 1) {
				x = 3;
				y = 1;
				return score;
			}

			return curItem;

			break;

		default:
			Debug.LogError ("Weird menu shit happeneed " + curMenu); 
			break;
		}

		return curItem;
	}


	public void ChangeMenu(int menuNumber){

		if (menuNumber == 3 && AirConsole.instance.GetActivePlayerDeviceIds.Count < 2)
			return;

		mainMenu.SetActive (false);
		tutorialMenu.SetActive (false);
		startMenu.SetActive (false);

		switch (menuNumber) {
		case 0:
			curItem = start;
			mainMenu.SetActive (true);
			break;
		case 1:
			curItem = back;
			tutorialMenu.SetActive (true);
			break;
		case 2:
			curItem = start2;
			startMenu.SetActive (true);
			break;

		case 3:
			//satatrty and shet
			Starter.s.StartGame();
			break;
		}

		y = 0;
		curMenu = menuNumber;

		cursor.goToPos = curItem.pos;
		cursor.size = curItem.size;
		cursor.instaGo ();
	}

	public void ChangeGameMode(int mode){//0 score --- 1 time
		if (mode == 0) {
			isTimeAttack = false;
			modeValue = 30;
		} else {
			isTimeAttack = true;
			modeValue = 120;
		}

		AddValue (0);
	}

	public void ChangeGridSize(int size){//0 small --- 1 big

	}

	public void AddValue (int value){//1 => 1 or 10 sec --- 10 => 10 or 1 min

		if (!isTimeAttack) {

			modeValue += value;
			modeValue = Mathf.Clamp (modeValue, 1, 200);

			textValue.text = modeValue.ToString();

		} else {

			if (Mathf.Abs (value) > 5) {
				modeValue += value * 6;

			} else {
				modeValue += value * 10;

			}

			modeValue = Mathf.Clamp (modeValue, 10, 600);

			int minuteCount = (int)(modeValue / 60);
			int secondCount = (int)(modeValue - (minuteCount * 60));

			if (secondCount < 10) {
				textValue.text = minuteCount.ToString () + ":0" + secondCount.ToString ();
			} else {
				textValue.text = minuteCount.ToString () + ":" + secondCount.ToString ();
			}

		}
	}

	[System.Serializable]
	public class MenuItem {
		public Transform pos;
		public float size;
		[System.Serializable]
		public class MyEventType : UnityEngine.Events.UnityEvent {}
		public MyEventType functionToCall;
	}
}
