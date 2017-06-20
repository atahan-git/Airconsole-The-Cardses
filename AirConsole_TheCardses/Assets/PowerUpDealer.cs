using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Linq;

public class PowerUpDealer : MonoBehaviour {

	public PlayerScript myPlayer;
	public PowerUpDealer[] otherPlayers;

	/*public enum PowerUpTypes
	{
		noUp,
		light,
		shadow,
		fire,
		earth,
		poison,
		nether,
		ice
	}*/

	public int[] DataTester = new int[5];

	NestedIntArray[] lightCodes  = new NestedIntArray[2];
	NestedIntArray[] shadowCodes = new NestedIntArray[2];
	NestedIntArray[] fireCodes   = new NestedIntArray[2];
	NestedIntArray[] earthCodes  = new NestedIntArray[2];
	NestedIntArray[] poisonCodes = new NestedIntArray[2];
	NestedIntArray[] netherCodes = new NestedIntArray[2];
	NestedIntArray[] iceCodes    = new NestedIntArray[2];

	//--------------------------------------------------------------------------

	public bool isLightActive = false;
	public bool isShadowActive = false;
	public bool isEarthActive = false;

	public int poisonId = -1;
	bool _isPoisonActive = false;
	public bool isPoisonActive{
		set{
			if (value) {
				EnablePoison ();
			} 
			_isPoisonActive = value;
		}
		get{
			return _isPoisonActive;
		}
	}

	bool _isIceActive = false;
	public bool isIceActive{
		set{
			if (value) {
				EnableIce ();
			} 
			_isIceActive = value;
		}
		get{
			return _isIceActive;
		}
	}

	//--------------------------------------------------------------------------
	private GameObject myLightEffect;
	private GameObject myShadowEffect;
	private GameObject myPosionEffect;
	private GameObject myIceEffect;
	private GameObject myEarthEffect;

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
		if(myPlayer.id == 0){
			if (Input.GetKeyDown (KeyCode.Alpha7)) {
			LightPowerUp ();
		}
			if (Input.GetKeyDown (KeyCode.Alpha6)) {
			ShadowPowerUp ();
		}
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
			FirePowerUp ();
		}
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
			EarthPowerUp ();
		}
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
			PoisonPowerUp ();
		}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
			NetherPowerUp ();
		}
			if (Input.GetKeyDown (KeyCode.Alpha5)) {
			IcePowerUp ();
		}
		}
		#endif
	}



	//------------------------------------------------------------------------------------------------------------------ main function
	public void UsePowerUp(JToken data){

		/*if (myPlayer.curPowerUp != PowerUpTypes.noUp)
			return;*/

		int[] mydata = ParseData (data);

		ActivatePowerUp (mydata);
		//print (mydata);
	}



	//------------------------------------------------------------------------------------------------------------------ data parser
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



	//------------------------------------------------------------------------------------------------------------------ type finder
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



	//------------------------------------------------------------------------------------------------------------------ power up functions
	//---------------------------------------------------------------------------------------------Done LightPowerUp
	void LightPowerUp(){ // 11
		print ("LightPowerUp");
		//do we have dragon?
		if (ScoreKeeper.s.players [myPlayer.id].Scores [11] > 0) {
			//yes
			ScoreKeeper.s.AddScore(myPlayer.id, 11, -1);
			if (isLightActive) {
				CancelInvoke ("undoLightPowerUp");
				Invoke ("undoLightPowerUp", PowerUpStuff.s.shadowTime);
				return;
			}
			isLightActive = true;
			myLightEffect = (GameObject)Instantiate (PowerUpStuff.s.LightEffect, transform.position, transform.rotation);
			myLightEffect.transform.parent = transform;
			myLightEffect.transform.localScale = transform.localScale;
			Invoke ("undoLightPowerUp", PowerUpStuff.s.lightTime);
			DisableIce ();
			DisablePoison ();
		}
	}

	void undoLightPowerUp(){
		isLightActive = false;
		if (myLightEffect != null)
			Destroy (myLightEffect.gameObject);
		myLightEffect = null;
	}


	//---------------------------------------------------------------------------------------------Done ShadowPowerUp
	CardScript[] ShadowMem = new CardScript[99];
	GameObject[] ShadowEffMem = new GameObject[99];
	int n = 0;

	void ShadowPowerUp(){ // 14
		print ("ShadowPowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [14] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 14, -1);
			if (isShadowActive) {
				CancelInvoke ("undoShadowPowerUp");
				Invoke ("undoShadowPowerUp", PowerUpStuff.s.shadowTime);
				return;
			}
			n = 0;
			isShadowActive = true;
			myShadowEffect = (GameObject)Instantiate (PowerUpStuff.s.ShadowEffect, transform.position, transform.rotation);
			myShadowEffect.transform.parent = transform;
			myShadowEffect.transform.localScale = transform.localScale;
			Invoke ("undoShadowPowerUp", PowerUpStuff.s.shadowTime);
		}
	}

	void undoShadowPowerUp(){
		print ("shadow end");
		foreach (GameObject gam in ShadowEffMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}

		StartCoroutine (CheckCardsCOROT (ShadowMem, 7));

		isShadowActive = false;
		if (myShadowEffect != null)
			Destroy (myShadowEffect.gameObject);
		myShadowEffect = null;
	}


	public void ShadowSelect(CardScript myCardS){

		GameObject temp = (GameObject)Instantiate (PowerUpStuff.s.ShadowSelectEffect, myCardS.transform.position, myCardS.transform.rotation);
		ShadowEffMem [n] = temp;

		ShadowMem [n] = myCardS;

		n++;
	}


	//---------------------------------------------------------------------------------------------Done FirePowerUp
	void FirePowerUp(){ // 9
		print ("FirePowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [9] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 9, -1);
			StartCoroutine(FireActivate());
			myPlayer.canSelect = false;
		}
	}

	IEnumerator FireActivate(){

		//gfx
		Instantiate(PowerUpStuff.s.FireEffect, transform.position, transform.rotation);
		//

		Vector3 playerPos = myPlayer.playerPos;

		//get cards
		int leftLimit  = (int)Mathf.Clamp (playerPos.x - 1, 0, myPlayer.cardGen.gridSizeX - 1);
		int rightLimit = (int)Mathf.Clamp (playerPos.x + 1, 0, myPlayer.cardGen.gridSizeX - 1);
		int downLimit  = (int)Mathf.Clamp (playerPos.y - 1, 0, myPlayer.cardGen.gridSizeY - 1);
		int upLimit    = (int)Mathf.Clamp (playerPos.y + 1, 0, myPlayer.cardGen.gridSizeY - 1);

		CardScript[] cardsToCheck = new CardScript[11];
		/*for (int i = 0; i < 11; i++) {
			cardsToCheck [i] = new CardScript ();
		}*/

		if (myPlayer.rotatedCards [0] != null) {
			cardsToCheck [10] = myPlayer.rotatedCards [0];
			myPlayer.rotatedCards[0] = null;
			foreach (GameObject gam in myPlayer.playerEffectMem) {
				if (gam != null)
					Destroy (gam.gameObject);
			}
		}
		

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
		StartCoroutine (CheckCardsCOROT (cardsToCheck, 2));
	}


	//---------------------------------------------------------------------------------------------Done EarthPowerUp
	CardScript[] earthMem = new CardScript[4];
	GameObject[] earthEfMem = new GameObject[2];

	void EarthPowerUp(){ // 8
		print ("EarthPowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [8] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 8, -1);
			if (isEarthActive) {
				CancelInvoke ("undoEarthPowerUp");
				Invoke ("undoEarthPowerUp", PowerUpStuff.s.shadowTime);
				return;
			}
			isEarthActive = true;
			myEarthEffect = (GameObject)Instantiate (PowerUpStuff.s.EarthEffect, transform.position, transform.rotation);
			myEarthEffect.transform.parent = transform;
			myEarthEffect.transform.localScale = transform.localScale;
			Invoke ("undoEarthPowerUp", PowerUpStuff.s.earthTime);
		}
	}

	void undoEarthPowerUp(){
		isEarthActive = false;
		if (myEarthEffect != null)
			Destroy (myEarthEffect.gameObject);
		myEarthEffect = null;

		EarthCheck ();
		//CheckCardsCOROTquick (earthMem , 1);
		earthMem = new CardScript[4];

		foreach (GameObject gam in earthEfMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}
	}

	public void EarthPlace(){
		CardScript myCardS = new CardScript();

		while (!(myCardS != null && myCardS.cardType != 0 && myCardS.isSelected != true)) {

			Vector3 randPos = new Vector3 (Random.Range (0, myPlayer.cardGen.gridSizeX), Random.Range (0, myPlayer.cardGen.gridScaleY), 0);

			myCardS = myPlayer.cardGen.allCards [(int)randPos.x, (int)randPos.y].GetComponent<CardScript> ();

		}

		myCardS.RotateSelf ();
		myCardS.isSelected = true;

		GameObject temp = (GameObject)Instantiate (PowerUpStuff.s.EarthSelectEffect, myCardS.transform.position, myCardS.transform.rotation);
		if (earthEfMem [0] == null) {
			earthEfMem [0] = temp;
		} else {
			earthEfMem [1] = temp;
		}

		if (earthMem [0] == null) {
			earthMem [0] = myCardS;
		} else {
			earthMem [1] = myCardS;
		}
	}

	public void EarthPreCheck(){

		if (myPlayer.rotatedCards [0].cardType == earthMem [0].cardType) {
			int myCardType = myPlayer.rotatedCards [0].cardType;

			myPlayer.rotatedCards [0].cardType = 0;	
			myPlayer.rotatedCards [0].isSelected = false;
			myPlayer.rotatedCards [0] = null;
			earthMem [0].cardType = 0;
			earthMem [0].isSelected = false;
			earthMem [0] = null;

			foreach (GameObject gam in earthEfMem) {
				if (gam != null)
					Destroy (gam.gameObject);
			}
			foreach (GameObject gam in myPlayer.playerEffectMem) {
				if (gam != null)
					Destroy (gam.gameObject);
			}

			ScoreKeeper.s.AddScore (myPlayer.id, myCardType, 1);

		}


	}

	public void EarthCheck(){

		earthMem [2] = myPlayer.rotatedCards [0];
		earthMem [3] = myPlayer.rotatedCards [1];

		foreach (GameObject gam in earthEfMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}
		foreach (GameObject gam in myPlayer.playerEffectMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}

		CheckCardsCOROTquick (earthMem, 1);

		myPlayer.rotatedCards [0] = null;
		myPlayer.rotatedCards [1] = null;

	}

	//---------------------------------------------------------------------------------------------Done PoisonPowerUp
	void PoisonPowerUp(){ // 13
		print ("PoisonPowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [13] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 13, -1);
			foreach(PowerUpDealer others in otherPlayers){
				if (others != null) {
					if (!others.isLightActive) {
						others.DisablePoison ();
						others.isPoisonActive = true;
						others.poisonId = myPlayer.id;
					}
				}
			}
		}
	}

	void EnablePoison(){
		
		if (myPosionEffect != null)
			Destroy (myPosionEffect.gameObject);
		myPosionEffect = null;

		CancelInvoke ("DisablePoison");

		myPosionEffect = (GameObject)Instantiate (PowerUpStuff.s.PoisonEffect, transform.position, transform.rotation);
		myPosionEffect.transform.parent = transform;
		myPosionEffect.transform.localScale = transform.localScale;

		Invoke ("DisablePoison", PowerUpStuff.s.poisonTime);

	}

	public void DisablePoison(){
		isPoisonActive = false;

		if (myPosionEffect != null)
			Destroy (myPosionEffect.gameObject);
		myPosionEffect = null;
	}

	public void PoisonCheck(CardScript[] rotatedCards){

		//got them correct
		if (rotatedCards [0].cardType == rotatedCards [1].cardType) {
			int myCardType = rotatedCards [0].cardType;

			rotatedCards [0].cardType = 0;	
			rotatedCards [0].isSelected = false;
			rotatedCards [0] = null;
			rotatedCards [1].cardType = 0;
			rotatedCards [1].isSelected = false;
			rotatedCards [1] = null;

			ScoreKeeper.s.AddScore (poisonId, 6 /*myCardType*/, 1);
			ScoreKeeper.s.AddScore (myPlayer.id, myCardType, 1);

		} else {
			rotatedCards [0].RotateSelf ();
			rotatedCards [0].isSelected = false;
			rotatedCards [0] = null;
			rotatedCards [1].RotateSelf ();
			rotatedCards [1].isSelected = false;
			rotatedCards [1] = null;
		}
	}

	public void PoisonDragon(int cardType){
		ScoreKeeper.s.AddScore (poisonId, 13 /*cardType*/, 1);
	}

	//---------------------------------------------------------------------------------------------Done NetherPowerUp
	void NetherPowerUp(){ // 12
		print ("NetherPowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [12] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 12, -1);
			StartCoroutine(NetherActivate());
		}
	}
	//rotatedCards [0]._ReSelectTime = rotatedCards [0].ReSelectTime * PowerUpStuff.s.shadowMultiplier;
	int netherCount = 0;
	IEnumerator NetherActivate(){

		for (int y = 0; y < myPlayer.cardGen.gridSizeY; y++) {
			
			CardScript NetherPos = myPlayer.cardGen.allCards [0, y].GetComponent<CardScript> ();
			Instantiate (PowerUpStuff.s.NetherEffect, NetherPos.transform.position, Quaternion.identity);

			for (int x = 0; x < myPlayer.cardGen.gridSizeX; x++) {

				CardScript myCardS = myPlayer.cardGen.allCards [x, y].GetComponent<CardScript> ();

				if (!myCardS.isSelected) {
					
					if (myCardS.cardType == 0) {
						
						netherCount++;
						if (netherCount >= 2) {
							ScoreKeeper.s.AddScore (myPlayer.id, 5, 1);
							netherCount = 0;
						}

						Instantiate (PowerUpStuff.s.NetherGetEffect, myCardS.transform.position - Vector3.forward, myCardS.transform.rotation);
						//myCardS.cardType = 5;
						//myCardS.cardType = 0;
						yield return new WaitForSeconds (0.01f);
						
					} else {
						myCardS.CoolRotate ();
						myCardS.Invoke ("SelfClose", PowerUpStuff.s.NetherReRotateTime);
						myCardS.isSelected = true;
					}
				}
				yield return new WaitForSeconds(0.005f);
			}
			yield return new WaitForSeconds(0.05f);
		}
		netherCount = 0;
	}




	//---------------------------------------------------------------------------------------------Done IcePowerUp
	void IcePowerUp(){ // 10 
		print ("IcePowerUp");
		if (ScoreKeeper.s.players [myPlayer.id].Scores [10] > 0) {
			ScoreKeeper.s.AddScore(myPlayer.id, 10, -1);
			foreach(PowerUpDealer others in otherPlayers){
				if (others != null) {
					if (!others.isLightActive) {
						others.DisableIce ();
						others.isIceActive = true;
					}
				}
			}
		}
	}

	void EnableIce(){
		
		if (myIceEffect != null)
			Destroy (myIceEffect.gameObject);
		myIceEffect = null;

		CancelInvoke ("DisableIce");

		myIceEffect = (GameObject)Instantiate (PowerUpStuff.s.IceEffect, transform.position, transform.rotation);
		myIceEffect.transform.parent = transform;
		myIceEffect.transform.localScale = transform.localScale;

		Invoke ("DisableIce", PowerUpStuff.s.iceTime);

		myPlayer.canMove = false;
	}
		
	public void DisableIce(){
		isIceActive = false;
		myPlayer.canMove = true;

		if (myIceEffect != null)
			Destroy (myIceEffect.gameObject);
		myIceEffect = null;
	}

	//------------------------------------------------------------------------------------------------------------------ helper/multiple used function
	[System.Serializable]
	public class NestedIntArray{
		public int[] ar = new int[9];
	}

	void RotateCard(CardScript myCardScript, CardScript[] myCardsToCheck, ref int n){

		if (myCardScript.cardType != 0) {
			if (!myCardScript.isSelected) {

				myCardScript.RotateSelf ();

				myCardsToCheck [n] = myCardScript;
				myCardScript.isSelected = true;

				n++;
			}
		}

	}

	IEnumerator CheckCardsCOROT(CardScript[] cardsToCheck, int cardType){

		//check Cards
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cardType != 0) {

					/*if(cardsToCheck[l].cardType >= 8){

						ScoreKeeper.s.AddScore(myPlayer.id, cardType + 7, 1);

						if(isPoisonActive)
							PoisonDragon (cardsToCheck[l].cardType);

						cardsToCheck[l].cardType = 0;
						cardsToCheck[l].isSelected = false;
					}*/

					for (int k = 1; k < cardsToCheck.Length; k++) {
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
										if(isPoisonActive)
											ScoreKeeper.s.AddScore (poisonId, myCardType, 1);

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

		//Rotate unused cards
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cardType != 0) {

					cardsToCheck [l].RotateSelf ();
					cardsToCheck [l].isSelected = false;
					cardsToCheck [l] = null;

				}
			}
		}
		myPlayer.canSelect = true;
	}

	void CheckCardsCOROTquick(CardScript[] cardsToCheck, int cardType){

		//check Cards
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cardType != 0) {

					/*if(cardsToCheck[l].cardType >= 8){

						ScoreKeeper.s.AddScore(myPlayer.id, cardType + 7, 1);

						if(isPoisonActive)
							PoisonDragon (cardsToCheck[l].cardType);

						cardsToCheck[l].cardType = 0;
						cardsToCheck[l].isSelected = false;
					}*/

					for (int k = 1; k < cardsToCheck.Length; k++) {
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
										if(isPoisonActive)
											ScoreKeeper.s.AddScore (poisonId, 6 /*myCardType*/, 1);

										//yield return new WaitForSeconds (0.1f);
									}
								}
							}
						}
					}
				}
			}
		}
			

		//Rotate unused cards
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cardType != 0) {

					cardsToCheck [l].RotateSelf ();
					cardsToCheck [l].isSelected = false;
					cardsToCheck [l] = null;

				}
			}
		}

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


	/*public void ShadowCheck(CardScript[] rotatedCards){
		
		//got them correct
		if (rotatedCards [0].cardType == rotatedCards [1].cardType) {
			int myCardType = rotatedCards [0].cardType;

			rotatedCards [0]._ReSelectTime = rotatedCards [0].ReSelectTime * PowerUpStuff.s.shadowMultiplier;
			rotatedCards [0].cardType = 0;	
			rotatedCards [0].isSelected = false;
			rotatedCards [0] = null;
			rotatedCards [1]._ReSelectTime = rotatedCards [1].ReSelectTime * PowerUpStuff.s.shadowMultiplier;
			rotatedCards [1].cardType = 0;
			rotatedCards [1].isSelected = false;
			rotatedCards [1] = null;

			ScoreKeeper.s.AddScore(myPlayer.id, myCardType, 1);

		} else {
			rotatedCards [0].RotateSelf ();
			rotatedCards [0].isSelected = false;
			rotatedCards [0] = null;
			rotatedCards [1].RotateSelf ();
			rotatedCards [1].isSelected = false;
			rotatedCards [1] = null;
		}
	}*/


	/*IEnumerator NetherActivate(){

	//gfx
	Instantiate(PowerUpStuff.s.NetherEffect, transform.position, transform.rotation);
	//

	//get cards

	CardScript[] cardsToCheck = new CardScript[myPlayer.cardGen.gridSizeX + myPlayer.cardGen.gridSizeY + 5];

	if (myPlayer.rotatedCards [0] != null)
		cardsToCheck [myPlayer.cardGen.gridSizeX + myPlayer.cardGen.gridSizeY + 4] = myPlayer.rotatedCards [0];


	int n = 0;

	Vector3 playerPos = myPlayer.playerPos;

	CardScript myCardS = myPlayer.cardGen.allCards [(int)playerPos.x, (int)playerPos.y].GetComponent<CardScript> ();

	RotateCard (myCardS, cardsToCheck, ref n);

	for (int i = 1; i <= myPlayer.cardGen.gridSizeX + 1; i++) {

		try {
			myCardS = myPlayer.cardGen.allCards [(int)playerPos.x + i, (int)playerPos.y].GetComponent<CardScript> ();
		} catch {}

		RotateCard (myCardS, cardsToCheck, ref n);

		try {
			myCardS = myPlayer.cardGen.allCards [(int)playerPos.x - i, (int)playerPos.y].GetComponent<CardScript> ();
		} catch {}
		RotateCard (myCardS, cardsToCheck, ref n);

		try {
			myCardS = myPlayer.cardGen.allCards [(int)playerPos.x, (int)playerPos.y + i].GetComponent<CardScript> ();
		} catch {}
		RotateCard (myCardS, cardsToCheck, ref n);

		try {
			myCardS = myPlayer.cardGen.allCards [(int)playerPos.x, (int)playerPos.y - i].GetComponent<CardScript> ();
		} catch {}
		RotateCard (myCardS, cardsToCheck, ref n);

		yield return new WaitForSeconds (0.02f);
	}

	yield return new WaitForSeconds (0.1f);

	//check Cards
	StartCoroutine (CheckCardsCOROT (cardsToCheck));
}*/
}