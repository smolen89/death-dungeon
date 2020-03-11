namespace RD.GameEngine.ECS.Components
{
	public interface IComponentPool<T> where T : ComponentPoolable
	{
		T New();
		void CleanUp();
		void ReturnObject( T component );
	}
}