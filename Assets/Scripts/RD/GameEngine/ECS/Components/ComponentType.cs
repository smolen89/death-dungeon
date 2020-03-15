using RD.GameEngine.ECS.Managers;

namespace RD.GameEngine.ECS.Components
{
	internal static class ComponentType<T> where T : IComponent
	{
		static ComponentType()
		{
			CType = ComponentTypeManager.GetTypeFor<T>();

			if ( CType == null )
			{
				CType = new ComponentType();
				ComponentTypeManager.SetTypeFor<T>( CType );
			}
		}

		public static ComponentType CType { get; private set; }
	}

	public sealed class ComponentType
	{
		private static int nextBit;
		private static int nextId;

		static ComponentType()
		{
			nextBit = 1;
			nextId = 0;
		}

		public ComponentType()
		{
			this.Id = nextId++;
			this.Bit = nextBit <<= 1;
		}

		public int Id { get; private set; }
		public int Bit { get; private set; }
	}
}