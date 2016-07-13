using UnityEngine;
using System.Collections;

public class DataHandler : MonoBehaviour {

	public static int playerCount = 2;
	public static int gridSizeX = 11;
	public static int gridSizeY = 4;

	public static bool isTimeAttack = true; //if not it is score

	public static int gameSetting = 120;

	public static DataHandler s;

	// Use this for initialization
	void Awake () {

		s = this;
		DontDestroyOnLoad (gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
