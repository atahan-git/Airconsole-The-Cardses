﻿using UnityEngine;
using System.Collections;

public class GetColor_Particle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
		ps.startColor = this.transform.root.gameObject.GetComponentInChildren<BeamParam>().BeamColor;
		ps.startSize *= this.transform.root.gameObject.GetComponentInChildren<BeamParam>().Scale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
