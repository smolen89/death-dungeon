﻿// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;

	[CreateAssetMenu( fileName = "New FloatVariable", menuName = "Variables/Float Variable" )]
	public class FloatVariable : ScriptableObject
	{
#if UNITY_EDITOR

		[Multiline]
		public string DeveloperDescription = "";
#endif
		public float Value;

		public void SetValue( float value )
		{
			Value = value;
		}

		public void SetValue( FloatVariable value )
		{
			Value = value.Value;
		}

		public void ApplyChange( float amount )
		{
			Value += amount;
		}

		public void ApplyChange( FloatVariable amount )
		{
			Value += amount.Value;
		}
	}
