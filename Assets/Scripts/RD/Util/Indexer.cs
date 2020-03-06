using System.Collections.Concurrent;
using System.Threading;

namespace RD.Util
{
	internal sealed class Indexer
	{
		private readonly ConcurrentStack<int> freeIDs;
		private int lastId;
		public int LastId => lastId;

		public Indexer( int startId = 0 )
		{
			freeIDs = new ConcurrentStack<int>();
			lastId = startId;
		}

		public int GetFreeId()
		{
			if( !freeIDs.TryPop( out int newId ) )
			{
				newId = Interlocked.Increment( ref lastId );
			}
			return newId;
		}

		public void ReleaseId( int releasedId ) => freeIDs.Push( releasedId );
	}
}