// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Proto
{
	/// <summary>
	/// Ta klasa niekoniecznie będzie używana, jednak ma dość ciekawe podejście do rotacji.
	/// </summary>
	[Obsolete( "Dont use this!" )]
	public static class Directions
	{
		/// <summary>
		/// 8 kierunków. 4 proste i 4 skośne.
		/// </summary>
		public static int[] EightDirections = { 8, 9, 6, 3, 2, 1, 4, 7 };

		/// <summary>
		/// 4 proste kierunki.
		/// </summary>
		public static int[] FourDirections = { 8, 6, 2, 4 };

		/// <summary>
		/// 4 ukośne kierunki
		/// </summary>
		public static int[] DiagonalDirections = { 9, 3, 1, 7 };

		/// <summary>
		/// 8 kierunków plus '5' odpowiadające za bierzącą lokalizację.
		/// </summary>
		public static int[] NineDirections = { 5, 8, 9, 6, 3, 2, 1, 4, 7 };

		/// <summary>
		/// Pary przeciwstawne z 4 kierunków.
		/// </summary>
		public static int[][] FourDirectionPairs = { new[] { 8, 2 }, new[] { 6, 4 } };

		/// <summary>
		/// pary przeciwstawne z 8 kierunków.
		/// </summary>
		public static int[][] EightDirectionPairs =
		{
			new[] { 8, 2 },
			new[] { 9, 1 },
			new[] { 6, 4 },
			new[] { 3, 7 }
		};

		/// <summary>
		/// Obracanie kierunku. Uwzględnia globalną metrykę. (Chebyshev i Dijkstra)
		/// </summary>
		/// <param name="direction">Obecny kierunek</param>
		/// <param name="clockwise">W którym kierunku</param>
		/// <param name="times">Ile razy obrócić</param>
		/// <returns>Nowy kierunek.</returns>
		public static int RotateDirection( this int direction, bool clockwise, int times = 1 )
		{
			if ( Globals.DefaultMetric == DistanceMetric.Chebyshev )
			{
				return direction.RotateEightWayDir( clockwise, times );
			}

			return direction.RotateFourWayDir( clockwise, times );
		}

		public static int RotateEightWayDir( this int direction, bool clockwise, int times = 1 )
		{
			// jeśli jest 5 to nie ma potrzeby zmieniać czegokolwiek.
			if ( direction == 5 )
			{
				return 5;
			}

			// jeśli ilość obróceń jest na minusie to kierunek będzie odwrotny
			if ( times < 0 )
			{
				times = -times;
				clockwise = !clockwise;
			}

			for ( int i = 0; i < times; ++i )
			{
				switch ( direction )
				{
					case 7:
						direction = clockwise ? 8 : 4;

						break;

					case 8:
						direction = clockwise ? 9 : 7;

						break;

					case 9:
						direction = clockwise ? 6 : 8;

						break;

					case 4:
						direction = clockwise ? 7 : 1;

						break;

					case 6:
						direction = clockwise ? 3 : 9;

						break;

					case 1:
						direction = clockwise ? 4 : 2;

						break;

					case 2:
						direction = clockwise ? 1 : 3;

						break;

					case 3:
						direction = clockwise ? 2 : 6;

						break;

					default:
						return 0;
				}
			}

			return direction;
		}

		public static int RotateFourWayDir( this int direction, bool clockwise, int times = 1 )
		{
			if ( direction == 5 )
			{
				return 5;
			}

			if ( times < 0 )
			{
				times = -times;
				clockwise = !clockwise;
			}

			for ( int i = 0; i < times; ++i )
			{
				switch ( direction )
				{
					case 8:
						direction = clockwise ? 6 : 4;

						break;

					case 4:
						direction = clockwise ? 8 : 2;

						break;

					case 6:
						direction = clockwise ? 2 : 8;

						break;

					case 2:
						direction = clockwise ? 4 : 6;

						break;

					default:
						throw new ArgumentException( "Rotate4WayDir accepts only 4-way directions: 2(down), 4(left), 5(neutral), 6(right), and 8(up)." );
				}
			}

			return direction;
		}

		/// <summary>
		/// returns a list of directions: the original direction plus some number of other adjacent
		/// directions. 'distance' is how far you want to go out on each side.
		/// </summary>
		public static List<int> GetArc( this int i, int distance, bool clockwise = true )
		{
			List<int> result = new List<int>();

			for ( int num = -distance; num <= distance; ++num )
			{
				result.Add( i.RotateDirection( clockwise, num ) );
			}

			return result;
		}

		public static int DirectionOf( this Vec position, Vec target )
		{
			return Globals.DefaultMetric == DistanceMetric.Chebyshev 
				? position.EightWayDirectionOf( target ) 
				: position.FourWayDirectionOf( target );
		}

		/// <summary>
		/// determines which of the 8 directions is closest to the actual direction. Ties go to the cardinal directions.
		/// </summary>
		/// <param name="position">Current position</param>
		/// <param name="target">Target position</param>
		/// <returns></returns>
		public static int EightWayDirectionOf( this Vec position, Vec target )
		{
			/*
			  4 - way direction
			  								x = target.x
											y < target.y

												5,8
												5,7
												5,6
												5,5
			x > target.x		1,4	2,4	3,4	4,4	5,4	6,4	7,4	8,4	9,4		x < target.x
			y = target.y						5,3						y = target.y
				 								5,2
				 								5,1
												5,0

						  					x = target.x
											y > target.y
			*/

			/*
				4 - way direction diagonal

					 			1,8								9,8		x - y == target.x - target.y
						 			2,7				 	 	8,7
							 			3,6			 	7,6
											4,5		6,5
												5,4
											4,3		6,3
										3,2				7,2
									2,1						8,1
								1,0								9,0		x + y == target.x + target.y

			*/

			/*
				8 - way direction Adjacent

			 						x > target.x			x < target.x
									y < target.y			y < target.y
									dx < dy					dx < dy

								 	 	2,8	3,8	4,8	 	6,8	7,8	8,8
				x > target.x	 	1,7	 	3,7	4,7		6,7	7,7	 	9,7	 	 x < target.x
				y < target.y	 	1,6	2,6	 	4,6		6,6 	8,6	9,6	 	 y < target.y
				dx > dy			 	1,5	2,5	3,5	 		 	7,5	8,5	9,5	 	 dx > dy
													5,4
				 x > target.x	 	1,3	2,3	3,3	 		 	7,3	8,3	9,3	 	 x < target.x
				 y > target.y	 	1,2	2,2	 	4,2		6,2	 	8,2	9,2	 	 y > target.y
				 dx > dy		 	1,1	 	3,1	4,1		6,1	7,1	 	9,1	 	 dx > dy
								 	 	2,0	3,0	4,0		6,0	7,0	8,0

									 x > target.x		  x < target.x
									 y > target.y		  y > target.y
									 dx < dy			  dx < dy

			 */

			int x = position.x;
			int y = position.y;
			int dx = Mathf.Abs( target.x - x );
			int dy = Mathf.Abs( target.y - y );

			// y = target.y -- poziomo (lewo prawo)
			if ( dy == 0 )
			{
				// jestem po lewej czyli target po prawej
				if ( x < target.x )
					return 6;

				// jestem po prawej czyli target po lewej
				if ( x > target.x )
					return 4;

				// x = y mamy takie same
				if ( dx == 0 )
					return 5;
			}

			// x = target.x -- pionowo (góra dół)
			if ( dx == 0 )
			{
				// jestem u góry czyli target na dole
				if ( y > target.y )
					return 2;

				// jestem niżej czyli target u góry
				if ( y < target.y )
					return 8;
			}

			// Ukośnie z lewej góry => prawy dół
			if ( x + y == target.x + target.y )
			{
				//slope is -1
				if ( x > target.x )
					return 7;

				if ( x < target.x )
					return 3;
			}

			// Ukośnie z prawej góry => lewy dół
			if ( x - y == target.x - target.y )
			{
				//slope is 1
				if ( x > target.x )
					return 1;

				if ( x < target.x )
					return 9;
			}

			// calculate all other dirs here

			int orthogonalDirection; //orthogonal
			int diagonalDirection; //diagonal

			int dprimary = Mathf.Min( dy, dx ); // Smaller
			int dsecondary = Mathf.Max( dy, dx ); // Bigger

			// Prawa strona
			if ( x < target.x )
			{
				// x < target.x

				if ( y > target.y )
				{
					// x < target.x	y > target.y
					diagonalDirection = 3;

					if ( dx > dy )
					{
						//  x < target.x	y > target.y	dx > dy
						orthogonalDirection = 6;
					}
					else
					{
						//  x < target.x	y > target.y	dx < dy
						orthogonalDirection = 2;
					}
				}
				else
				{
					// x < target.x	y < target.y
					diagonalDirection = 1;

					if ( dx > dy )
					{
						// x < target.x	y < target.y	dx > dy
						orthogonalDirection = 6;
					}
					else
					{
						// x < target.x	y < target.y	dx < dy
						orthogonalDirection = 8;
					}
				}
			}
			else
			{
				// x > target.x
				if ( y > target.y )
				{
					// x > target.x		y > target.y
					diagonalDirection = 1;

					if ( dx > dy )
					{
						// x > target.x		y > target.y	dx > dy
						orthogonalDirection = 4;
					}
					else
					{
						// x > target.x		y > target.y	dx < dy
						orthogonalDirection = 2;
					}
				}
				else
				{
					// x > target.x		y < target.y
					diagonalDirection = 7;

					if ( dx > dy )
					{
						// x > target.x		y < target.y	dx > dy
						orthogonalDirection = 4;
					}
					else
					{
						// x > target.x		y < target.y	dx < dy
						orthogonalDirection = 8;
					}
				}
			}

			int tiebreaker = orthogonalDirection;
			float ratio = dprimary / (float) dsecondary;

			if ( ratio < 0.5f )
				return orthogonalDirection;

			if ( ratio > 0.5f )
				return diagonalDirection;

			return tiebreaker;
		}

		public static int FourWayDirectionOf( this Vec position, Vec target )
		{
			//determines which of the 4 directions is closest to the actual direction. Ties go to the vertical directions.
			int x = position.x;
			int y = position.y;
			int dx = Mathf.Abs( target.x - x );
			int dy = Mathf.Abs( target.y - y );

			// Ta sama lokalizacja
			if ( dx == 0 && dy == 0 )
				return 5;

			// mamy napewno po prawej bądź lewej
			if ( dx > dy )
			{
				if ( target.x > x )
					return 6;

				return 4;
			}

			// mamy napewno u góry bądź na dole
			if ( target.y > y )
			{
				//dx <= dy
				return 8;
			}

			return 2;
		}

		/// <summary>
		/// Losowy kierunek (8)
		/// </summary>
		public static int RandomDirection()
		{
			int result = Random.Range( 0, 9 );

			if ( result == 5 )
			{
				result = 9;
			}

			return result;
		}
	}
}