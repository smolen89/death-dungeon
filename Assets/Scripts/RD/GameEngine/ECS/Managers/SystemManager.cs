using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RD.GameEngine.ECS.Components;
using RD.GameEngine.ECS.Entities;
using RD.GameEngine.ECS.Systems;
using RD.GameEngine.ECS.Systems.Attributes;
using RD.Util;

namespace RD.GameEngine.ECS.Managers
{
	public sealed class SystemManager
	{
		private readonly World world;
		private readonly IDictionary<Type, IList> systems;
		private readonly SystenBitManager systenBitManager;
		private readonly ListEx<BaseEntitySystem> mergedListEx;
		private IDictionary<int, SystemLayer> updateLayers;
		private IDictionary<int, SystemLayer> drawLayers;

		internal SystemManager( World world )
		{
			mergedListEx = new ListEx<BaseEntitySystem>();
			drawLayers = new Dictionary<int, SystemLayer>();
			updateLayers = new Dictionary<int, SystemLayer>();
			systenBitManager = new SystenBitManager();
			systems = new Dictionary<Type, IList>();
			this.world = world;
		}

		public ListEx<BaseEntitySystem> Systems => mergedListEx;

		public T SetSystem<T>( T system, GameLoopType gameLoopType, int layer = 0, ExecutionType executionType = ExecutionType.Sync ) where T : BaseEntitySystem
			=> (T) SetSystem( system.GetType(), system, gameLoopType, layer, executionType );

		public List<T> GetSystems<T>() where T : BaseEntitySystem
		{
			systems.TryGetValue( typeof(T), out IList system );

			return (List<T>) system;
		}

		public T GetSystem<T>() where T : BaseEntitySystem
		{
			this.systems.TryGetValue( typeof(T), out IList systems );

			Debug.Assert( systems != null, nameof(this.systems) + " != null" );

			if ( systems != null || systems.Count > 1 )
			{
				throw new InvalidOperationException( $"System list contains more that one element of type {typeof(T)}" );
			}

			return (T) systems[ 0 ];
		}

		internal void InitializeAll( bool processAttributes, IEnumerable<Assembly> assembliesToScan = null )
		{
			if ( processAttributes )
			{
				IDictionary<Type, List<Attribute>> types = assembliesToScan == null
					? AttributesProcessor.Process( AttributesProcessor.SupportedAttributes )
					: AttributesProcessor.Process( AttributesProcessor.SupportedAttributes, assembliesToScan );

				foreach ( KeyValuePair<Type, List<Attribute>> item in types )
				{
					if ( typeof(BaseEntitySystem).IsAssignableFrom( item.Key ) )
					{
						Type type = item.Key;
						var pee = (EntitySystemAttribute) item.Value[ 0 ];
						var instance = (BaseEntitySystem) Activator.CreateInstance( type );
						SetSystem( instance, pee.GameLoopType, pee.Layer, pee.ExecutionType );
					}
					else if ( typeof(IEntityTemplate).IsAssignableFrom( item.Key ) )
					{
						Type type = item.Key;
						var pee = (EntityTemplateAttribute) item.Value[ 0 ];
						var instance = (IEntityTemplate) Activator.CreateInstance( type );
						world.SetEntityTemplate( pee.Name, instance );
					}
					else if ( typeof(ComponentPoolable).IsAssignableFrom( item.Key ) )
					{
						CreatePool( item.Key, item.Value );
					}
				}
			}

			for ( int index = 0; index < mergedListEx.Count; index++ )
			{
				mergedListEx.Get( index ).LoadContent();
			}
		}

		internal void TerminateAll()
		{
			for ( int index = 0; index < Systems.Count; index++ )
			{
				BaseEntitySystem entitySystem = Systems.Get( index );
				entitySystem.UnloadContent();
			}

			Systems.Clear();
		}

		internal void Update() => Process( updateLayers );

		internal void Draw() => Process( drawLayers );

		private static void Process( IDictionary<int, SystemLayer> systemsToProcess )
		{
			foreach ( int item in systemsToProcess.Keys )
			{
				if ( systemsToProcess[ item ].Sync.Count > 0 )
					ProcessBagSync( systemsToProcess[ item ].Sync );

				if ( systemsToProcess[ item ].Async.Count > 0 )
					ProcessBagAsync( systemsToProcess[ item ].Async );
			}
		}

		private static void SetSystem( ref IDictionary<int, SystemLayer> layers, BaseEntitySystem system, int layer, ExecutionType executionType )
		{
			if ( !layers.ContainsKey( layer ) )
			{
				layers[ layer ] = new SystemLayer();
			}

			ListEx<BaseEntitySystem> updateBag = layers[ layer ][ executionType ];

			if ( !updateBag.Contains( system ) )
			{
				updateBag.Add( system );
			}
			#if !FULLDOTNET
			layers = ( from d in layers orderby d.Key ascending select d ).ToDictionary( pair => pair.Key, pair => pair.Value );
			#endif
		}

		private static ComponentPoolable CreateInstance( Type type ) => (ComponentPoolable) Activator.CreateInstance( type );

		private static void ProcessBagSync( ListEx<BaseEntitySystem> entitySystems )
		{
			for ( int index = 0; index < entitySystems.Count; index++ )
			{
				entitySystems.Get( index ).Process();
			}
		}

		private static void ProcessBagAsync( ListEx<BaseEntitySystem> entitySystems )
			=> Parallel.ForEach( entitySystems, entitySystem => entitySystem.Process() );

		private void CreatePool( Type type, IEnumerable<Attribute> attributes )
		{
			ComponentPoolAttribute propertyComponentPool = null;

			foreach ( var componentPool in attributes.OfType<ComponentPoolAttribute>() )
			{
				propertyComponentPool = componentPool;
			}

			MethodInfo[] methods = type.GetMethods();

			IEnumerable<MethodInfo> methodInfos =
				from methodInfo in methods
				let methodAttributes = methodInfo.GetCustomAttributes( false )
				from attribute in methodAttributes.OfType<ComponentCreateAttribute>()
				select methodInfo;

			Func<Type, ComponentPoolable> create = null;

			foreach ( MethodInfo methodInfo in methodInfos )
			{
				create = (Func<Type, ComponentPoolable>) Delegate.CreateDelegate( typeof(Func<Type, ComponentPoolable>), methodInfo );
			}

			if ( create == null )
				create = CreateInstance;

			IComponentPool<ComponentPoolable> pool;

			if ( propertyComponentPool == null )
			{
				throw new NullReferenceException( nameof(propertyComponentPool) );
			}

			if ( !propertyComponentPool.IsSupportMultiThread )
			{
				pool = new ComponentPool<ComponentPoolable>(
					propertyComponentPool.InitialSize,
					propertyComponentPool.ResizeSize,
					propertyComponentPool.IsResisable,
					create,
					type );
			}
			else
			{
				pool = new ComponentPoolMultiThread<ComponentPoolable>(
					propertyComponentPool.InitialSize,
					propertyComponentPool.ResizeSize,
					propertyComponentPool.IsResisable,
					create,
					type );
			}

			world.SetPool( type, pool );
		}

		private BaseEntitySystem SetSystem( Type systemType, BaseEntitySystem system, GameLoopType gameLoopType, int layer = 0, ExecutionType executionType = ExecutionType.Sync )
		{
			system.World = world;

			if ( systems.ContainsKey( systemType ) )
			{
				systems[ systemType ].Add( system );
			}
			else
			{
				Type genericType = typeof(List<>);
				Type listType = genericType.MakeGenericType( systemType );
				systems[ systemType ] = (IList) Activator.CreateInstance( listType );
				systems[ systemType ].Add( system );
			}

			switch ( gameLoopType )
			{
				case GameLoopType.Update:
					SetSystem( ref updateLayers, system, layer, executionType );

					break;

				case GameLoopType.Draw:
					SetSystem( ref drawLayers, system, layer, executionType );

					break;

				default:
					throw new ArgumentOutOfRangeException( nameof(gameLoopType), gameLoopType, null );
			}

			if ( !mergedListEx.Contains( system ) )
			{
				mergedListEx.Add( system );
			}

			system.Bit = systenBitManager.GetBitFor( system );

			return system;
		}

		private sealed class SystemLayer
		{
			public readonly ListEx<BaseEntitySystem> Sync;
			public readonly ListEx<BaseEntitySystem> Async;

			public SystemLayer()
			{
				Sync = new ListEx<BaseEntitySystem>();
				Async = new ListEx<BaseEntitySystem>();
			}

			public ListEx<BaseEntitySystem> this[ ExecutionType executionType ]
			{
				get
				{
					switch ( executionType )
					{
						case ExecutionType.Sync:
							return Sync;

						case ExecutionType.Async:
							return Async;

						default:
							throw new ArgumentOutOfRangeException( nameof(executionType), executionType, null );
					}
				}
			}
		}
	}
}