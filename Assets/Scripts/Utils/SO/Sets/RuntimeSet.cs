// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System.Collections.Generic;
using UnityEngine;

	public abstract class RuntimeSet<T> : ScriptableObject
	{
		public List<T> Items = new List<T>();

		public void Add( T thing )
		{
			if( !Items.Contains( thing ) )
				Items.Add( thing );
		}

		public void Remove( T thing )
		{
			if( Items.Contains( thing ) )
				Items.Remove( thing );
		}
	}
