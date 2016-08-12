using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class PlayerScript : MonoBehaviour {

	//public PowerUpDealer.PowerUpTypes curPowerUp = PowerUpDealer.PowerUpTypes.noUp;

	public int id = 0;
	public Vector3 playerPos;

	public CardGenerator cardGen;
	public PlayerAssigner master;
	public PowerUpDealer powerUp;

	float animSpeed = 20f;
	//float moveSpeed = 0.3f;//lower the better
	float CheckSpeed = 0.3f;
	//public float JoystickDeadZone = 0.5f;

	[HideInInspector]
	public CardScript[] rotatedCards = new CardScript[2];
	[HideInInspector]
	public GameObject[] playerEffectMem = new GameObject[2];

	public bool isActive = true;
	public bool canMove = true;
	public bool canSelect = true;

	public GameObject playerEffect;

	// Use this for initialization
	void Start () {
		if (DataHandler.s == null) {
			/*if (id == 2 || id == 3) {
				Destroy (gameObject);
				return;
			}*/
			AirConsole.instance.onMessage += OnMessage;

			if (CardGenerator.s.gridSizeY >= 5) {
				float myScale = 0.85f;
				transform.localScale = new Vector3 (myScale, myScale, myScale);
			}
			return;
		} else if (DataHandler.gridSizeY >= 5) {
			float myScale = 0.85f;
			transform.localScale = new Vector3 (myScale, myScale, myScale);
		}
		/*if (AirConsole.instance.GetNickname (AirConsole.instance.ConvertPlayerNumberToDeviceId (id)) == "Guest 0")
			Destroy (gameObject);*/

		AirConsole.instance.onMessage += OnMessage;
	}
	
	// Update is called once per frame
	void Update () {

		playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY -1), 0);
		transform.position = Vector3.Lerp(transform.position, cardGen.grid[(int)playerPos.x, (int)playerPos.y], animSpeed * Time.deltaTime);
		#if UNITY_EDITOR
		if(!canMove)
			return;
		if(id == 0 ){
			if(Input.GetKeyDown(KeyCode.W))
				playerPos = AddVectors (playerPos, new Vector3 (0, 1, 0));
			if(Input.GetKeyDown(KeyCode.S))
				playerPos = AddVectors (playerPos, new Vector3 (0, -1, 0));
			if(Input.GetKeyDown(KeyCode.D))
				playerPos = AddVectors (playerPos, new Vector3 (1, 0, 0));
			if(Input.GetKeyDown(KeyCode.A))
				playerPos = AddVectors (playerPos, new Vector3 (-1, 0, 0));
			if(Input.GetKeyDown(KeyCode.Space))
				PressSelect();



		}else if(id == 1){
			if(Input.GetKeyDown(KeyCode.UpArrow))
				playerPos = AddVectors (playerPos, new Vector3 (0, 1, 0));
			if(Input.GetKeyDown(KeyCode.DownArrow))
				playerPos = AddVectors (playerPos, new Vector3 (0, -1, 0));
			if(Input.GetKeyDown(KeyCode.RightArrow))
				playerPos = AddVectors (playerPos, new Vector3 (1, 0, 0));
			if(Input.GetKeyDown(KeyCode.LeftArrow))
				playerPos = AddVectors (playerPos, new Vector3 (-1, 0, 0));
			if(Input.GetKeyDown(KeyCode.RightShift))
				PressSelect();
		}

		//playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY -1), 0);
		#endif
	
	}

	void OnMessage (int device_id, JToken data) {
		if (!isActive)
			return;

		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if (active_player != -1) {
			//print ("player is legit");
			if (active_player == id) {
				//print ("player is us " + id);
				//print ("got something4");
				print(data);
				if (data ["swipedigital-left"] != null) {
					//print ("TryMove");
					if(!canMove)
						return;
					Move (data);

				} else if (data ["Select"] != null) {
					if ((bool)data ["Select"] ["pressed"]) {

						if (!canSelect)
							return;

						PressSelect ();
					}
				} else if (data ["swipepattern-right"] != null) {
					powerUp.UsePowerUp (data);
				}
			}
		}
	}

	Coroutine moveRoutine;

	void Move(JToken data){

		if ((bool)data ["swipedigital-left"] ["pressed"]) {
			//print ("got something5");

			if((bool)data ["swipedigital-left"] ["message"] ["right"])
				playerPos = AddVectors (playerPos, new Vector3 (1, 0, 0));
			if((bool)data ["swipedigital-left"] ["message"] ["left"])
				playerPos = AddVectors (playerPos, new Vector3 (-1, 0, 0));
			if((bool)data ["swipedigital-left"] ["message"] ["up"])
				playerPos = AddVectors (playerPos, new Vector3 (0, 1, 0));
			if((bool)data ["swipedigital-left"] ["message"] ["down"])
				playerPos = AddVectors (playerPos, new Vector3 (0, -1, 0));

		}
		playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY - 1), 0);
	}

	void PressSelect(){

		CardScript myCardS = cardGen.allCards [(int)playerPos.x, (int)playerPos.y].GetComponent<CardScript> ();

		if (myCardS.cardType == 0)
			return;
		if (myCardS.isSelected)
			return;
		if (rotatedCards [1] != null)
			return;

		myCardS.RotateSelf ();
		myCardS.isSelected = true;

		if (powerUp.isShadowActive) {
			powerUp.ShadowSelect(myCardS);
			return;
		}

		GameObject temp = (GameObject)Instantiate (playerEffect, myCardS.transform.position, myCardS.transform.rotation);

		/*if(myCardS.cardType >= 8){
			StartCoroutine (DragonCheck(myCardS, temp));
			return;
		}*/

		if (playerEffectMem [0] == null) {
			playerEffectMem [0] = temp;
		} else {
			playerEffectMem [1] = temp;
		}
		if (powerUp.isEarthActive) {
			powerUp.EarthPlace ();
		}

		if (rotatedCards [0] == null) {
			rotatedCards [0] = myCardS;
			Invoke ("UnSelectOneCard", 10f);
			if (powerUp.isEarthActive) {
				powerUp.Invoke("EarthPreCheck", CheckSpeed);
			}
		} else {
			rotatedCards [1] = myCardS;
			CancelInvoke ("UnSelectOneCard");
			powerUp.CancelInvoke ("EarthPreCheck");
			Invoke ("CheckCards", CheckSpeed);
		}
	}

	IEnumerator DragonCheck(CardScript myCardS, GameObject temp){

		Instantiate (myCardS.getEffect, myCardS.transform.position, myCardS.transform.rotation);

		yield return new WaitForSeconds (CheckSpeed * 3);

		ScoreKeeper.s.AddScore(id, myCardS.cardType, 1);

		if (powerUp.isPoisonActive) {
			powerUp.PoisonDragon (myCardS.cardType);
		}

		myCardS.cardType = 0;	
		myCardS.isSelected = false;

		Destroy (temp);
	}

	void CheckCards(){

		foreach (GameObject gam in playerEffectMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}

		if (powerUp.isEarthActive) {
			powerUp.EarthCheck();
			return;
		}

		if (powerUp.isPoisonActive) {
			powerUp.PoisonCheck(rotatedCards);
			return;
		}

		//got them correct
		if (rotatedCards [0].cardType == rotatedCards [1].cardType) {
			int myCardType = rotatedCards [0].cardType;

			rotatedCards [0].cardType = 0;	
			rotatedCards [0].isSelected = false;
			rotatedCards [0] = null;
			rotatedCards [1].cardType = 0;
			rotatedCards [1].isSelected = false;
			rotatedCards [1] = null;

			ScoreKeeper.s.AddScore(id, myCardType, 1);

		} else {
			rotatedCards [0].RotateSelf ();
			rotatedCards [0].isSelected = false;
			rotatedCards [0] = null;
			rotatedCards [1].RotateSelf ();
			rotatedCards [1].isSelected = false;
			rotatedCards [1] = null;
		}
	}
		

	Vector3 AddVectors(Vector3 vec1, Vector3 vec2){
		return new Vector3 (vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
	}

	void UnSelectOneCard(){
		rotatedCards [0].RotateSelf ();
		rotatedCards [0].isSelected = false;
		rotatedCards [0] = null;

		foreach (GameObject gam in playerEffectMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}
	}

	//old move script
	/*void Move(JToken data){
		
		if ((bool)data ["dpadrelative-left"] ["pressed"]) {
			//print ("got something5");

			switch ((string)data ["dpadrelative-left"] ["message"] ["direction"]) {
			case "right":
				playerPos = AddVectors (playerPos, new Vector3 (1, 0, 0));
				break;
			case "left":
				playerPos = AddVectors (playerPos, new Vector3 (-1, 0, 0));
				break;
			case "up":
				playerPos = AddVectors (playerPos, new Vector3 (0, 1, 0));
				break;
			case "down":
				playerPos = AddVectors (playerPos, new Vector3 (0, -1, 0));
				break;
			default:
				print ("error");
				break;
			}

		}
		playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY - 1), 0);
	}*/

	//other old move
	/*void Move(JToken data){
		//print ("Moving");
		//start moving
		print(data);
		if ((bool)data ["dpadrelative-left"] ["pressed"]) {
			//print ("got something5");

			switch ((string)data ["dpadrelative-left"] ["message"] ["direction"]) {
			case "right":
				print ("Right");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
				moveRoutine = StartCoroutine (MoveStep (new Vector3 (1, 0, 0), moveSpeed));
				break;
			case "left":
				print ("Left");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
				moveRoutine = StartCoroutine (MoveStep (new Vector3 (-1, 0, 0), moveSpeed));
				break;
			case "up":
				print ("Up");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
				moveRoutine = StartCoroutine (MoveStep (new Vector3 (0, 1, 0), moveSpeed));
				break;
			case "down":
				print ("Down");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
				moveRoutine = StartCoroutine (MoveStep (new Vector3 (0, -1, 0), moveSpeed));
				break;
			default:
				print ("error");
				break;
			}

			// stop moving
		} else {
			print ("stopped moving");
			try {
				StopCoroutine (moveRoutine);
			} catch {}
		}

	}

	IEnumerator MoveStep(Vector3 moveAm, float repeatRate) {
		//print ("started to move");
		while(true) {
			playerPos = AddVectors (playerPos, moveAm);
			playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY - 1), 0);
			//print (moveAm);
			yield return new WaitForSeconds(repeatRate);
		}
	}*/

	/*bool moveLock = false;

	void Move(JToken data){
		//print ("Moving");
		//start moving
		print(data);
		if ((bool)data ["joystick-left"] ["pressed"]) {
			//print ("got something5");

			float x = (float)data ["joystick-left"] ["message"] ["x"];
			float y = -(float)data ["joystick-left"] ["message"] ["y"];
			//is x or y is first
			if (Mathf.Abs (x) < JoystickDeadZone && Mathf.Abs (y) < JoystickDeadZone) {
				print ("stopped moving");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
			}

			if (Mathf.Abs (x) >= Mathf.Abs (y)) {
				//is x big or small enough
				if (x > JoystickDeadZone) {
					print ("Right");
					try {
						StopCoroutine (moveRoutine);
					} catch {}
					moveRoutine = StartCoroutine (MoveStep (new Vector3 (1, 0, 0), moveSpeed));

				} else if (x < -JoystickDeadZone) {
					print ("Left");
					try {
						StopCoroutine (moveRoutine);
					} catch {}
					moveRoutine = StartCoroutine (MoveStep (new Vector3 (-1, 0, 0), moveSpeed));

				}
			} else {
				if (y > JoystickDeadZone) {
					print ("Up");
					try {
						StopCoroutine (moveRoutine);
					} catch {}
					moveRoutine = StartCoroutine (MoveStep (new Vector3 (0, 1, 0), moveSpeed));

				} else if (y < -JoystickDeadZone) {
					print ("Down");
					try {
						StopCoroutine (moveRoutine);
					} catch {}
					moveRoutine = StartCoroutine (MoveStep (new Vector3 (0, -1, 0), moveSpeed));

				}
			}



			// stop moving
		} else {
			print ("stopped moving");
			try {
				StopCoroutine (moveRoutine);
			} catch {}
		}

	}

	IEnumerator MoveStep(Vector3 moveAm, float repeatRate) {
		//print ("started to move");
		while (moveLock) {
			yield return 0;
		}
		while(true) {
			moveLock = true;
			Invoke ("EndLock", repeatRate);
			playerPos = AddVectors (playerPos, moveAm);
			playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY - 1), 0);
			//print (moveAm);
			yield return new WaitForSeconds(repeatRate);
		}
	}

	void EndLock(){
		moveLock = false;
	}*/
}