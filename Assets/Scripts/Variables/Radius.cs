using System.Collections.Generic;

public class Radius
{
	private Vec centerPosition;
	private int radius;
	private VecArray<bool> frontier;
	private Vec topLeft;

	public Rect Bounds { get; set; }

	public Radius( Vec centerPosition, int radius, Rect bounds )
	{
		this.centerPosition = centerPosition;
		this.radius = radius;
		this.Bounds = bounds;
		topLeft = centerPosition - radius;
		int size = radius * 2 + 1;
		frontier = new VecArray<bool>( size, size );
	}

	public Radius( Vec centerPosition, int radius ) : this( centerPosition, radius, Rect.Empty )
	{
	}


	public Vec Center
	{
		get => centerPosition;
		set
		{
			centerPosition = value;
			topLeft = centerPosition - radius;
		}
	}

	public int CenterX => centerPosition.x;
	public int CenterY => centerPosition.y;

	public int RadiusArea
	{
		get => radius;
		set
		{
			if ( value != radius )
			{
				radius = value;
				topLeft = centerPosition - radius;
				int size = radius * 2 - 1;
				frontier = new VecArray<bool>( size, size );
			}
		}
	}

	public IEnumerable<Vec> CalculatePositions()
	{
		frontier.Fill( false );
		frontier[ frontier.Width / 2, frontier.Height / 2 ] = true;

		Queue<Vec> q = new Queue<Vec>();
		q.Enqueue( centerPosition );

		Vec current;
		Vec localNeighbor;

		while ( q.Count != 0 )
		{
			current = q.Dequeue();

			yield return current;

			foreach ( Direction dir in Direction.EightDirections() )
			{
				Vec neighbor = current + dir;
				localNeighbor = neighbor + topLeft;

				if ( Distances.DistanceBetweenAdjacent( centerPosition, neighbor ) > radius ||
				     frontier[ localNeighbor ] ||
				     Bounds != Rect.Empty && Bounds.Contains( neighbor )
				)
				{
					continue;
				}

				q.Enqueue( neighbor );
				frontier[ localNeighbor ] = true;
			}
		}
	}
}