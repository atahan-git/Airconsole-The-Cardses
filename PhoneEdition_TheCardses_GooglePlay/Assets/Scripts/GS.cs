using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GS : MonoBehaviour {

	public static GS s;
	public static GameSetting a; //active

	[Header("Game Settings Object")]
	[Header("Access this for all settings!")]

	int _activeGameMode = 0;
	public GameSetting[] allModes;

	// Use this for initialization
	void Awake () {
		if (s != null) {
			Destroy (this.gameObject);
			return;
		} else {
			s = this;
		}
		a = allModes [_activeGameMode].Copy();
	}

	public void ChangeGameMode (int id){
		_activeGameMode = id;
		UpdateMode ();
	}

	void UpdateMode (){
		a = null;
		a = allModes [_activeGameMode].Copy();
		print ("Active Game Settings: " + a.PresetName);
	}
		
}

[System.Serializable]
public class GameSetting {

	public string PresetName = "Default";

	[Header("Card & Grid Settings")]

	public int gridSizeX = 8;
	public int gridSizeY = 4;

	public float gridScaleX = 1.75f;
	public float gridScaleY = 2.25f;
	public float scaleMultiplier = 1.25f;


	[Space]

	[Tooltip("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public Sprite[] cardSprites = new Sprite[16];
	public GameObject card;
	public GameObject getEffectPrefab;

	[Space]

	public float cardReOpenTime = 15f;
	[Tooltip("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public float[] cardChances = new float[15];
	[Tooltip("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public int[] cardScores = new int[15];


	[Header("PLayer Card Selection Settings")]

	public GameObject blueCardSelect;
	public GameObject redCardSelect;
	public GameObject greenCardSelect;
	public GameObject yellowCardSelect;

	[Space]

	public float checkSpeedPlayer = 0.35f;


	[Header("Power Up Settings")]

	public int earth_activeCount = 10;
	public float earth_checkSpeed = 0.35f;
	[Space]
	public float ice_activeTime = 10f;
	[Space]
	public float light_activeTime = 10f;
	[Space]
	public float poison_activeTime = 2f;
	public float poison_checkSpeed = 4f;
	public int poison_damage = 5;
	public int poison_combo = 5;
	[Space]
	public float shadow_activeTime = 4f;

	[Header("NPC Settings")]
	public bool isNPC = false;
	public int npcId = 4;
	public string npcName = "Dragon";
	public GameObject npcScoreboard;

	public GameSetting Copy(){
		GameSetting myCopy = new GameSetting ();
		myCopy.PresetName = PresetName;

		myCopy.gridSizeX = gridSizeX;
		myCopy.gridSizeY = gridSizeY;
		myCopy.gridScaleX = gridScaleX;
		myCopy.gridScaleY = gridScaleY;
		myCopy.scaleMultiplier = scaleMultiplier;

		myCopy.cardSprites = cardSprites;
		myCopy.card = card;
		myCopy.getEffectPrefab = getEffectPrefab;

		myCopy.cardReOpenTime = cardReOpenTime;
		myCopy.cardChances = cardChances;
		myCopy.cardScores = cardScores;

		myCopy.blueCardSelect = blueCardSelect;
		myCopy.redCardSelect = redCardSelect;
		myCopy.greenCardSelect = greenCardSelect;
		myCopy.yellowCardSelect = yellowCardSelect;

		myCopy.checkSpeedPlayer = checkSpeedPlayer;

		myCopy.earth_activeCount = earth_activeCount;
		myCopy.earth_checkSpeed = earth_checkSpeed;

		myCopy.ice_activeTime = ice_activeTime;

		myCopy.light_activeTime = light_activeTime;

		myCopy.poison_activeTime = poison_activeTime;
		myCopy.poison_checkSpeed = poison_checkSpeed;
		myCopy.poison_damage = poison_damage;
		myCopy.poison_combo = poison_combo;

		myCopy.shadow_activeTime = shadow_activeTime;

		myCopy.isNPC = isNPC;
		myCopy.npcId = npcId;
		myCopy.npcName = npcName;
		myCopy.npcScoreboard = npcScoreboard;

		return myCopy;
	}
}