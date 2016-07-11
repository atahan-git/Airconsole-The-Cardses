using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class PlayerScript : MonoBehaviour {

	public int id = 0;
	public Vector3 playerPos;

	int maxGrid = 3;

	public CardGenerator cardGen;
	public PlayerAssigner master;

	public float speed = 20f;
	float CheckSpeed = 0.3f;

	CardScript[] rotatedCards = new CardScript[2];


	// Use this for initialization
	void Start () {
		AirConsole.instance.onMessage += OnMessage;
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = Vector3.Lerp(transform.position, cardGen.grid[(int)playerPos.x, (int)playerPos.y], speed * Time.deltaTime);
	
	}

	void OnMessage (int device_id, JToken data) {
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if (active_player != -1) {
			if (active_player == id) {
				//print ("got something4");
				print (data);
				if (data ["dpad-left"] != null) {
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
					playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, maxGrid), (int)Mathf.Clamp (playerPos.y, 0, maxGrid), 0);
				} else if (data ["Select"] != null) {
					if ((bool)data ["Select"] ["pressed"]) {

						CardScript myCardS = cardGen.Cards [(int)playerPos.x, (int)playerPos.y].GetComponent<CardScript> ();

						if (myCardS.cardType == 8)
							return;
						if (myCardS.isSelected)
							return;

						myCardS.RotateSelf ();

						if (rotatedCards [0] == null) {
							rotatedCards [0] = myCardS;
							myCardS.isSelected = true;
						} else {
							rotatedCards [1] = myCardS;
							myCardS.isSelected = true;
							Invoke ("CheckCards", CheckSpeed);
						}
					}
				}
			}
		}
	}

	void CheckCards(){
		//got them correct
		if (rotatedCards [0].cardType == rotatedCards [1].cardType) {
			
			rotatedCards [0].cardType = 8;	
			rotatedCards [0].SetColor ();
			rotatedCards [0] = null;
			rotatedCards [1].cardType = 8;
			rotatedCards [1].SetColor ();
			rotatedCards [1] = null;

			if (id == 0) {
				master.scorePlayerBlue ++;
			} else {
				master.scorePlayerRed ++;
			}
			master.UpdateScoreUI ();

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
}
