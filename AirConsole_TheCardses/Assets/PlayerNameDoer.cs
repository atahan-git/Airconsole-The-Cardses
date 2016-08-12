using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class PlayerNameDoer : MonoBehaviour {

	public int id;
	public Text text;

	void Awake () {
		text = GetComponentInChildren<Text> ();
		AirConsole.instance.onConnect += UpdateName;
		AirConsole.instance.onDisconnect += UpdateName;
		InvokeRepeating ("CallIt", 0.5f, 0.5f);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CallIt(){
		UpdateName (-1);
	}

	void UpdateName(int rand){
		//if (AirConsole.instance.GetControllerDeviceIds ().Count > id) {
			try {
				text.text = AirConsole.instance.GetNickname (AirConsole.instance.ConvertPlayerNumberToDeviceId (id));
			} catch {
				if (text != null)
					text.text = "NO PLAYER";
			}

			if (AirConsole.instance.GetNickname (AirConsole.instance.ConvertPlayerNumberToDeviceId (id)) == "Guest 0" && text != null)
				text.text = "NO PLAYER";
		//}	
	}
}
