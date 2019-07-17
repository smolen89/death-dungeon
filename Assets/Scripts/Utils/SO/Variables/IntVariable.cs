// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;

	[CreateAssetMenu( fileName = "New IntVariable", menuName = "Variables/Int Variable" )]
	public class IntVariable : ScriptableObject
	{
#if UNITY_EDITOR

		[Multiline]
		public string DeveloperDescription = "";
#endif
		public int Value;

		public void SetValue( int value )
		{
			Value = value;
		}

		public void SetValue( IntVariable value )
		{
			Value = value.Value;
		}

		public void ApplyChange( int amount )
		{
			Value += amount;
		}

		public void ApplyChange( IntVariable amount )
		{
			Value += amount.Value;
		}
	}
