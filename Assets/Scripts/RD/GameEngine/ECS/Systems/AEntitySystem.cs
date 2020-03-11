using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RD.GameEngine.ECS.Entities;
using RD.Util;

namespace RD.GameEngine.ECS.Systems
{
	public abstract class AEntitySystem<TState> : ASystem<TState>
	{
		private readonly IEntitySet set;

		public event ActionIn<IEntity> EntityAdded
		{
			add => set.EntityAdded += value;
			remove => set.EntityAdded -= value;
		}

		public event ActionIn<IEntity> EntityRemoved
		{
			add => set.EntityRemoved += value;
			remove => set.EntityRemoved -= value;
		}

		protected AEntitySystem( IEntitySet set, SystemRunner<TState> runner ) : base( runner )
		{
			this.set = set ?? throw new ArgumentNullException( nameof(set) );
		}

		protected AEntitySystem( IEntitySet set ) : this( set, null )
		{
		}

		protected AEntitySystem( World world, SystemRunner<TState> runner ) : base( runner )
		{
			//set = EntitySetBuilderFactory.Create( GetType() )( world ?? throw new ArgumentNullException( nameof(world) ) ).Build();
		}

		protected AEntitySystem( World world ) : this( world, null )
		{
		}

		protected virtual void Update( TState state, in IEntity entity )
		{
		}

		protected virtual void Update( TState state, in IEntity[] entities )
		{
			foreach ( IEntity entity in entities )
			{
				Update( state, entity );
			}
		}

		public override bool IsEnabled
		{
			get => base.IsEnabled && set.Count > 0;
			set => base.IsEnabled = value;
		}

		internal sealed override void Update( int index, int maxIndex )
		{
			int entitiesToUpdate = set.Count / ( maxIndex + 1 );
			int start = index * entitiesToUpdate;

			if ( index == maxIndex )
			{
				entitiesToUpdate = set.Count - start;
			}

			Update( CurrentState, set.GetEntities( start, entitiesToUpdate ) );
		}

		protected override void PostUpdate( TState state )
		{
			set.Complete();

			base.PostUpdate( state );
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public override void Dispose()
		{
			set.Dispose();
		}
	}
}