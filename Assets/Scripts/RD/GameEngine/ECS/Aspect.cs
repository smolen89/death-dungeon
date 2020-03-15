using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using RD.GameEngine.ECS.Components;
using RD.GameEngine.ECS.Entities;
using RD.GameEngine.ECS.Managers;

namespace RD.GameEngine.ECS
{
	public class Aspect
	{
		protected BigInteger ContainsTypesMap { get; set; }
		protected BigInteger ExcludeTypesMap { get; set; }
		protected BigInteger OneTypesMap { get; set; }

		protected Aspect()
		{
			ContainsTypesMap = 0;
			ExcludeTypesMap = 0;
			OneTypesMap = 0;
		}

		public static Aspect All( params Type[] types ) => new Aspect().GetAll();
		public static Aspect Empty() => new Aspect();
		public static Aspect Exclude( params Type[] types ) => new Aspect().GetExclude( types );
		public static Aspect One( params Type[] types ) => new Aspect().GetOne( types );

		public virtual bool Interests( IEntity entity )
		{
			Debug.Assert( entity != null, $"Entity must not be null." );

			if ( !( ContainsTypesMap > 0 || ExcludeTypesMap > 0 || OneTypesMap > 0 ) )
			{
				return false;
			}

			return ( ( OneTypesMap & entity.TypeBits ) != 0 || OneTypesMap == 0 ) &&
			       ( ( ContainsTypesMap & entity.TypeBits ) == ContainsTypesMap || ContainsTypesMap == 0 ) &&
			       ( ( ExcludeTypesMap & entity.TypeBits ) == 0 || ExcludeTypesMap == 0 );
		}

		public Aspect GetAll( params Type[] types )
		{
			Debug.Assert( types != null, "Types must not be null." );

			foreach ( ComponentType componentType in types.Select( ComponentTypeManager.GetTypeFor ) )
			{
				ContainsTypesMap |= componentType.Bit;
			}

			return this;
		}

		public Aspect GetExclude( params Type[] types )
		{
			Debug.Assert( types != null, "Types must not be null." );

			foreach ( ComponentType componentType in types.Select( ComponentTypeManager.GetTypeFor ) )
			{
				ExcludeTypesMap |= componentType.Bit;
			}

			return this;
		}

		public Aspect GetOne( params Type[] types )
		{
			Debug.Assert( types != null, "Types must not be null." );

			foreach ( ComponentType componentType in types.Select( ComponentTypeManager.GetTypeFor ) )
			{
				OneTypesMap |= componentType.Bit;
			}

			return this;
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder( 1024 );
			
			builder.AppendLine( "Aspect: " );
			AppendTypes( builder, "Requires the components: ", ContainsTypesMap );
			AppendTypes( builder, "Has none of the components: ", ExcludeTypesMap );
			AppendTypes( builder, "Has at lest one of the components: ", OneTypesMap );

			return builder.ToString();
		}

		private static void AppendTypes( in StringBuilder builder, string headerMessage, BigInteger typeBit )
		{
			if ( typeBit != 0 )
			{
				builder.Append( headerMessage );

				foreach ( Type type in ComponentTypeManager.GetTypesFromBits( typeBit ) )
				{
					builder.Append( ", " );
					builder.Append( type.Name );
				}
			}
		}
	}
}