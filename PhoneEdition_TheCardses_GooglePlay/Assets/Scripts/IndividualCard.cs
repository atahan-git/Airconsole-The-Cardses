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

	public int x = -1;
	public int y = -1;

	int _cardType = 0;
	[SerializeField]
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

	public Sprite[] cardSprites;
	public SpriteRenderer myRenderer;

	[Space]

	public GameObject getEffectPrefab;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		isSelectable = true;
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
	public GameObject selectedEffect;

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
		Invoke ("ReOpenCard", CardHandler.s.cardReOpenTime);
	}

	public void ReOpenCard () {
		anim.SetBool ("isOpen", false);
		Invoke ("RandomizeCardType", 0.5f);
		Invoke ("ReEnableSelection", 0.5f);
	}

	public void NetherMatch (){
		isSelectable = false;
		anim.SetTrigger ("JustRotate");
		Invoke ("RandomizeCardType", 0.5f);
		Invoke ("ReEnableSelection", 0.5f);
	}

	//-----------------------------------------Utility Functions

	void DestroySelectedEfect (){
		if (selectedEffect != null)
			Destroy (selectedEffect);
		selectedEffect = null;
	}

	void UpdateGraphics (){
		myRenderer.sprite = cardSprites [cardType];
	}

	public void UpdateCardType (int type) {
		cardType = type;
		CancelInvoke("RandomizeCardType");

		if (DataHandler.s.myPlayerIdentifier == 'B') {
			GoogleAPI.s.logText.LogMessage ("Sending card type to the other party");
			DataHandler.s.SendCardType (x, y, cardType);
		}
	}

	void ReEnableSelection (){
		isSelectable = true;
	}

	public void RandomizeCardType () {
		int dragonChance = CardHandler.s.dragonChance;
		int type;

		if (Random.Range (0, dragonChance) == 0) {
			//we got a dragon!
			type = Random.Range(8,15);
		} else {
			//just a normal card
			type = Random.Range(1,8);
		}

		UpdateCardType (type);
	}

	void SpawnGetEffect () {
		Instantiate (getEffectPrefab, transform.position, Quaternion.identity);
	}
}
