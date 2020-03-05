namespace RD.Util.DiceNotation
{
	/// <summary>
	/// The DiceParser interface can be implement to parse string into DiceExpression
	/// </summary>
	public interface IDiceParser
	{
		/// <summary>
		/// Create a new DiceExpression by parsing the specified string.
		/// </summary>
		/// <param name="expression">A dice notation string expression. Ex. 3d6+3.</param>
		/// <returns>A Dice Expression parsed from the specified string.</returns>
		DiceExpression Parse( string expression );
	}
}