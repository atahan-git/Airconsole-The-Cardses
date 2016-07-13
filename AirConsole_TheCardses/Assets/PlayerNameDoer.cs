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
		AirConsole.instance.onConnect += UpdateName;
		AirConsole.instance.onDisconnect += UpdateName;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void UpdateName(int rand){
		try {
			text.text = AirConsole.instance.GetNickname (AirConsole.instance.ConvertPlayerNumberToDeviceId (id));
		} catch {
			text.text = "NO PLAYER";
		}
	}
}
