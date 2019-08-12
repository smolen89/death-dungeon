using System;

namespace RD.ECS.Message
{
	public interface IPublisher : IDisposable
	{
		IDisposable RegisterListener<T>( ActionIn<T> action );

		void Publish<TMessage>( in TMessage eventData ) where TMessage : IMessage;
	}
}