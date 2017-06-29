using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragon : MonoBehaviour {

	float firstActivateTime = 5f;
	float minTime = 5f;
	float maxTime = 10f;

	public GameObject activatePrefab;

	// Use this for initialization
	void Start () {
		StartCoroutine (DragonLogicLoop());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool isActive = true;

	IEnumerator DragonLogicLoop (){

		yield return new WaitForSeconds (firstActivateTime);

		while (isActive) {
			StartCoroutine (SelectSquareCards (SelectRandomCenter()));

			yield return new WaitForSeconds (Random.Range (minTime, maxTime));
		}
	}

	IndividualCard SelectRandomCenter (){
		int id = Random.Range ((int)0, (int)6);
		switch (id) {
		case 0:
			return CardHandler.s.allCards [1, 1].GetComponent<IndividualCard> ();
			break;
		case 1:
			return CardHandler.s.allCards [1, 4].GetComponent<IndividualCard> ();
			break;
		case 2:
			return CardHandler.s.allCards [4, 1].GetComponent<IndividualCard> ();
			break;
		case 3:
			return CardHandler.s.allCards [4, 4].GetComponent<IndividualCard> ();
			break;
		case 4: 
			return CardHandler.s.allCards [7, 1].GetComponent<IndividualCard> ();
			break;
		case 5:
			return CardHandler.s.allCards [7, 4].GetComponent<IndividualCard> ();
			break;
		}
		return CardHandler.s.allCards [7, 4].GetComponent<IndividualCard> ();
	}

	IndividualCard[] mem_Cards = new IndividualCard[9];
	IEnumerator SelectSquareCards (IndividualCard center){


		Instantiate (activatePrefab, center.transform.position, Quaternion.identity);

		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);


		//get cards
		int leftLimit  = (int)Mathf.Clamp (center.x - 1, 0, gridSizeX - 1);
		int rightLimit = (int)Mathf.Clamp (center.x + 1, 0, gridSizeX - 1);
		int downLimit  = (int)Mathf.Clamp (center.y - 1, 0, gridSizeY - 1);
		int upLimit    = (int)Mathf.Clamp (center.y + 1, 0, gridSizeY - 1);

		int n = 0;
		for (int i = leftLimit; i <= rightLimit; i++) {
			for (int m = downLimit; m <= upLimit; m++) {

				IndividualCard myCardS = CardHandler.s.allCards [i, m].GetComponent<IndividualCard> ();

				if (myCardS.cardType != 0) {
					if (myCardS.isSelectable) {

						SelectCard (myCardS, n);
						n++;

						yield return new WaitForSeconds (0.05f);
					}
				}
			}
		}

		yield return new WaitForSeconds (0.3f);

		CheckCards ();
	}

	void SelectCard (IndividualCard myCard, int i){
		myCard.SelectCard ();
		mem_Cards[i] = myCard;
	}

	void CheckCards (){
		CardChecker.s.CheckPowerUp (DataHandler.p_NPC, mem_Cards, 2, CallBack);
	}

	public void CallBack (){
	}
}
