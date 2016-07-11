using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bobs.PDF
{
	public class SequentialReader : Reader
	{
		public SequentialReader(Tokenizer tokenizer)
		{
			_tokenizer = tokenizer;
		}

		private readonly Tokenizer _tokenizer;

		public override void Read()
		{
			ReadHeader();
		}

		private void ReadHeader()
		{
			_tokenizer.MoveNext();
			if ((_tokenizer.TokenType != TokenType.Comment)
				|| (!_tokenizer.Token.StartsWith("PDF-")))
				throw new FormatException("PDF header expected!");
			string[] version = _tokenizer.Token.Substring(4).Split('.');
			int major = int.Parse(version[0]);
			int minor = int.Parse(version[1]);
		}
	}
}
