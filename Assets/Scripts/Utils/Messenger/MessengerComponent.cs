// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

	//This manager will ensure that the messenger's eventTable will be cleaned up upon loading of a new level.
	public sealed class MessengerComponent : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad( gameObject );
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}

		//Clean up eventTable every time a new level loads.
		private void SceneManager_sceneLoaded( Scene arg0, LoadSceneMode arg1 )
		{
			Messenger.Cleanup();
		}
	}
