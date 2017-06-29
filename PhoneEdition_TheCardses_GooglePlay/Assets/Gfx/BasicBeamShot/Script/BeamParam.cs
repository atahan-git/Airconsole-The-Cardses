﻿using UnityEngine;
using System.Collections;

public class BeamParam : MonoBehaviour {
	
	public Color BeamColor = Color.white;
	public float AnimationSpd = 0.1f;
	public float Scale = 1.0f;
	public float MaxLength = 32.0f;
	public bool bEnd = false;
	public bool bGero = false;

	public void SetBeamParam(BeamParam param)
	{
		if (param != null) {
			this.BeamColor = param.BeamColor;
			this.AnimationSpd = param.AnimationSpd;
			this.Scale = param.Scale;
			this.MaxLength = param.MaxLength;
		}
	}

	void Awake () {
		BeamParam param = this.transform.root.gameObject.GetComponentInChildren<BeamParam>();

		if(param != null)
		{
			this.BeamColor = param.BeamColor;
			this.AnimationSpd = param.AnimationSpd;
			this.Scale = param.Scale;
			this.MaxLength = param.MaxLength;
		}

	}
}
