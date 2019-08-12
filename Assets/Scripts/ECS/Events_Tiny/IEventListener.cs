// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi

namespace RD.ECS.Events_Tiny
{
	//todo comments
	public interface IEventListener { }

	public interface IEventListener<TEventData> : IEventListener where TEventData : struct
	{
		void Raise( TEventData eventData );
	}
}