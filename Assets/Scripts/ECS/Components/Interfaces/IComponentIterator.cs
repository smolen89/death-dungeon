namespace RD.ECS.Components
{
	/// <summary>
	/// The interface describes a functionality of an iterator that enumerates all components
	/// of particular entity.
	/// </summary>
	public interface IComponentIterator
	{
		//todo add comments
		T Get<T>() where T : struct, IComponent;
		IComponent Get();
		bool MoveNext();
	}
}