using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour {

	public OptionsMenuHandler menu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame(){
		if (AirConsole.instance.GetActivePlayerDeviceIds.Count >= 2) {

			/*DataHandler.gridSizeX = (int)menu.sliderGridSizeX.value;
			DataHandler.gridSizeY = (int)menu.sliderGridSizeY.value;*/

			DataHandler.playerCount = AirConsole.instance.GetActivePlayerDeviceIds.Count;

			DataHandler.isTimeAttack = menu.isTimeAttack;
			DataHandler.gameSetting = (int)menu.sliderModeSet.value;

			SceneManager.LoadScene (1);
		}
	}
}
