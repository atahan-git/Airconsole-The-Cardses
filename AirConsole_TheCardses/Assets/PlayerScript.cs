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

	public float animSpeed = 20f;
	public float moveSpeed = 0.5f;//lower the better
	float CheckSpeed = 0.3f;

	[HideInInspector]
	public CardScript[] rotatedCards = new CardScript[2];
	GameObject[] playerEffectMem = new GameObject[2];

	public bool canMove = true;

	public GameObject playerEffect;

	// Use this for initialization
	void Start () {
		AirConsole.instance.onMessage += OnMessage;
	}
	
	// Update is called once per frame
	void Update () {

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



		}else{
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

		playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY -1), 0);
		#endif
	
	}

	void OnMessage (int device_id, JToken data) {
		//print (data);
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if (active_player != -1) {
			if (active_player == id) {
				//print ("got something4");
				//print (data);
				if (data ["dpadrelative-left "] != null) {
					
					if(!canMove)
						return;
					Move (data);

				} else if (data ["Select"] != null) {
					if ((bool)data ["Select"] ["pressed"]) {

						PressSelect ();
					}
				} else if (data ["swipepattern-right"] != null) {
					powerUp.UsePowerUp (data);
				}
			}
		}
	}

	void Move(JToken data){
		//start moving
		if ((bool)data ["dpadrelative-left"] ["pressed"]) {
			//print ("got something5");

			switch ((string)data ["dpadrelative-left"] ["message"] ["direction"]) {
			case "right":
				print ("Right");
				StartCoroutine (MoveStep (new Vector3 (1, 0, 0), moveSpeed));
				break;
			case "left":
				print ("Left");
				StartCoroutine (MoveStep (new Vector3 (-1, 0, 0), moveSpeed));
				break;
			case "up":
				print ("Up");
				StartCoroutine (MoveStep (new Vector3 (0, 1, 0), moveSpeed));
				break;
			case "down":
				print ("Down");
				StartCoroutine (MoveStep (new Vector3 (0, -1, 0), moveSpeed));
				break;
			default:
				print ("error");
				break;
			}

			// stop moving
		} else {
			print ("stopped moving");
			StopCoroutine ("MoveStep");
		}

	}

	IEnumerator MoveStep(Vector3 moveAm, float repeatRate) {
		print ("started to move");
		while(true) {
			playerPos = AddVectors (playerPos, moveAm);
			playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY - 1), 0);

			yield return new WaitForSeconds(repeatRate);
		}
	}

	void PressSelect(){

		CardScript myCardS = cardGen.allCards [(int)playerPos.x, (int)playerPos.y].GetComponent<CardScript> ();

		if (myCardS.cardType == 0)
			return;
		if (myCardS.isSelected)
			return;

		myCardS.RotateSelf ();
		myCardS.isSelected = true;

		if (powerUp.isShadowActive) {
			powerUp.ShadowSelect(myCardS);
			return;
		}

		GameObject temp = (GameObject)Instantiate (playerEffect, myCardS.transform.position, myCardS.transform.rotation);
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
			if (powerUp.isEarthActive) {
				powerUp.Invoke("EarthPreCheck", CheckSpeed);
			}
		} else {
			rotatedCards [1] = myCardS;
			Invoke ("CheckCards", CheckSpeed);
		}
	}

	void CheckCards(){

		foreach (GameObject gam in playerEffectMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}

		if (powerUp.isPoisonActive) {
			powerUp.PoisonCheck(rotatedCards);
			return;
		}

		if (powerUp.isEarthActive) {
			powerUp.EarthCheck();
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


	//old move script
	/*void Move(JToken data){
		
		if ((bool)data ["dpad-left"] ["pressed"]) {
			//print ("got something5");

			switch ((string)data ["dpad-left"] ["message"] ["direction"]) {
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
}