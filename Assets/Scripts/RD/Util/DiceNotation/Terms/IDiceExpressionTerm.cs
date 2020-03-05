using System.Collections.Generic;
using RD.Util.RND;

namespace RD.Util.DiceNotation.Terms
{
	public interface IDiceExpressionTerm
	{
		IEnumerable<TermResult> GetResults( IRandom random );
		IEnumerable<TermResult> GetResults();
	}
}