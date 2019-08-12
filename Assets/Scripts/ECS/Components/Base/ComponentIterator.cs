using System;
using System.Collections.Generic;

namespace RD.ECS.Components
{
	public class ComponentIterator : IComponentIterator
	{
		protected IDictionary<Type, int> entityComponentsTable;
		protected IDictionary<Type, int> componentsHashesTable;
		protected IList<IList<IComponent>> componentsMatrix;

		protected IEnumerator<KeyValuePair<Type, int>> iterator;

		public ComponentIterator( IDictionary<Type, int> entityComponentsTable, IDictionary<Type, int> componentsHashesTable, IList<IList<IComponent>> componentsMatrix )
		{
			this.entityComponentsTable = entityComponentsTable ?? throw new ArgumentNullException( nameof( entityComponentsTable ) );
			this.componentsHashesTable = componentsHashesTable ?? throw new ArgumentNullException( nameof( componentsHashesTable ) );
			this.componentsMatrix = componentsMatrix ?? throw new ArgumentNullException( nameof( componentsMatrix ) );

			iterator = entityComponentsTable.GetEnumerator();
		}

		public TComponent Get<TComponent>() where TComponent : struct, IComponent
		{
			return (TComponent)Get();
		}

		public IComponent Get()
		{
			KeyValuePair<Type, int> currentComponentInfo = iterator.Current;
			Type cashedComponentType = currentComponentInfo.Key;

			if( cashedComponentType == null || !componentsHashesTable.ContainsKey( cashedComponentType ) )
			{
				throw new InvalidIteratorException();
			}

			int componentsGroupHash = componentsHashesTable[ cashedComponentType ];
			return componentsMatrix[ componentsGroupHash ][ currentComponentInfo.Value ];
		}

		public bool MoveNext()
		{
			return iterator.MoveNext();
		}
	}
}