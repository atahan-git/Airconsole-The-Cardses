using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CardScript : NetworkBehaviour {

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

	[SyncVar]
	public bool isPoison = false;
	[SyncVar]
	public int poisonGiver = -1;

	public Vector3 myPos;

	[SyncVar(hook = "SetType")]
	int _cardType = 0;
	[SerializeField]
	public int cardType{
		get{
			return _cardType;
		}
	}

	//public Color[] colors;
	//public Sprite[] sprites;

	//public Renderer rend;
	public SpriteRenderer sRend;



	Quaternion defPos = Quaternion.Euler(0, 180, 0);
	Quaternion openPos = Quaternion.Euler(0, 0, 0);
	//Quaternion CoolPos = Quaternion.Euler(0, 359, 0);
	Quaternion goToRot = Quaternion.Euler(0, 180, 0);

	public float rotSpeed = 20f;
	public float ReSelectTime = 15f;
	[HideInInspector]
	public float _ReSelectTime = 15f;
	public int dragonChance = 5;

	[SyncVar]
	public bool isSelected = false;
	[SyncVar(hook = "SetState")]
	bool state = false;

	bool resetInProgress = false;

	public GameObject getEffect;

	GameObject myPoisonEffect;

	Animator anim;
	// Use this for initialization
	void Start () {
		//myPoisonEffect = PowerUpStuff.s.TrapCardEffect;
		//transform.rotation = Quaternion.Euler(0, 180, 0);
		_ReSelectTime = ReSelectTime;
		anim = GetComponent<Animator> ();

		if(isServer)
			CmdSelectRandomCardType ();
		
	}
	
	// Update is called once per frame
	void Update () {
		//transform.rotation = Quaternion.Slerp (transform.rotation, goToRot, rotSpeed * Time.deltaTime);

		/*if (Input.GetKeyDown (KeyCode.R)) {
			SelectRandomCardType ();
		}*/
	}

	public void SetDirtyBitses () {
		SetDirtyBit (int.MaxValue);
	}

	public void SetColor(){
		//rend.material.color = colors [cardType];
		sRend.sprite = AllCardSprites.s.fronts[_cardType];
		if (isPoison) {
			if (localPlayer == null)
				localPlayer = GameObject.Find ("Local Player").GetComponent<PlayerScript>().id;
			sRend.sprite = AllCardSprites.s.trapcard;
		}
	}

	void CommandWait () {
		resetInProgress = false;
		if(isServer)
			CmdSelectRandomCardType ();
	}

	[Command]
	public void CmdSelectRandomCardType(){
		CancelInvoke ("CommandWait");
		//try to be a dragon
		if (Random.Range (0, dragonChance) == 0) {
			//get our dragon type
			SetType(Random.Range(8,14 + 1));
		} else {
			//get our regular card type
			SetType(Random.Range(1,7 + 1));
		}

		_ReSelectTime = ReSelectTime;

		isPoison = false;
		isSelected = false;
		//SetDirtyBitses ();
		//print ("card selected = " + cardType);
	}

	public void SelfClose(){
		isSelected = false;
		SetState (false);
		Invoke ("CommandWait", 0.3f);
	}

	public CardScript(){

	}
	bool isMatched = false;
	//LOCAL
	void CardMatched () {
		print (isPoison + "Card Mathced");
		isMatched = true;
		if (!isPoison) {
			if(isServer)
			anim.SetBool ("isOpen", true);
			if(isServer)
			anim.SetTrigger ("isMatched");
			
			if (myPoisonEffect != null) {
				myPoisonEffect.GetComponent<DisableAndDestroy> ().Engage ("CardScript");
				myPoisonEffect = null;
			}

		} else {
			if (myPoisonEffect != null) {
				myPoisonEffect.GetComponent<DisableAndDestroy> ().Engage ("CardScript");
				myPoisonEffect = null;
			}
			SetState (false);
			Invoke ("WaitAndTurn", 0.1f);
		}
	}

	void WaitAndTurn () {
		isPoison = false;
		isMatched = false;
		SetColor ();
		SetState (true);
	}

	//LOCAL
	public void SetType (int value) {
		_cardType = value;
		SetColor ();

		if (_cardType == 0 && !resetInProgress) {
			resetInProgress = true;
			CardMatched ();
			state = true;
			Invoke ("TurnBack", _ReSelectTime - 0.3f);
			Invoke ("CommandWait", _ReSelectTime);
		}
	}

	void TurnBack () {
		SetState (false);
	}

	int localPlayer;

	//LOCAL
	public void SetState (bool value) {
		state = value;
		if(isServer)
		anim.SetBool ("isOpen", value);
		if (state) {
			if (isPoison && !isMatched) {
				if (myPoisonEffect == null) {
					myPoisonEffect = (GameObject)Instantiate (PowerUpStuff.s.TrapCardEffect, transform.position,Quaternion.identity);
					//myPoisonEffect.transform.localPosition = Vector3.zero;
				}
				SetColor ();
			}
		} else {
			if (myPoisonEffect != null) {
				//if (!(isPoison && localPlayer == poisonGiver)) {
					myPoisonEffect.GetComponent<DisableAndDestroy> ().Engage ("CardScript");
					myPoisonEffect = null;
				//}
			}
		}
	}

	public void SpawnGetEffect () {
		Instantiate (getEffect, transform.position, transform.rotation);
	}

	public void JustRotate () {
		if (myPoisonEffect != null) {
			myPoisonEffect.GetComponent<DisableAndDestroy> ().Engage ("CardScript");
			myPoisonEffect = null;
		}
		if(isServer)
			anim.SetTrigger ("JustRotate");
	}
}
