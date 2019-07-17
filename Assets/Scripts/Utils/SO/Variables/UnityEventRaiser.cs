// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;
using UnityEngine.Events;

	public class UnityEventRaiser : MonoBehaviour
	{
		public UnityEvent OnEnableEvent;

		public void OnEnable()
		{
			OnEnableEvent.Invoke();
		}
	}
