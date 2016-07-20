using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bobs.PDF.Objects
{
	public class PdfStream
	{
		public PdfStream(PdfDictionary dictionary, Stream stream)
		{
			Dictionary	= dictionary;
			Stream		= stream;
		}

		public PdfDictionary Dictionary { get; }
		public Stream Stream { get; }

		public override string ToString()
		{
			return $"{Dictionary}, {Stream}";
		}
	}
}
