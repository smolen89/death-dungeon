using RD.GameEngine.ECS.Components;

namespace RD.GameEngine.ECS.Systems
{
	public abstract class AComponentSystem<TState, TComponent> : ASystem<TState> where TComponent : IComponent
	{
		//private readonly ComponentPool<TComponent> component;
	}
}