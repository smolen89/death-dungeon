using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RD.GameEngine.ECS.Components
{
	public static class ComponentTypeManager
	{
		private static readonly Dictionary<Type, ComponentType> ComponentTypes = new Dictionary<Type, ComponentType>();

		public static int GetBit<T>() where T : IComponent => GetTypeFor<T>().Bit;
		public static int GetId<T>() where T : IComponent => GetTypeFor<T>().Id;
		
		public static ComponentType GetTypeFor<T>() where T : IComponent => GetTypeFor( typeof(T) );
		internal static void SetTypeFor<T>( ComponentType type ) => ComponentTypes.Add( typeof(T), type );
		
		public static ComponentType GetTypeFor( Type component )
		{
			Debug.Assert( component != null, "Component must not be null." );


			if ( ComponentTypes.TryGetValue( component, out ComponentType result ) ) 
				return result;

			result = new ComponentType();
			ComponentTypes.Add( component, result );

			return result;
		}

		internal static IEnumerable<Type> GetTypesFromBits( int bits )
		{
			foreach ( KeyValuePair<Type,ComponentType> keyValuePair in ComponentTypes )
			{
				if ((keyValuePair.Value.Bit & bits)!=0)
				{
					yield return keyValuePair.Key;
				}
			}
		}

		public static void Initialize( IEnumerable<Type> types, bool ignoreInvalidTypes = false )
		{
			foreach ( Type type in types )
			{
				if ( typeof(IComponent).IsAssignableFrom( type ) )
				{
					if (type.IsInterface) continue;

					if (type == typeof(ComponentPoolable)) continue;

					GetTypeFor( type );
				}
				else if (!ignoreInvalidTypes)
				{
					throw new ArgumentException($"Type{type} does not implement {typeof(IComponent)} interface.");
				}
			}
		}

		public static void Initialize( params Assembly[] assembliesToScan )
		{
			if ( assembliesToScan.Length == 0 )
				assembliesToScan = AppDomain.CurrentDomain.GetAssemblies().ToArray();

			foreach ( Assembly assembly in assembliesToScan )
			{
				IEnumerable<Type> types = assembly.GetTypes();
				Initialize( types,true );
			}
		}
	}
}