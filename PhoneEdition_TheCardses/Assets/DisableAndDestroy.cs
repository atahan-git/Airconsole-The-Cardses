using UnityEngine;
using System.Collections;

public class DisableAndDestroy : MonoBehaviour {

	ParticleSystem[] myParticles;
	LightBeamsControlScript[] myLights;

	public float destroyTime = 2f;

	// Use this for initialization
	void Start () {
		myParticles = GetComponentsInChildren<ParticleSystem> ();
		myLights = GetComponentsInChildren<LightBeamsControlScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void Engage (string callerDEBUG) {
		//if (isActiveAndEnabled) {
		gameObject.name = Random.Range(245f, 332523f).ToString();
		print(callerDEBUG + " - " + gameObject.name);

		myParticles = GetComponentsInChildren<ParticleSystem> ();
		myLights = GetComponentsInChildren<LightBeamsControlScript> ();

			foreach (ParticleSystem sys in myParticles) {
				sys.Stop ();
			}

			foreach (LightBeamsControlScript lig in myLights) {
				lig.Stop ();
			}


			Destroy (gameObject, destroyTime);
		//}
	}
}
