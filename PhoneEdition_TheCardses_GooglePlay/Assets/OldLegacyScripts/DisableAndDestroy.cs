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


	public void Engage () {

		myParticles = GetComponentsInChildren<ParticleSystem> ();
		myLights = GetComponentsInChildren<LightBeamsControlScript> ();

		foreach (ParticleSystem sys in myParticles) {
			if (sys != null)
				sys.Stop ();
		}

		foreach (LightBeamsControlScript lig in myLights) {
			if (lig != null)
				lig.Stop ();
		}


		Destroy (gameObject, destroyTime);

	}
}
