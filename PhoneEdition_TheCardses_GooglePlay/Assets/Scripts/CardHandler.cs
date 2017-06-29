using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour {

	public static CardHandler s;


	[HideInInspector]
	public Vector3[,] grid = new Vector3[4, 4];
	[HideInInspector]
	public GameObject[,] allCards = new GameObject[4, 4];


	void Awake(){
		s = this;
	}


	// Use this for initialization
	void Start () {

		s = this;

		SetUpGrid ();

		DataLogger.s.LogMessage("card grid done");
		if (DataHandler.s.myPlayerIdentifier == DataHandler.p_blue) {
			DataLogger.s.LogMessage("initializing starting cards");
			InitializeFirstStartingCards ();
			DataLogger.s.LogMessage("Card intialized succesfully");
		}
	}



	//--------------------------------------------------------Initialization Functions

	public void SetUpGrid (){

		//first clean old cards if they exist
		DeleteCards ();

		//give us a better centered position
		Vector3 center = new Vector3 (transform.position.x - GS.a.gridScaleX * ((float)GS.a.gridSizeX / 2f - 0.5f), transform.position.y - GS.a.gridScaleY * ((float)GS.a.gridSizeY / 2f - 0.5f), transform.position.z);

		//set up arrays according to the sizes that are given to us
		grid = new Vector3[GS.a.gridSizeX, GS.a.gridSizeY];
		allCards = new GameObject[GS.a.gridSizeX, GS.a.gridSizeY];

		//set up grid positions
		for (int i = 0; i < GS.a.gridSizeX; i++) {
			for (int m = 0; m < GS.a.gridSizeY; m++) {

				grid [i, m] = new Vector3 ();
				grid [i, m] = center + new Vector3 (i * GS.a.gridScaleX, m * GS.a.gridScaleY, 0);

			}
		}

		//instantiate cards at correct positions
		for (int i = 0; i < GS.a.gridSizeX; i++) {
			for (int m = 0; m < GS.a.gridSizeY; m++) {

				GameObject myCard = (GameObject)Instantiate (GS.a.card, grid [i, m], Quaternion.identity);
				myCard.transform.parent = transform;
				myCard.transform.position = grid [i, m];
				myCard.transform.localScale = new Vector3 (GS.a.scaleMultiplier, GS.a.scaleMultiplier, GS.a.scaleMultiplier);
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
