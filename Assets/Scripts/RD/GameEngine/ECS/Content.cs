using System;
using RD.GameEngine.ECS.Entities;
using RD.Util;
using UnityEditor;

namespace RD.GameEngine.ECS
{
	public static class Worlds
	{
		internal static readonly object Sync;
		internal static readonly Indexer WorldIndexer;
		internal static World[] WorldList;

		static Worlds()
		{
			Sync = new object();
			WorldIndexer = new Indexer( 0 );
			WorldList = new World[2];
		}
	}

	public sealed class World : IDisposable
	{
		private readonly Indexer entityIndexer;
		
		internal readonly int Id;
		internal int LastEntityId => entityIndexer.LastId;

		public readonly int MaxEntityCount;
		public event Action<IEntity> EntityDisposed;

		public Entity[] Entities;

		public World( int maxEntityCount )
		{
			if ( maxEntityCount < 0 )
			{
				throw new ArgumentException( "Argument cannot be negative", nameof(maxEntityCount) );
			}

			MaxEntityCount = maxEntityCount;
			
			// Setup World
			Id = Worlds.WorldIndexer.GetFreeId();

			// Setup Entities etc.
			entityIndexer = new Indexer( -1 );
			Entities = EmptyArray<Entity>.Value;

			// Add this world to world List
			lock ( Worlds.Sync )
			{
				Util.ArrayExtension.EnsureLength( ref Worlds.WorldList, Id );
				Worlds.WorldList[ Id ] = this;
			}

			//todo Add this to Subscription
		}

		public World() : this( int.MaxValue )
		{
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			lock ( Worlds.Sync )
			{
				Worlds.WorldList[ Id ] = null;
			}
			// todo publish the message on Menaged Resource Release all message
			// todo publish World Disposed Message
			
			Worlds.WorldIndexer.ReleaseId( Id );
			// ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
			GC.SuppressFinalize( this );
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString() => $"World {Id}";
	}
}