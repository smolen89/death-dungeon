using UnityEngine;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;

namespace RD
{
	internal sealed class Indexer
	{
		private readonly ConcurrentStack<int> freeIDs;
		private int lastID;
		public int LastID => lastID;

		public Indexer( int startID = 0 )
		{
			freeIDs = new ConcurrentStack<int>();
			lastID = startID;
		}

		public int GetFreeID()
		{
			if( !freeIDs.TryPop( out int newID ) )
			{
				newID = Interlocked.Increment( ref lastID );
			}
			return newID;
		}

		public void ReleaseID( int releasedID ) => freeIDs.Push( releasedID );
	}
}