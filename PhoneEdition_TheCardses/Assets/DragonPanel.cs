using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DragonPanel : MonoBehaviour {

	public int type = -1;
	public int count = -1;

	public Image myImage;
	public Text myText;
	public Image bg;

	public Color[] colors;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetType () {
		myImage.sprite = AllCardSprites.s.fronts [type];
		myText.text = count.ToString();
		bg.color = colors [type - 8];
	}
}
