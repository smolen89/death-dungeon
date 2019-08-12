namespace RD.ECS.Events_Tiny
{
	public interface IEventManager
	{
		uint RegisterListener<TEventData>( IEventListener eventListener ) where TEventData : struct;

		void UnregisterListener( uint listenerID );

		void Notify<TEventData>( TEventData eventData, uint destListenerID = uint.MaxValue ) where TEventData : struct;
	}
}