using System;

namespace RD.GameEngine.ECS.Systems
{
	/// <summary>
	/// Base interface for update systems.
	/// </summary>
	public interface ISystem<in TState> : IDisposable
	{
		bool IsEnabled { get; set; }
		void Update( TState state );
	}
}