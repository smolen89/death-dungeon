using System;
using System.Collections;
using System.Collections.Generic;

namespace RD.Util
{
	public class ListEx<T> : IEnumerable<T>
	{
		private T[] array;

		public ListEx( int capacity = 16 )
		{
			array = new T[capacity];
			Count = 0;
		}

		public int Capacity => array.Length;
		public bool IsEmpty => Count == 0;
		
		public int Count { get; private set; }

		public T this[ int index ]
		{
			get => array[ index ];
			set
			{
				if ( index >= array.Length )
				{
					Grow( index * 2 );	//todo ogarnąć by nie robiło *2 tylko jakąś subtelną ilość.
					Count = index + 1;
				}
				else if ( index >= Count )
				{
					Count = index + 1;
				}

				array[ index ] = value;
			}
		}

		public void Add( T element )
		{
			if ( Count == array.Length )
			{
				Grow();
			}

			array[ Count++ ] = element;
		}

		public void AddRange( ListEx<T> rangeOfElements )
		{
			for ( int index = 0, j = rangeOfElements.Count; j >index; index++ )
			{
				Add(rangeOfElements.Get(index)  );
			}
		}

		public void Clear()
		{
			for ( int index = Count-1;  index>=0; index-- )
			{
				array[ index ] = default(T);
			}

			Count = 0;
		}

		public bool Contains( T element )
		{
			for (int index = this.Count - 1; index >= 0; --index)
			{
				if (element.Equals(array[index]))
				{
					return true;
				}
			}

			return false;
		}
		
		public T Get(int index)
		{
			return this.array[index];
		}
		
		public T Remove(int index)
		{
			// Make copy of element to remove so it can be returned.
			T result = this.array[index];
			this.Count--;
            
			// Overwrite item to remove with last element.
			this.array[index] = this.array[this.Count];

			// Null last element, so garbage collector can do its work.
			this.array[this.Count] = default(T);
			return result;
		}
		
		public bool Remove(T element)
		{
			for (int index = this.Count - 1; index >= 0; --index)
			{
				if (element.Equals(this.array[index]))
				{
					--this.Count;

					// Overwrite item to remove with last element.
					this.array[index] = this.array[this.Count];
					this.array[this.Count] = default(T);

					return true;
				}
			}

			return false;
		}

		public bool RemoveAll(ListEx<T> list)
		{
			bool isResult = false;
			for (int index = list.Count - 1; index >= 0; --index)
			{
				if (this.Remove(list.Get(index)))
				{
					isResult = true;
				}
			}

			return isResult;
		}
		
		public T RemoveLast()
		{
			if (this.Count > 0)
			{
				this.Count--;
				T result = this.array[this.Count];

				// default(T) if class = null.
				this.array[this.Count] = default(T);
				return result;
			}

			return default(T);
		}
		
		public void Set(int index, T element)
		{
			if (index >= this.array.Length)
			{
				this.Grow(index * 2);
				this.Count = index + 1;
			}
			else if (index >= this.Count)
			{
				this.Count = index + 1;
			}

			this.array[index] = element;
		}
		
		private void Grow()
		{
			this.Grow((int)(this.array.Length * 1.5) + 1);
		}
		
		private void Grow(int newCapacity)
		{
			T[] oldElements = this.array;
			this.array = new T[newCapacity];
			Array.Copy(oldElements, 0, this.array, 0, oldElements.Length);
		}
		
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new ListExEnumerator<T>(this);
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ListExEnumerator<T>(this);
		}
	}
}