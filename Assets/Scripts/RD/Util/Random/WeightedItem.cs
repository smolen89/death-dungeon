namespace RD.Util.RND
{
	internal class WeightedItem<T>
	{
		public T Item { get; private set; }

		public int Weight { get; private set; }

		public WeightedItem( T item, int weight )
		{
			Item = item;
			Weight = weight;
		}
	}
}