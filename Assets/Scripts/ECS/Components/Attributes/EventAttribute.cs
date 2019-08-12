using System;

namespace RD.ECS.Components
{
	[AttributeUsage( AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = true )]
	public sealed class EventAttribute : Attribute
	{
		public readonly EventTarget eventTarget;
		public readonly EventType eventType;
		public readonly int priority;

		public EventAttribute( EventTarget eventTarget, EventType eventType, int priority )
		{
			this.eventTarget = eventTarget;
			this.eventType = eventType;
			this.priority = priority;
		}
	}

	public enum EventTarget { Any, Self }

	public enum EventType { Added, Removed }
}