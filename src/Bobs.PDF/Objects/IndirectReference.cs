using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bobs.PDF.Objects
{
	public struct IndirectReference
	{
		public IndirectReference(int objectNumber, ushort generationNumber)
		{
			ObjectNumber		= objectNumber;
			GenerationNumber	= generationNumber;
		}

		public int ObjectNumber { get; }
		public ushort GenerationNumber { get; }

		public override string ToString()
		{
			return $"{ObjectNumber} {GenerationNumber} R";
		}
	}
}
