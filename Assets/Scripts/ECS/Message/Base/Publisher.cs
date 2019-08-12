using System;

namespace RD.ECS.Message
{
	internal static class Publisher
	{
		public static void Publish<TMessage>( int worldID, in TMessage message ) => Publisher<TMessage>.Publish( worldID, message );
	}

	internal static class Publisher<T>
	{
		private readonly struct Listener : IDisposable
		{
			private readonly int worldID;
			private readonly ActionIn<T> action;

			public Listener( int worldID, ActionIn<T> action )
			{
				this.worldID = worldID;
				this.action = action;
			}

			public void Dispose()
			{
				lock( _lockObject )
				{
					Actions[ worldID ] -= action;
				}
			}
		}

		private static readonly object _lockObject;
		public static ActionIn<T>[] Actions;

		static Publisher()
		{
			_lockObject = new object();
			Actions = new ActionIn<T>[ 2 ];
			if( typeof( T ) != typeof( WorldDisposedMessage ) )
			{
				Publisher<WorldDisposedMessage>.RegisterListener( 0, On );
			}
		}

		private static void On( in WorldDisposedMessage message )
		{
			lock( _lockObject )
			{
				if( message.WorldID < Actions.Length )
				{
					Actions[ message.WorldID ] = null;
				}
			}
		}

		public static IDisposable RegisterListener( int worldID, ActionIn<T> action )
		{
			lock( _lockObject )
			{
				ArrayExtension.EnsureLength( ref Actions, worldID );
				Actions[ worldID ] += action;
			}
			return new Listener( worldID, action );
		}

		public static void Publish( int worldID, in T message )
		{
			if( worldID < Actions.Length )
			{
				Actions[ worldID ]?.Invoke( message );
			}
		}
	}
}