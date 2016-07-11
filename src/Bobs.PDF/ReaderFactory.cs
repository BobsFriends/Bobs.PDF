using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bobs.PDF
{
	public static class ReaderFactory
	{
		public static Reader Create(Stream stream)
		{
			Tokenizer tokenizer = new Tokenizer(stream);
			return new SequentialReader(tokenizer);
		}

		public static Reader Create(byte[] data)
		{
			Tokenizer tokenizer = new Tokenizer(data);
			return new SequentialReader(tokenizer);
		}

		public static Reader Create(string text)
		{
			Tokenizer tokenizer = new Tokenizer(text);
			return new SequentialReader(tokenizer);
		}
	}
}
