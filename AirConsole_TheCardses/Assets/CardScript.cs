using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour {

	public int cardType = 0;

	public Color[] colors;

	public Sprite[] sprites;

	public Renderer rend;
	public SpriteRenderer sRend;

	public bool state = false;

	Quaternion defPos = Quaternion.Euler(0, 180, 0);
	Quaternion openPos = Quaternion.Euler(0, 0, 0);

	Quaternion goToRot = Quaternion.Euler(0, 180, 0);

	public float rotSpeed = 20f;

	public bool isSelected = false;
	// Use this for initialization
	void Start () {

		transform.rotation = Quaternion.Euler(0, 180, 0);

		//cardType = Random.Range (0, 8);

		//rend = GetComponentInChildren<Renderer> ();

		//rend.material.color = colors [cardType];
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Slerp (transform.rotation, goToRot, rotSpeed * Time.deltaTime);
	}

	public void SetColor(){
		//rend.material.color = colors [cardType];
		sRend.sprite = sprites[cardType];
	}

	public void RotateSelf(){
		if (state) {
			state = false;

			goToRot = defPos;

		} else {
			state = true;

			goToRot = openPos;

		}
	}
}
