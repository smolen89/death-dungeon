using System;
using System.Collections.Generic;
using System.Reflection;

namespace RD.GameEngine.ECS.Systems.Attributes
{
	public class AttributesProcessor
	{
		public static readonly List<Type> SupportedAttributes = new List<Type>
		{
			typeof(EntitySystemAttribute),
			typeof(EntityTemplateAttribute),
			typeof(ComponentCreateAttribute),
			typeof(ComponentPoolAttribute)
		};

		public static IDictionary<Type, List<Attribute>> Process( List<Type> supportedAttributes )
		{
			return Process( supportedAttributes, AppDomain.CurrentDomain.GetAssemblies() );
		}

		public static IDictionary<Type, List<Attribute>> Process( List<Type> supportedAttributes, IEnumerable<Assembly> assembliesToScan )
		{
			IDictionary<Type, List<Attribute>> attributeTypes = new Dictionary<Type, List<Attribute>>();

			if ( assembliesToScan == null ) 
				return attributeTypes;

			foreach ( Assembly item in assembliesToScan )
			{
				IEnumerable<Type> types = item.GetTypes();

				foreach ( Type type in types )
				{
					object[] attributes = type.GetCustomAttributes( false );

					foreach ( object attribute in attributes )
					{
						if ( !supportedAttributes.Contains( attribute.GetType() ) ) 
							continue;

						if ( !attributeTypes.ContainsKey( type ) )
							attributeTypes[ type ] = new List<Attribute>();
						
						attributeTypes[type].Add( (Attribute)attribute );
					}
				}
			}

			return attributeTypes;
		}
	}
}