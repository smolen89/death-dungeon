// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;
using UnityEngine.UI;

	[ExecuteInEditMode]
	public class SliderSetter : MonoBehaviour
	{
		public Slider Slider;
		public FloatVariable Variable;

		private void Update()
		{
			if( Slider != null && Variable != null )
				Slider.value = Variable.Value;
		}
	}
