using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bobs.PDF
{
	public enum TokenType
	{
		Unknown				= -1,
		StartOfFile			= 00,
		StartOfArray,
		StartOfDictionary,
		EndOfFile			= 10,
		EndOfArray,
		EndOfDictionary,
		Comment				= 20,
		Whitespace,
		Number,
		Word,
		Name,
		String,
	}

	public enum TokenSubType
	{
		None,
		Integer,
		Real,
		Literal,
		Hex,
	}
}
