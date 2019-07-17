// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;

	public class Thing : MonoBehaviour
	{
		public ThingRuntimeSet RuntimeSet;

		private void OnEnable()
		{
			RuntimeSet.Add( this );
		}

		private void OnDisable()
		{
			RuntimeSet.Remove( this );
		}
	}
