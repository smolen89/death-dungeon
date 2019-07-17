// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;

	[CreateAssetMenu( fileName = "New StringVariable", menuName = "Variables/String Variable" )]
	public class StringVariable : ScriptableObject
	{
		[SerializeField]
		private string value = "";

		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}
	}
