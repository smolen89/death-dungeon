namespace RD.Util.RND
{
	/// <summary>
	/// An Interface for pseudo-random number generators to implement.
	/// Useful when mocking out systems for tests, as the the random number generator can be swapped for
	/// a mock implementation that returns known and expected values
	/// </summary>
	/// <remarks>
	/// Pseudo-random number generators are devices that produces a sequence of numbers 
	/// that meet certain statistical requirements for randomness
	/// </remarks>
	public interface IRandom
	{
		/// <summary>
		/// Gets the next pseudo-random integer between 0 and the specified maxValue inclusive
		/// </summary>
		/// <param name="maxValue">Inclusive maximum result</param>
		/// <returns>Returns a pseudo-random integer between 0 and the specified maxValue inclusive</returns>
		int Next( int maxValue );

		/// <summary>
		/// Gets the next pseudo-random integer between the specified minValue and maxValue inclusive
		/// </summary>
		/// <param name="minValue">Inclusive minimum result</param>
		/// <param name="maxValue">Inclusive maximum result</param>
		/// <returns>Returns a pseudo-random integer between the specified minValue and maxValue inclusive</returns>
		int Next( int minValue, int maxValue );

		/// <summary>
		/// Saves the current state of the pseudo-random number generator
		/// </summary>
		/// <example>
		/// If you generated three random numbers and then called Save to store the state and 
		/// followed that up by generating 10 more numbers before calling Restore with the previously saved RandomState
		/// the Restore method should return the generator back to the state when Save was first called.
		/// This means that if you went on to generate 10 more numbers they would be the same 10 numbers that were
		/// generated the first time after Save was called.
		/// </example>
		/// <returns>A RandomState class representing the current state of this pseudo-random number generator</returns>
		RandomState Save();

		/// <summary>
		/// Restores the state of the pseudo-random number generator based on the specified state parameter
		/// </summary>
		/// <example>
		/// If you generated three random numbers and then called Save to store the state and 
		/// followed that up by generating 10 more numbers before calling Restore with the previously saved RandomState
		/// the Restore method should return the generator back to the state when Save was first called.
		/// This means that if you went on to generate 10 more numbers they would be the same 10 numbers that were
		/// generated the first time after Save was called.
		/// </example>
		/// <param name="state">The state to restore to, usually obtained from calling the Save method</param>
		void Restore( RandomState state );
	}
}