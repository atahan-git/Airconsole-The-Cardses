using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour {

	public Text textGridSizeX;
	public Slider sliderGridSizeX;
	public Text textGridSizeY;
	public Slider sliderGridSizeY;

	public Toggle timeAt;
	public Toggle score;

	public bool isTimeAttack = true;
	bool check = false;

	public Text textModeSet;
	public Slider sliderModeSet;

	// Use this for initialization
	void Start () {
		UpdateModeSetSlider ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnSliderChange(){
		UpdateValues ();
	}

	public void OnToggleChange(){
		//print ("called");
		if (isTimeAttack) {
			timeAt.isOn = false;
			score.isOn = true;
		} else {
			timeAt.isOn = true;
			score.isOn = false;
		}

		if (check) {
			isTimeAttack = !isTimeAttack;
			UpdateModeSetSlider ();
			check = false;
		} else {
			check = true;
		}
	}

	void UpdateModeSetSlider(){
		if (!isTimeAttack) {
			sliderModeSet.maxValue = 100;
			sliderModeSet.minValue = 5;
			sliderModeSet.value = 20;
		} else {
			sliderModeSet.maxValue = 300;
			sliderModeSet.minValue = 10;
			sliderModeSet.value = 120;
		}

		UpdateValues ();
	}


	void UpdateValues(){

		textGridSizeX.text = sliderGridSizeX.value.ToString();
		textGridSizeY.text = sliderGridSizeY.value.ToString();

		if (!isTimeAttack) {
			textModeSet.text = sliderModeSet.value.ToString ();
		} else {

			int minuteCount = (int)(sliderModeSet.value / 60);
			int secondCount = (int)(sliderModeSet.value - (minuteCount * 60));
			if (secondCount < 10) {
				textModeSet.text = minuteCount.ToString () + ":0" + secondCount;
			} else {
				textModeSet.text = minuteCount.ToString () + ":" + secondCount;
			}
		}

	}
}
