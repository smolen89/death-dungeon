using System.Collections;
using System.Collections.Generic;

namespace RD.Util
{
	internal class ListExEnumerator<T> : IEnumerator<T>
	{
		/// <summary>The bag.</summary>
		private volatile ListEx<T> list;

		/// <summary>The index.</summary>
		private volatile int index;

		/// <summary>Initializes a new instance of the <see cref="ListExEnumerator{T}"/> class.</summary>
		/// <param name="list">The bag.</param>
		public ListExEnumerator(ListEx<T> list)
		{
			this.list = list;
			this.Reset();
		}

		/// <summary>Gets the current element in the collection.</summary>
		/// <value>The current element.</value>
		/// <returns>The current element in the collection.</returns>
		T IEnumerator<T>.Current
		{
			get
			{
				return this.list.Get(this.index);
			}
		}

		/// <summary>Gets the current element in the collection.</summary>
		/// <value>The current.</value>
		/// <returns>The current element in the collection.</returns>
		object IEnumerator.Current
		{
			get
			{
				return this.list.Get(this.index);
			}
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			this.list = null;
		}

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
		public bool MoveNext()
		{
			return ++this.index < this.list.Count;
		}

		/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
		public void Reset()
		{
			this.index = -1;
		}
	}
}