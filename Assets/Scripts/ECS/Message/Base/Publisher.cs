using System;

namespace RD.ECS.Message
{
	internal static class Publisher
	{
		public static void Publish<TMessage>( in TMessage message ) => Publisher<TMessage>.Publish( message );
	}

	internal static class Publisher<T>
	{
		private readonly struct Listener : IDisposable
		{
			private readonly ActionIn<T> action;

			public Listener( ActionIn<T> action )
			{
				this.action = action;
			}

			public void Dispose()
			{
				lock( _lockObject )
				{
					Actions -= action;
				}
			}
		}

		private static readonly object _lockObject;
		public static ActionIn<T> Actions;

		static Publisher()
		{
			_lockObject = new object();
			Actions = null;
			if( typeof( T ) != typeof( WorldDisposedMessage ) )
			{
				Publisher<WorldDisposedMessage>.RegisterListener( On );
			}
		}

		private static void On( in WorldDisposedMessage message )
		{
			lock( _lockObject )
			{
				Actions = null;
			}
		}

		public static IDisposable RegisterListener( ActionIn<T> action )
		{
			lock( _lockObject )
			{
				Actions += action;
			}
			return new Listener( action );
		}

		public static void Publish( in T message )
		{
			Actions?.Invoke( message );
		}
	}
}