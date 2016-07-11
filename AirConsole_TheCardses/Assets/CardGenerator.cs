using UnityEngine;
using System.Collections;

public class CardGenerator : MonoBehaviour {

	public Vector3[,] grid = new Vector3[4,4];
	public GameObject[,] Cards = new GameObject[4,4];
	int gridSize = 4;

	public float gridScaleX = 1.5f;
	public float gridScaleY = 2f;

	public GameObject card;

	// Use this for initialization
	void Start () {

		transform.position = new Vector3 (transform.position.x - gridScaleX * 1.5f, transform.position.y - gridScaleY * 1.5f, transform.position.z);

		for (int i = 0; i < gridSize; i++) {
			for (int m = 0; m < gridSize; m++) {

				grid [i, m] = new Vector3();
				grid [i, m] = transform.position + new Vector3 (i * gridScaleX, m * gridScaleY, 0);

			}
		}

		int[] cardCodes = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
		RandomizeArray (cardCodes);

		int n = 0;
		for (int i = 0; i < gridSize; i++) {
			for (int m = 0; m < gridSize; m++) {
				
				GameObject myCard = (GameObject)Instantiate (card, grid [i, m], Quaternion.identity);
				Cards [i, m] = myCard;
				myCard.GetComponent<CardScript> ().cardType = cardCodes [n];
				myCard.GetComponent<CardScript> ().SetColor ();
				n++;

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
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
