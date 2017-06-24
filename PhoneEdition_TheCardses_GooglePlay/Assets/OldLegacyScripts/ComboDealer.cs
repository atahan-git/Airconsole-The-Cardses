using UnityEngine;
using System.Collections;

public class ComboDealer : MonoBehaviour {

	public float comboTimer;
	public int comboCount = 1;

	public int lastCardType = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddScoreWithComboCheck (int id, int cardType) {
		if (cardType == lastCardType || cardType - 7 == lastCardType) {
			ScoreKeeper.s.AddScore (id, cardType, comboCount);
			comboCount *= 2;

		} else {
			comboCount = 2;
			ScoreKeeper.s.AddScore (id, cardType, 1);
		}

		lastCardType = cardType;
		if (lastCardType >= 8)
			lastCardType -= 7;
	}
}
