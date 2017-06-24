using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour {

	public static CardHandler s;


	[HideInInspector]
	public Vector3[,] grid = new Vector3[4, 4];
	[HideInInspector]
	public GameObject[,] allCards = new GameObject[4, 4];

	public int gridSizeX = 10;
	public int gridSizeY = 5;

	public float gridScaleX = 1.49f;
	public float gridScaleY = 1.99f;
	public float scaleMultiplier = 1f;

	[Space]

	public GameObject card;

	[Space]

	public int dragonChance = 5;
	public float cardReOpenTime = 15f;


	void Awake(){
		s = this;
	}


	// Use this for initialization
	void Start () {
		s = this;

		SetUpGrid ();

		GoogleAPI.s.logText.LogMessage("card grid done");
		if (DataHandler.s.myPlayerIdentifier == DataHandler.p_blue) {
			GoogleAPI.s.logText.LogMessage("initializing starting cards");
			InitializeFirstStartingCards ();
			GoogleAPI.s.logText.LogMessage("Card intialized succesfully");
		}
	}



	//--------------------------------------------------------Initialization Functions

	public void SetUpGrid (){

		//first clean old cards if they exist
		DeleteCards ();

		//give us a better centered position
		Vector3 center = new Vector3 (transform.position.x - gridScaleX * ((float)gridSizeX/2f - 0.5f), transform.position.y - gridScaleY * ((float)gridSizeY/2f - 0.5f), transform.position.z);

		//set up arrays according to the sizes that are given to us
		grid = new Vector3[gridSizeX,gridSizeY];
		allCards = new GameObject[gridSizeX,gridSizeY];

		//set up grid positions
		for (int i = 0; i < gridSizeX; i++) {
			for (int m = 0; m < gridSizeY; m++) {

				grid [i, m] = new Vector3();
				grid [i, m] = center + new Vector3 (i * gridScaleX, m * gridScaleY, 0);

			}
		}

		//instantiate cards at correct positions
		for (int i = 0; i < gridSizeX; i++) {
			for (int m = 0; m < gridSizeY; m++) {

				GameObject myCard = (GameObject)Instantiate (card, grid [i, m], Quaternion.identity);
				myCard.transform.parent = transform;
				myCard.transform.position = grid [i, m];
				myCard.transform.localScale = new Vector3 (scaleMultiplier, scaleMultiplier, scaleMultiplier);
				allCards [i, m] = myCard;
				myCard.GetComponent<IndividualCard> ().x = i;
				myCard.GetComponent<IndividualCard> ().y = m;

			}
		}
	}

	public void InitializeFirstStartingCards(){
		foreach (GameObject card in allCards) {
			card.GetComponent<IndividualCard> ().RandomizeCardType ();
		}
	}

	void DeleteCards (){
		for (int i = 0; i < allCards.GetLength(0); i++) {
			for (int m = 0; m < allCards.GetLength(1); m++) {
				if (allCards[i,m] != null) {
					Destroy (allCards [i, m].gameObject);
					allCards [i, m] = null;
				}
			}
		}
	}


	//--------------------------------------------------------Helper Functions
	public enum CardActions {Select, UnSelect, Match, ReOpen}

	public void AccessCard (int x, int y, CardActions action){
		switch (action) {
		case CardActions.Select:
			allCards [x, y].GetComponent<IndividualCard> ().SelectCard ();
			break;
		case CardActions.UnSelect:
			allCards [x, y].GetComponent<IndividualCard> ().UnSelectCard ();
			break;
		case CardActions.Match:
			allCards [x, y].GetComponent<IndividualCard> ().MatchCard ();
			break;
		case CardActions.ReOpen:
			allCards [x, y].GetComponent<IndividualCard> ().ReOpenCard ();
			break;
		default:

			break;
		}

		//DataHandler.s.SendCardAction (x, y, action);
	}

	public void UpdateCardType (int x, int y, int type){
		allCards [x, y].GetComponent<IndividualCard> ().UpdateCardType (type);
			
	}
}
