using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Linq;

public class PowerUpDealer : MonoBehaviour {

	public PlayerScript myPlayer;
	public PowerUpDealer[] otherPlayers;

	public enum PowerUpTypes
	{
		noUp,
		light,
		shadow,
		fire,
		earth,
		poison,
		nether,
		ice
	}

	public int[] DataTester = new int[5];

	NestedIntArray[] lightCodes = new NestedIntArray[2];
	NestedIntArray[] shadowCodes = new NestedIntArray[2];
	NestedIntArray[] fireCodes = new NestedIntArray[2];
	NestedIntArray[] earthCodes = new NestedIntArray[2];
	NestedIntArray[] poisonCodes = new NestedIntArray[2];
	NestedIntArray[] netherCodes = new NestedIntArray[2];
	NestedIntArray[] iceCodes = new NestedIntArray[2];

	public bool isLightActive = false;
	public bool isShadowActive = false;
	public bool isEarthActive = false;
	public bool isPoisonActive = false;
	public bool isIceActive = false;

	private GameObject myLightEffect;
	private GameObject myShadowEffect;
	private GameObject[] myPosionEffects;
	private GameObject[] myIceEffects;

	void Start(){
		lightCodes = PowerUpCodes.s.lightCodes;
		shadowCodes = PowerUpCodes.s.shadowCodes;
		fireCodes = PowerUpCodes.s.fireCodes;
		earthCodes = PowerUpCodes.s.earthCodes;
		poisonCodes = PowerUpCodes.s.poisonCodes;
		netherCodes = PowerUpCodes.s.netherCodes;
		iceCodes = PowerUpCodes.s.iceCodes;


	}

	void Update(){
		#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.L)) {
			LightPowerUp ();
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			ShadowPowerUp ();
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			FirePowerUp ();
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			EarthPowerUp ();
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			PoisonPowerUp ();
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			NetherPowerUp ();
		}
		if (Input.GetKeyDown (KeyCode.I)) {
			IcePowerUp ();
		}
		#endif
	}

	//---------------------------------------------------------------------------- main function
	public void UsePowerUp(JToken data){

		/*if (myPlayer.curPowerUp != PowerUpTypes.noUp)
			return;*/

		int[] mydata = ParseData (data);

		ActivatePowerUp (mydata);
		//print (mydata);
	}
	//---------------------------------------------------------------------------- data parser
	int[] ParseData(JToken data){

		var check = data ["swipepattern-right"] ["message"].Children();

		int[] mydata = new int[9];
		int i = 0;

		foreach (JToken myItem in check) {

			try {
				mydata [i] = (int)myItem ["id"];
			} catch {
				print ("Wrong Shape!");
				break;
			}

			i++;
		}

		DataTester = mydata;

		return mydata;
	}
	//---------------------------------------------------------------------------- type finder
	void ActivatePowerUp(int[] parsedData){

		if (compArrNAr (parsedData, lightCodes)) {
			LightPowerUp ();
		} else if (compArrNAr (parsedData, shadowCodes)) {
			ShadowPowerUp ();
		} else if (compArrNAr (parsedData, fireCodes)) {
			FirePowerUp ();
		} else if (compArrNAr (parsedData, earthCodes)) {
			EarthPowerUp ();
		} else if (compArrNAr (parsedData, poisonCodes)) {
			PoisonPowerUp ();
		} else if (compArrNAr (parsedData, netherCodes)) {
			NetherPowerUp ();
		} else if (compArrNAr (parsedData, iceCodes)) {
			IcePowerUp ();
		} else {
			print ("Wrong Shape!");
		}
			
	}
	//---------------------------------------------------------------------------- power up functions
	void LightPowerUp(){ // 11
		print ("LightPowerUp");
		//do we have dragon?
		if (ScoreKeeper.s.players [myPlayer.id].Scores [11] > 0) {
			undoLightPowerUp ();
			//yes
			ScoreKeeper.s.AddScore(myPlayer.id, 11, -1);
			isLightActive = true;
			myLightEffect = (GameObject)Instantiate (PowerUpStuff.s.LightEffect, transform.position, transform.rotation);
			myLightEffect.transform.parent = transform;
			Invoke ("undoLightPowerUp", PowerUpStuff.s.lightTime);
		}
	}

	void undoLightPowerUp(){
		isLightActive = false;
		if (myLightEffect != null)
			Destroy (myLightEffect.gameObject);
		myLightEffect = null;
	}
	//--------------------------------------------------------------------
	void ShadowPowerUp(){ // 14
		print ("ShadowPowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [14] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 14, -1);
			isShadowActive = true;
			myShadowEffect = (GameObject)Instantiate (PowerUpStuff.s.ShadowEffect, transform.position, transform.rotation);
			myShadowEffect.transform.parent = transform;
			Invoke ("undoLightPowerUp", PowerUpStuff.s.shadowTime);
		}
	}

	void undoShadowPowerUp(){
		isShadowActive = false;
		if (myShadowEffect != null)
			Destroy (myShadowEffect.gameObject);
		myShadowEffect = null;
	}
	//--------------------------------------------------------------------
	void FirePowerUp(){ // 9
		print ("FirePowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [9] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 9, -1);
			StartCoroutine("FireActivate");
		}
	}

	IEnumerator FireActivate(){

		//gfx
		Instantiate(PowerUpStuff.s.FireEffect, transform.position, transform.rotation);
		//

		//get cards
		int leftLimit  = (int)Mathf.Clamp (myPlayer.playerPos.x - 1, 0, myPlayer.cardGen.gridSizeX - 1);
		int rightLimit = (int)Mathf.Clamp (myPlayer.playerPos.x + 1, 0, myPlayer.cardGen.gridSizeX - 1);
		int downLimit  = (int)Mathf.Clamp (myPlayer.playerPos.y - 1, 0, myPlayer.cardGen.gridSizeX - 1);
		int upLimit    = (int)Mathf.Clamp (myPlayer.playerPos.y + 1, 0, myPlayer.cardGen.gridSizeX - 1);

		CardScript[] cardsToCheck = new CardScript[11];
		/*for (int i = 0; i < 11; i++) {
			cardsToCheck [i] = new CardScript ();
		}*/

		if (myPlayer.rotatedCards [0] != null)
			cardsToCheck [10] = myPlayer.rotatedCards [0];

		int n = 0;
		for (int i = leftLimit; i <= rightLimit; i++) {
			for (int m = downLimit; m <= upLimit; m++) {

				CardScript myCardS = myPlayer.cardGen.allCards [i, m].GetComponent<CardScript> ();

				if (myCardS.cardType != 0) {
					if (!myCardS.isSelected) {

						myCardS.RotateSelf ();

						cardsToCheck [n] = myCardS;
						myCardS.isSelected = true;

						n++;
						yield return new WaitForSeconds (0.05f);
					}
				}
			}
		}

		yield return new WaitForSeconds (0.3f);

		//check Cards
		for (int l = 0; l < 11; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cardType != 0) {
				
					for (int k = 1; k < 11; k++) {
						if (cardsToCheck [k] != null && cardsToCheck [l] != null) {
							if (cardsToCheck [k].cardType != 0) {
								if (k != l) {
							
									if (cardsToCheck [k].cardType == cardsToCheck [l].cardType) {
								
										int myCardType = cardsToCheck [k].cardType;

										cardsToCheck [k].cardType = 0;	
										//rotatedCards [0].SetColor ();
										cardsToCheck [k].isSelected = false;
										cardsToCheck [k] = null;
										cardsToCheck [l].cardType = 0;
										//rotatedCards [1].SetColor ();
										cardsToCheck [l].isSelected = false;
										cardsToCheck [l] = null;

										ScoreKeeper.s.AddScore (myPlayer.id, myCardType, 1);

										yield return new WaitForSeconds (0.1f);
									}
								}
							}
						}
					}
				}
			}
		}

		yield return new WaitForSeconds (0.5f);

		//rotate any unused card

		for (int l = 0; l < 11; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cardType != 0) {
				
					cardsToCheck [l].RotateSelf ();
					cardsToCheck [l].isSelected = false;
					cardsToCheck [l] = null;

				}
			}
		}

	}
	//--------------------------------------------------------------------
	void EarthPowerUp(){ // 8
		print ("EarthPowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [11] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 11, -1);

		}
	}
	//--------------------------------------------------------------------
	void PoisonPowerUp(){ // 13
		print ("PoisonPowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [11] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 11, -1);

		}
	}

	void undoPoisonPowerUp(){
		isLightActive = false;
		Destroy (myLightEffect.gameObject);
		myLightEffect = null;
	}
	//--------------------------------------------------------------------
	void NetherPowerUp(){ // 12
		print ("NetherPowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [11] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 11, -1);

		}
	}
	//--------------------------------------------------------------------
	void IcePowerUp(){ // 10
		print ("IcePowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [11] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 11, -1);
			foreach(PowerUpDealer others in otherPlayers){
				others.isIceActive = true;
			}
		}
	}

	void undoIcePowerUp(){
		isLightActive = false;
		Destroy (myLightEffect.gameObject);
		myLightEffect = null;
	}
	//----------------------------------------------------------------------------
	[System.Serializable]
	public class NestedIntArray{
		public int[] ar = new int[9];
	}

	private bool compArrNAr (int[] arr1, NestedIntArray[] arr2){

		bool curState = true;

		for (int i = 0; i < arr1.Length; i++) {
			if (!arr1 [i].Equals (arr2 [0].ar [i]))
				curState = false;
		}
		if (curState)
			return true;

		for (int m = 0; m < arr1.Length; m++) {
			if (!arr1 [m].Equals (arr2 [1].ar[m]))
				return false;
		}

		return true;
	}
}