using UnityEngine;
using System.Collections;

public class CardGenerator : MonoBehaviour {

	public Vector3[,] grid = new Vector3[4,4];
	public GameObject[,] allCards = new GameObject[4,4];
	public int gridSizeX = 4;
	public int gridSizeY = 4;

	public float gridScaleX = 1.5f;
	public float gridScaleY = 2f;

	public GameObject card;

	// Use this for initialization
	void Start () {

		SetUpGrid ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetUpGrid (){

		//first clean old cards if they exist
		DeleteCards ();

		//give us a better centered position
		transform.position = new Vector3 (transform.position.x - gridScaleX * ((float)gridSizeX/2f - 0.5f), transform.position.y - gridScaleY * ((float)gridSizeY/2f - 0.5f), transform.position.z);

		//set up arrays according to the sizes that are given to us
		grid = new Vector3[gridSizeX,gridSizeY];
		allCards = new GameObject[gridSizeX,gridSizeY];

		//set up grid positions
		for (int i = 0; i < gridSizeX; i++) {
			for (int m = 0; m < gridSizeY; m++) {

				grid [i, m] = new Vector3();
				grid [i, m] = transform.position + new Vector3 (i * gridScaleX, m * gridScaleY, 0);

			}
		}
			
		//instantiate cards at correct positions
		for (int i = 0; i < gridSizeX; i++) {
			for (int m = 0; m < gridSizeY; m++) {

				GameObject myCard = (GameObject)Instantiate (card, grid [i, m], Quaternion.identity);
				allCards [i, m] = myCard;

			}
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

	void RandomizeArray(int[] arr){
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range (0, i);
			int tmp = arr [i];
			arr [i] = arr [r];
			arr [r] = tmp;
		}
	}
}
