// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi

using System;
using System.Diagnostics;
using UnityEngine;

namespace Tools
{
	public class Timer
	{
		private float start = 0f;
		private float result = 0f;

		public void Start()
		{
			start = Time.realtimeSinceStartup;
			result = 0f;
		}

		public void Stop() => result = Time.realtimeSinceStartup - start;

		public override string ToString() => $"{result * 1000} ms";

		public static long StopWatch( string name, Action action )
		{
			if( action == null )
			{
				Debugger.LogError( "Timer", $"{name}: Action is null." );
				return -1;
			}

			Stopwatch timer = new Stopwatch();
			timer.Start();
			action.Invoke();
			timer.Stop();

			Debugger.LogInfo( "Timer", $"{name}: {timer.ElapsedMilliseconds} ms." );
			return timer.ElapsedMilliseconds;
		}
	}
}