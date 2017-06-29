using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public static MainMenu s;
	
	public Button leftButton;
	public Text leftButtonText;

	public Button rightButton;
	public Text rightButtonText;

	public Image middleImage;
	public Text middleText;

	public Button backButton;
	public Button upButton;
	public Button downButton;

	public Text toolTip;

	void Awake (){
		s = this;
	}

	public void Left () {
		MenuManager.s.Left ();
	}

	public void Right () {
		MenuManager.s.Right ();
	}

	public void Up () {
		MenuManager.s.Up ();
	}

	public void Down () {
		MenuManager.s.Down ();
	}

	public void Back () {
		MenuManager.s.Back ();
	}
}
