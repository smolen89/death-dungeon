// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;
using UnityEngine.Events;

	public class GameEventListener : MonoBehaviour
	{
		[Tooltip("Event to register with.")]
		public GameEvent Event;

		[Tooltip("Response to invoke when Event is raised.")]
		public UnityEvent Response;

		private void OnEnable()
		{
			Event.RegisterListener( this );
		}

		private void OnDisable()
		{
			Event.UnregisterListener( this );
		}

		public void OnEventRaised()
		{
			Response.Invoke();
		}
	}
