﻿// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;

	public class ThingDisabler : MonoBehaviour
	{
		public ThingRuntimeSet Set;

		public void DisableAll()
		{
			// Loop backwards since the list may change when disabling
			for( int i = Set.Items.Count - 1; i >= 0; i-- )
			{
				Set.Items[i].gameObject.SetActive( false );
			}
		}

		public void DisableRandom()
		{
			int index = Random.Range(0, Set.Items.Count);
			Set.Items[index].gameObject.SetActive( false );
		}
	}
