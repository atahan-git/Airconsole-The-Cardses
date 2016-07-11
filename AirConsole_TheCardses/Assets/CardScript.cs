using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour {

	//--------CARD TYPES---------
	// 0 = empty a.k.a. already gotten
	// 1-7 = normal cards
	// 8-14 = dragons
	//---------------------------

	private int _cardType = 0;
	[SerializeField]
	public int cardType{
		get{
			return _cardType;
		}
		set{
			_cardType = value;
			SetColor ();

			if (_cardType == 0)
				CoolRotate ();
		}
	}

	//public Color[] colors;
	public Sprite[] sprites;

	//public Renderer rend;
	public SpriteRenderer sRend;

	public bool state = false;

	Quaternion defPos = Quaternion.Euler(0, 180, 0);
	Quaternion openPos = Quaternion.Euler(0, 0, 0);
	//Quaternion CoolPos = Quaternion.Euler(0, 359, 0);
	Quaternion goToRot = Quaternion.Euler(0, 180, 0);

	public float rotSpeed = 20f;
	public float ReSelectTime = 5f;
	public int dragonChance = 7;

	public bool isSelected = false;

	public GameObject getEffect;
	// Use this for initialization
	void Start () {

		transform.rotation = Quaternion.Euler(0, 180, 0);
		SelectRandomCardType ();

	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Slerp (transform.rotation, goToRot, rotSpeed * Time.deltaTime);

		if (Input.GetKeyDown (KeyCode.R)) {
			SelectRandomCardType ();
		}
	}

	public void SetColor(){
		//rend.material.color = colors [cardType];
		sRend.sprite = sprites[cardType];
		if (cardType == 0) {
			//print ("invoke pls");
			Invoke ("RotateSelf", ReSelectTime - 0.5f);
			Invoke ("SelectRandomCardType", ReSelectTime);
		}
	}

	public void SelectRandomCardType(){
		//try to be a dragon
		if (Random.Range (0, dragonChance) == 0) {
			//success!   / dragon

			//get our dragon type
			cardType = Random.Range(8,14 + 1);

		} else {
			//fail :(    / normal card

			//get our regular card type
			cardType = Random.Range(1,7 + 1);

		}

		//print ("card selected = " + cardType);
	}

	public void RotateSelf(){
		//print ("rotated Self");
		if (state) {
			state = false;

			goToRot = defPos;

		} else {
			state = true;

			goToRot = openPos;
		}
		//print ("rotation Done");
	}

	public void CoolRotate(){

		StartCoroutine ("changeIt");
		Invoke ("EndCoolRotate", 0.5f);

		Instantiate (getEffect, transform.position, transform.rotation);
	}
	IEnumerator changeIt(){
		while (true) {
			goToRot.eulerAngles = goToRot.eulerAngles + new Vector3 (0, 10, 0);
			yield return 0;
		}
	}

	public void EndCoolRotate(){

		StopCoroutine ("changeIt");
		//transform.rotation = openPos;
		goToRot = openPos;
	}

	public CardScript(){

	}
}
