﻿// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;
using UnityEngine.Audio;

	/// <summary>
	/// Takes a FloatVariable and sends a curve filtered version of its value
	/// to an exposed audio mixer parameter every frame on Update.
	/// </summary>
	public class AudioParameterSetter : MonoBehaviour
	{
		[Tooltip("Mixer to set the parameter in.")]
		public AudioMixer Mixer;

		[Tooltip("Name of the parameter to set in the mixer.")]
		public string ParameterName = "";

		[Tooltip("Variable to send to the mixer parameter.")]
		public FloatVariable Variable;

		[Tooltip("Minimum value of the Variable that is mapped to the curve.")]
		public FloatReference Min;

		[Tooltip("Maximum value of the Variable that is mapped to the curve.")]
		public FloatReference Max;

		[Tooltip("Curve to evaluate in order to look up a final value to send as the parameter.\n" +
				 "T=0 is when Variable == Min\n" +
				 "T=1 is when Variable == Max")]
		public AnimationCurve Curve;

		private void Update()
		{
			float t = Mathf.InverseLerp(Min.Value, Max.Value, Variable.Value);
			float value = Curve.Evaluate(Mathf.Clamp01(t));
			Mixer.SetFloat( ParameterName, value );
		}
	}
