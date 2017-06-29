using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour {

	public static SceneMaster s;

	public bool levelLoadLock = false;

	void Awake (){
		s = this;
	}

	int menuId = 0;
	public void LoadMenu (){
		
		LoadLevel (menuId);

	}

	int multiplayerId = 1;
	public void LoadMultiplayer (){
		GS.s.ChangeGameMode (0);
		LoadLevel (multiplayerId);
	}

	int singleStartId = 2;
	public void LoadSinglePLayerLevel (int id){
		GS.s.ChangeGameMode(id+1);
		print (id+1);
		LoadLevel(singleStartId + id);
	}

	void LoadLevel (int id){
		if (!levelLoadLock) {
			try{
			SceneManager.LoadScene (id);
			levelLoadLock = true;
			}catch{
				DataLogger.s.LogMessage ("Level " + id.ToString() + " doesn't exist");
			}
		}
	}

	void OnLevelWasLoaded (){
		levelLoadLock = false;
	}
}
