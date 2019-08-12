namespace RD.ECS.Systems
{
	public sealed class SequentialSystem<TUpdate> : ISystemUpdate<TUpdate>
	{
		private readonly ISystemUpdate<TUpdate>[] systems;

		public SequentialSystem( params ISystemUpdate<TUpdate>[] systems )
		{
			//this.systems = systems ?? EmptyArray<ISystemUpdate<TUpdate>>.Value;
			this.systems = systems ?? System.Array.Empty<ISystemUpdate<TUpdate>>();
		}

		public bool IsEnabled { get; set; }

		public void Update( TUpdate dt )
		{
			if( IsEnabled )
			{
				foreach( ISystemUpdate<TUpdate> system in systems )
				{
					system?.Update( dt );
				}
				//for( int i = 0; i < systems.Length; i++ )
				//{
				//	systems[ i ]?.Update( dt );
				//}
			}
		}

		public void Dispose()
		{
			for( int i = systems.Length - 1; i >= 0; i-- )
			{
				systems[ i ].Dispose();
			}
		}
	}
}