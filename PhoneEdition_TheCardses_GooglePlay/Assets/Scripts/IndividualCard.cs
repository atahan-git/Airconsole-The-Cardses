using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualCard : MonoBehaviour {

	//--------CARD TYPES---------
	// 0 = empty / already gotten
	// 1-7 = normal cards
	// 8-14 = dragons
	//---------------------------
	//1 = Earth
	//2 = Fire
	//3 = Ice
	//4 = Light
	//5 = Nether
	//6 = Poison
	//7 = Shadow
	//---------------------------
	// 8 = Earth Dragon
	// 9 = Fire Dragon
	//10 = Ice Dragon
	//11 = Light Dragon
	//12 = Nether Dragon
	//13 = Poison Dragon
	//14 = Shadow Dragon
	//---------------------------


	[HideInInspector]
	public int x = -1;
	[HideInInspector]
	public int y = -1;

	bool _isPoison = false;
	public bool isPoison{
		get{
			return _isPoison;
		}
		set{
			_isPoison = value;
			if (_isPoison) {
				cardType = 15;
			} 
			UpdateGraphics ();
		}
	}

	int _cardType = 0;
	public int cardType{
		get{
			return _cardType;
		}
		set{
			_cardType = value;
			UpdateGraphics ();
		}
	}
	[HideInInspector]
	public bool isSelectable = true;
	Animator anim;

	[Space]


	//public GameObject[] cardPrefabs = new GameObject[15];
	public SpriteRenderer myRenderer;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		isSelectable = true;
		//myRenderer.enabled = false;
	}

	void Update (){
		if (Input.GetKeyDown (KeyCode.R)) {
			RandomizeCardType ();
		}
	}

	//-----------------------------------------Card Matching Functions

	public void SelectCard () {
		anim.SetBool ("isOpen", true);
		isSelectable = false;
	}


	[HideInInspector]
	public GameObject selectedEffect{
		get{
			return _selectedEffect;
		}set{
			DestroySelectedEfect ();
			_selectedEffect = value;
		}
	}

	GameObject _selectedEffect;

	public void UnSelectCard () {
		anim.SetBool ("isOpen", false);
		isSelectable = true;
		DestroySelectedEfect ();
	}

	public void MatchCard () {
		anim.SetTrigger ("Matched");
		anim.SetBool ("isOpen", true);
		isSelectable = false;
		DestroySelectedEfect ();
		cardType = 0;
		Invoke ("ReOpenCard", GS.a.cardReOpenTime);
	}

	public void ReOpenCard () {
		DestroySelectedEfect ();
		anim.SetBool ("isOpen", false);
		Invoke ("RandomizeCardType", 0.5f);
		Invoke ("ReEnableSelection", 0.5f);
	}

	public void NetherMatch (){
		DestroySelectedEfect ();
		isSelectable = false;
		anim.SetTrigger ("JustRotate");
		Invoke ("RandomizeCardType", 0.5f);
		Invoke ("ReEnableSelection", 0.5f);
	}

	public void PoisonMatch (){
		anim.SetBool ("isOpen", false);
		_isPoison = false;
		DestroySelectedEfect ();
		Invoke ("RandomizeCardType", 0.5f);
		Invoke ("ReEnableSelection", 0.5f);
	}

	//-----------------------------------------Utility Functions

	void DestroySelectedEfect (){
		try{
			if (_selectedEffect != null)
				Destroy (_selectedEffect);
			_selectedEffect = null;
		}catch(System.Exception e){
			GoogleAPI.s.logText.LogMessage (e.StackTrace, true);
		}
	}

	//GameObject activeOne;
	void UpdateGraphics (){
		/*if (activeOne != null) {
			Destroy (activeOne);
			activeOne = null;
		}
		if (cardPrefabs [cardType] != null) {
			activeOne = (GameObject)Instantiate (cardPrefabs [cardType], myRenderer.transform);
			activeOne.transform.ResetTransformation ();
		} else {*/
		myRenderer.sprite = GS.a.cardSprites [cardType];
		//}
	}

	public void UpdateCardType (int type) {
		cardType = type;
		CancelInvoke("RandomizeCardType");

		if (DataHandler.s.myPlayerIdentifier == 'B') {
			DataLogger.s.LogMessage ("Master Card Type Send");
			DataHandler.s.SendCardType (x, y, cardType);
		}
	}

	void ReEnableSelection (){
		isSelectable = true;
	}

	public void RandomizeCardType () {
		int type = 0;
		/*int dragonChance = CardHandler.s.dragonChance;
		int type;

		if (Random.Range (0, dragonChance) == 0) {
			//we got a dragon!
			type = Random.Range(8,15);
		} else {
			//just a normal card
			type = Random.Range(1,8);
		}*/

		type = RandFuncs.Sample (GS.a.cardChances);

		UpdateCardType (type);
	}

	void SpawnGetEffect () {
		Instantiate (GS.a.getEffectPrefab, transform.position, Quaternion.identity);
	}
}
