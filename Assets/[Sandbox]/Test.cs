using System;
using System.Collections;
using System.Collections.Generic;
using RD.ECS;
using RD.ECS.Components;
using RD.ECS.Contexts;
using RD.ECS.Entities;
using RD.ECS.Message;
using RD.ECS.Systems;
using RD.GameEngine.Events;
using UnityEngine;

public class Test : MonoBehaviour, IDisposable
{
	private World world;

	// Start is called before the first frame update
	private void Start()
	{
		world = new World();
		world.RegisterListener( this );

		world.Publish( new NewMessage( "aaa" ) );
	}

	// Update is called once per frame
	private void Update()
	{
	}

	[RegisterListener]
	private void PropagateEvent( in NewMessage message )
	{
		Debugger.Log( "Test", $"{message.Name}" );
	}

	public readonly struct NewMessage : IMessage
	{
		public readonly string Name;

		public NewMessage( string name ) => this.Name = name;
	}

	public void Dispose() => throw new NotImplementedException();
}