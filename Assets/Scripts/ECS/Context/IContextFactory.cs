namespace RD.ECS.Contexts
{
	/// <summary>
	/// The interface describes a functionality of a world context's factory.
	/// </summary>
	public interface IContextFactory
	{
		/// <summary>
		/// The method creates a new instance of IWorldContext type
		/// </summary>
		/// <returns>The method returns a reference to a new instance of IWorldContext type</returns>
		IContext CreateNewContextInstance();
	}
}
