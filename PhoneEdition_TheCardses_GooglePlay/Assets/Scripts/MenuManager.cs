using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	public static MenuManager s;

	public int curLevel = 0;
	/*
	 * 0 = Menu
	 * 1 = Multiplayer default level
	 * 
	 */



	// Use this for initialization
	void Awake () {
		if (s != null) {
			Destroy (this.gameObject);
			return;
		} else {
			s = this;
		}
	}

	//-----------------------
	MainMenu mm;
	//-----------------------


	void Start (){
		mm = MainMenu.s;
		UpdateMenu ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnRoomWasLoaded () {

	}

	//-----------------------------------------------------------------------Main Menu Controls

	public int MenuState = 0;
	/*
	 * 0 = Opening state
	 * 1 = Multiplayer selected
	 * 2 = SinglePlayer Selected
	 * 
	 */

	public int selectState = 0;

	public int playerCount = 2;

	public void Left () {
		switch (MenuState) {
		case 0:
			switch (selectState) {
			case 0:
				MenuState = 1;
				break;
			case 1:
				selectState = 0;
				MenuState = 2;
				break;
			}
			break;
		case 1:
			GoogleAPI.s.Login ();
			break;
		case 2:
			
			break;
		}
		UpdateMenu ();
	}

	public void Right () {
		switch (MenuState) {
		case 0:

			break;
		case 1:
			GoogleAPI.s.playerCount = playerCount;
			GoogleAPI.s.GetQuickMatch ();
			break;
		case 2:
			SceneMaster.s.LoadSinglePLayerLevel (selectState);
			break;
		}
		UpdateMenu ();
	}

	public void Up (){
		switch (MenuState) {
		case 0:
			selectState++;
			selectState = (int)Mathf.Clamp (selectState, 0, 1);
			break;
		case 1:
			playerCount++;
			playerCount = (int)Mathf.Clamp (playerCount, 2, 4);
			break;
		case 2:
			selectState++;
			selectState = (int)Mathf.Clamp (selectState, 0, 6);
			break;
		}
		UpdateMenu ();
	}

	public void Down (){
		switch (MenuState) {
		case 0:
			selectState--;
			selectState = (int)Mathf.Clamp (selectState, 0, 1);
			break;
		case 1:
			playerCount--;
			playerCount = (int)Mathf.Clamp (playerCount, 2, 4);
			break;
		case 2:
			selectState--;
			selectState = (int)Mathf.Clamp (selectState, 0, 6);
			break;
		}
		UpdateMenu ();
	}

	public void Back () {
		MenuState = 0;
		selectState = 0;
		UpdateMenu ();
	}

	public void UpdateMenu (){
		switch (MenuState) {

		case 0:
			mm.middleText.text = "O";
			mm.toolTip.text = "Select Game Mode";
			mm.rightButton.interactable = false;
			mm.backButton.interactable = false;

			switch (selectState) {
			case 0:
				mm.leftButtonText.text = "Multi Player";
				mm.downButton.interactable = false;
				mm.upButton.interactable = true;

				break;
			case 1:
				mm.leftButtonText.text = "Single Player";
				mm.downButton.interactable = true;
				mm.upButton.interactable = false;
				break;
			}
			break;


		case 1:
			mm.toolTip.text = "Select Player Count";
			mm.middleText.text = playerCount.ToString ();
			mm.leftButtonText.text = "Login";
			mm.backButton.interactable = true;

			switch(playerCount){
			case 2:
				mm.downButton.interactable = false;
				mm.upButton.interactable = true;
				break;
			case 3:
				mm.downButton.interactable = true;
				mm.upButton.interactable = true;
				break;
			case 4:
				mm.downButton.interactable = true;
				mm.upButton.interactable = false;
				break;
			}

			if (GoogleAPI.s.canPlay)
				mm.rightButton.interactable = true;
			else
				mm.rightButton.interactable = false;
			break;


		case 2:
			mm.middleText.text = selectState.ToString();
			mm.toolTip.text = "Select Level";
			mm.rightButton.interactable = true;
			mm.backButton.interactable = true;

			switch (selectState) {
			case 0:
				mm.leftButtonText.text = "Fire Dragon";
				mm.downButton.interactable = false;
				mm.upButton.interactable = true;
				break;
			case 1:
				mm.leftButtonText.text = "Earth Dragon";
				mm.downButton.interactable = true;
				break;
			case 2:
				mm.leftButtonText.text = "Ice Dragon";
				break;
			case 3:
				mm.leftButtonText.text = "Shadow Dragon";
				break;
			case 4:
				mm.leftButtonText.text = "Nether Dragon";
				break;
			case 5:
				mm.leftButtonText.text = "Poison Dragon";
				mm.upButton.interactable = true;
				break;
			case 6:
				mm.leftButtonText.text = "Light Dragon";
				mm.downButton.interactable = true;
				mm.upButton.interactable = false;
				break;
			}
			break;
		}
	}


}
