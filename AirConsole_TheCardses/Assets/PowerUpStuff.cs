﻿using UnityEngine;
using System.Collections;

public class PowerUpStuff : MonoBehaviour {

	public static PowerUpStuff s;

	public float lightTime = 10f;
	public GameObject LightEffect;
	public float shadowTime = 10f;
	public GameObject ShadowEffect;
	public float poisonTime = 10f;
	public GameObject PoisonEffect;
	public float iceTime = 5f;
	public GameObject IceEffect;

	public GameObject FireEffect;

	// Use this for initialization
	void Awake () {
		s = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}