using System;

namespace RD.ECS.Systems
{
	public interface ISystem : IDisposable
	{
		bool IsEnabled { get; set; }
	}

	public interface ISystemUpdate<in TUpdate> : ISystem
	{
		void Update( TUpdate dt );
	}

	public interface ISystemInitialize : ISystem
	{
		void Initialize();
	}
}