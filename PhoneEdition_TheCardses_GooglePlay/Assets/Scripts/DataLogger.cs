using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataLogger : MonoBehaviour {

	public static DataLogger s;

	Text myText;
	public Text myErrorText;

	// Use this for initialization
	void Awake () {
		if (s != null) {
			Destroy (s.gameObject);
		}
		s = this;
		myText = GetComponent<Text> ();
		LogMessage("log initialized");
		LogMessage("error log initialized", true);
		UpdateVersionText ();
	}

	float counter = 0f;
	float timeReq = 0.5f;
	float otherCounter = 0f;
	string message = " ";

	void Update (){
		if (counter > timeReq) {
			if (NextMessage ())
				counter = 0;
		}
		counter += Time.deltaTime;
		otherCounter += Time.deltaTime;

		myText.text = message + " - " + ((int)otherCounter).ToString ();


		if (errorcounter > errortimeReq) {
			if (NextErrorMessage ())
				errorcounter = 0;
		}
		errorcounter += Time.deltaTime;
		otherCounter2 += Time.deltaTime;

		if (myErrorText != null) {
			myErrorText.text = errormessage + " - " + ((int)otherCounter2).ToString ();
		}
	}

	bool NextMessage (){
		try{
			message = queue.Dequeue ();
			otherCounter = 0;
			return true;
		}catch{
			
		}
		return false;
	}

	Queue<string> queue = new Queue<string>();

	public void LogMessage (string log){
		queue.Enqueue (log);
	}


	float errorcounter = 0f;
	float errortimeReq = 2f;
	float otherCounter2 = 0f;
	string errormessage = " ";

	bool NextErrorMessage (){
		if (myErrorText != null) {
			try {
				errormessage = errorqueue.Dequeue ();
				otherCounter2 = 0;
				return true;
			} catch {
				
				return false;
			}
			return false;
		} else {
			return true;	
		}
		return true;
	}

	Queue<string> errorqueue = new Queue<string>();
	public void LogMessage (string log, bool isError){
		errorqueue.Enqueue (log);
	}

	public Text version;
	public TextAsset versionText;
	void UpdateVersionText (){
		try{
		version.text = GetVersionNumber ();
		}catch{
			Invoke ("UpdateVersionText", 2f);
		}
	}

	string GetVersionNumber (){
		try{
			string content = versionText.text;

			if (content != null) {
				return content;
			} else {
				return " ";
			}
		}catch(System.Exception e){
			DataLogger.s.LogMessage (e.StackTrace);
		}
		return " ";
	}
}
