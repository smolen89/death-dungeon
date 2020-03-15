using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using RD.GameEngine.ECS.Entities;

namespace RD.GameEngine.ECS.Systems
{
	public abstract class BaseEntitySystem
	{
		// ReSharper disable once InconsistentNaming
		protected World world;
		// ReSharper enable once InconsistentNaming
		private IDictionary<int, IEntity> actives;
		private readonly Aspect aspect;

		static BaseEntitySystem()
		{
			// Initialize EventManager as BlackBoard
		}

		protected BaseEntitySystem()
		{
			Bit = 0;
			aspect = Aspect.Empty();
			IsEnabled = true;
		}

		protected BaseEntitySystem( Aspect aspect ) : this()
		{
			Debug.Assert( aspect != null, $"Aspect must not be null." );

			this.aspect = aspect;
		}

		//todo Blackboard
		// public static BlackBoard BlackBoard { get; protected set; }

		public IEnumerable<IEntity> ActivesEntities
		{
			get { return actives.Values; }
		}

		public World World
		{
			get { return world; }
			protected internal set
			{
				world = value;

				if ( world.IsSortedEntities )
				{
					actives = new SortedDictionary<int, IEntity>();
				}
				else
				{
					actives = new Dictionary<int, IEntity>();
				}
			}
		}

		public bool IsEnabled { get; set; }
		internal BigInteger Bit { get; set; }

		public Aspect Aspect
		{
			get { return aspect; }
		}

		public virtual void LoadContent()
		{
		}

		public virtual void UnloadContent()
		{
		}

		public virtual void OnAdded( IEntity entity )
		{
		}

		public virtual void OnChanged( IEntity entity )
		{
			Debug.Assert( entity != null, $"Entity must not be null." );
			bool contains = ( Bit & entity.SystemBits ) == Bit;
			bool interest = Aspect.Interests( entity );

			if ( interest && !contains )
			{
				Add( entity );
			}
			else if ( !interest && contains )
			{
				Remove( entity );
			}
			else if ( interest && contains && entity.IsEnabled )
			{
				Enable( entity );
			}
			else if ( interest && contains && !entity.IsEnabled )
			{
				Disable( entity );
			}
		}

		public virtual void OnDisabled( IEntity entity )
		{
		}

		public virtual void OnEnabled( IEntity entity )
		{
		}

		public virtual void OnRemoved( IEntity entity )
		{
		}

		public virtual void Process()
		{
			if ( CheckProcessing() )
			{
				Begin();
				ProcessEntities( actives );
				End();
			}
		}

		public void Toggle() => IsEnabled = !IsEnabled;

		protected void Add( IEntity entity )
		{
			Debug.Assert( entity != null, $"Entity must not be null." );

			entity.AddSystemBit( Bit );

			if ( entity.IsEnabled )
			{
				Enable( entity );
			}

			OnAdded( entity );
		}

		protected virtual void Begin()
		{
		}

		protected virtual bool CheckProcessing() => IsEnabled;

		protected virtual void End()
		{
		}

		protected virtual bool Interests( IEntity entity ) => Aspect.Interests( entity );

		protected virtual void ProcessEntities( IDictionary<int, IEntity> entities )
		{
		}

		protected virtual void Remove( IEntity entity )
		{
			Debug.Assert( entity != null, $"Entity must not be null." );

			entity.RemoveSystemBit( Bit );

			if ( entity.IsEnabled )
			{
				Disable( entity );
			}

			OnRemoved( entity );
		}

		private void Disable( IEntity entity )
		{
			Debug.Assert( entity != null, $"Entity must not be null." );

			if ( !actives.ContainsKey( entity.Id ) )
			{
				return;
			}

			actives.Remove( entity.Id );
			OnDisabled( entity );
		}

		private void Enable( IEntity entity )
		{
			Debug.Assert( entity!=null,"Entity must not be null." );

			if ( actives.ContainsKey( entity.Id ) )
			{
				return;
			}

			actives.Add( entity.Id, entity );
			OnEnabled( entity );
		}
	}
}