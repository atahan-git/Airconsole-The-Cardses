using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataLogger : MonoBehaviour {

	public static DataLogger s;

	Text myText;

	// Use this for initialization
	void Awake () {
		s = this;
		myText = GetComponent<Text> ();
	}

	float counter = 0f;
	float otherCounter = 0f;
	float othertimeReq = 1f;
	float timeReq = 0.5f;

	void Update (){
		if (counter > timeReq) {
			if (NextMessage ())
				counter = 0;
		}
		counter += Time.deltaTime;
		otherCounter += Time.deltaTime;
	}

	bool NextMessage (){
		try{
			myText.text = queue.Dequeue ();
			return true;
		}catch{
			if (otherCounter > othertimeReq) {
				myText.text = myText.text + " -";
				otherCounter = 0;
			}
		}
		return false;
	}

	Queue<string> queue = new Queue<string>();

	public void LogMessage (string log){
		queue.Enqueue (log);
	}

	public void LogMessage (string log, bool isError){
		queue.Enqueue (log);
		queue.Enqueue (log);
		queue.Enqueue (log);
		queue.Enqueue (log);
	}
}
