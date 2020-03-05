using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RD.Util.DiceNotation
{
	/// <summary>
	/// The Parser class in used to parse a string into a DiceExpression.
	/// </summary>
	public class DiceParser : IDiceParser
	{
		private readonly Regex whitespacePattern;

		/// <summary>Initializes a new instance of the <see cref="DiceParser" /> class.</summary>
		public DiceParser()
		{
			whitespacePattern = new Regex( @"\s+" );
		}

		/// <summary>
		/// Create a new DiceExpression by parsing the specified string.
		/// </summary>
		/// <param name="expression">A dice notation string expression. Ex. 3d6+3.</param>
		/// <returns>A Dice Expression parsed from the specified string.</returns>
		/// <exception cref="ArgumentException">Invalid dice notation supplied</exception>
		public DiceExpression Parse( string expression )
		{
			if ( string.IsNullOrEmpty( expression ) )
			{
				throw new ArgumentException( "A dice notation expression must be supplied.", nameof(expression) );
			}

			string cleanExpression = whitespacePattern.Replace( expression.ToLower(), "" );
			cleanExpression = cleanExpression.Replace( "+-", "-" );

			ParseValues parseValues = new ParseValues().Init();
			var dice = new DiceExpression();

			for ( int i = 0; i < cleanExpression.Length; i++ )
			{
				char c = cleanExpression[ i ];

				if ( char.IsDigit( c ) )
				{
					parseValues.Constant += c.ToString();
				}
				else if ( c == '*' )
				{
					parseValues.Scalar *= int.Parse( parseValues.Constant, CultureInfo.CurrentCulture );
					parseValues.Constant = string.Empty;
				}
				else if ( c == 'd' )
				{
					if ( parseValues.Constant.Length == 0 )
					{
						parseValues.Constant = "1";
					}

					parseValues.Multiplicity = int.Parse( parseValues.Constant, CultureInfo.CurrentCulture );
					parseValues.Constant = string.Empty;
				}
				else if ( c == 'k' )
				{
					string chooseAccum = string.Empty;

					while ( i+1<cleanExpression.Length && char.IsDigit( cleanExpression[i+1] ) )
					{
						chooseAccum += cleanExpression[ i + 1 ].ToString();
						i++;
					}

					parseValues.Choose = int.Parse( chooseAccum, CultureInfo.CurrentCulture );
				}
				else if ( c == '+' )
				{
					Append( dice, parseValues );
					parseValues = new ParseValues().Init();
				}
				else if ( c == '-' )
				{
					Append( dice, parseValues );
					parseValues = new ParseValues().Init();
					parseValues.Scalar = -1;
				}
				else
				{
					throw new ArgumentException("Invalid character in dice expression",nameof(expression));
				}
			}

			Append( dice, parseValues );

			return dice;
		}

		private static void Append( DiceExpression dice, ParseValues parseValues )
		{
			int constant = int.Parse( parseValues.Constant, CultureInfo.CurrentCulture );

			if ( parseValues.Multiplicity == 0 )
			{
				dice.Constant( parseValues.Scalar * constant );
			}
			else
			{
				dice.Dice( parseValues.Multiplicity, constant, parseValues.Scalar, parseValues.Choose );
			}
		}
		private struct ParseValues
		{
			public string Constant;
			public int Scalar;
			public int Multiplicity;
			public int? Choose;

			public ParseValues Init()
			{
				Scalar = 1;
				Constant = string.Empty;

				return this;
			}
		}
	}
}