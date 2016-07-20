using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bobs.PDF.Objects;

namespace Bobs.PDF
{
	public class SequentialReader : Reader
	{
		public SequentialReader(Tokenizer tokenizer)
		{
			_tokenizer = tokenizer;
		}

		private readonly Tokenizer _tokenizer;

		public PdfDictionary TrailerDictionary { get; private set; }

		public override void Read()
		{
			ReadHeader();
			ReadBody();
		}

		protected override void ReadHeader()
		{
			if (!_tokenizer.MoveNext())
				throw new FormatException("PDF header expected!");
			if (!_tokenizer.IsOf(TokenType.Comment)
				|| !_tokenizer.Token.StartsWith("PDF-"))
				throw new FormatException("PDF header expected!");
			string[] version = _tokenizer.Token.Substring(4).Split('.');
			int major = int.Parse(version[0]);
			int minor = int.Parse(version[1]);
		}

		protected override void ReadBody()
		{
			do
			{
				ReadContent("xref", "startxref");
				if (_tokenizer.Token == "xref")
				{
					ReadCrossReferenceTable();
					ReadContent("startxref");
					TrailerDictionary = Pop<PdfDictionary>();
				}
				else
				{
					TrailerDictionary = Store.Entries.Select(e => e.Value).OfType<PdfDictionary>().FirstOrDefault(d => d.Get<string>("Type") == "XRef");
				}
				ReadStartOfCrossReferenceTable();
			}
			while (_tokenizer.TokenType != TokenType.EndOfFile);	// Repeat for updates
		}

		protected override void ReadContent(params string[] stopTokens)
		{
			do
			{
				switch (_tokenizer.TokenType)
				{
				case TokenType.Comment:
				case TokenType.Whitespace:
					{
						// Ignore comments and insignificant white space
						break;
					}
				case TokenType.String:
					{
						Push(new PdfString(_tokenizer.Buffer.Data));
						break;
					}
				case TokenType.Number:
					{
						if (_tokenizer.SubType == TokenSubType.Integer)
							Push(_tokenizer.IntegerValue);
						else if (_tokenizer.SubType == TokenSubType.Real)
							Push(_tokenizer.RealValue);
						else
							throw new InvalidOperationException($"{_tokenizer.SubType} is not a valid subtype of TokenType {_tokenizer.TokenType}!");
						break;
					}
				case TokenType.Name:
					{
						Push(_tokenizer.Token);
						break;
					}
				case TokenType.StartOfDictionary:
					{
						BeginStack();
						break;
					}
				case TokenType.EndOfDictionary:
					{
						PdfDictionary dictionary = new PdfDictionary();
						while (!IsEmpty)
						{
							object value	= Pop<object>();
							string name		= Pop<string>();
							dictionary.Add(name, value);
						}
						EndStack();
						Push(dictionary);
						break;
					}
				case TokenType.StartOfArray:
					{
						BeginStack();
						break;
					}
				case TokenType.EndOfArray:
					{
						Stack<object> elements = EndStack();
						object[] value = elements.Reverse().ToArray();
						elements.Clear();
						Push(value);
						break;
					}
				case TokenType.Word:
					{
						if (stopTokens.Contains(_tokenizer.Token))
							return;
						Execute(_tokenizer.Token);
						break;
					}
				default:
					{
						throw new NotImplementedException($"Don't know how to read TokenType {_tokenizer.TokenType}!");
					}
				}
			}
			while (_tokenizer.MoveNext());
		}

		protected override void ReadCrossReferenceTable()
		{
			_tokenizer.Expect(TokenType.Word, token: "xref");

			_tokenizer.SkipWhitespace();
			while (!_tokenizer.IsOf(TokenType.Word))
			{
				_tokenizer.Expect(TokenType.Number, TokenSubType.Integer);

				int firstObjectNumber = _tokenizer.IntegerValue;

				_tokenizer.SkipWhitespace();
				_tokenizer.Expect(TokenType.Number, TokenSubType.Integer);

				int numberOfEntries = _tokenizer.IntegerValue;

				_tokenizer.SkipWhitespace();

				for (int offset = 0; offset < numberOfEntries; offset++)
				{
					_tokenizer.Expect(TokenType.Number, TokenSubType.Integer);

					int positionOrNumber = _tokenizer.IntegerValue;

					_tokenizer.SkipWhitespace();
					_tokenizer.Expect(TokenType.Number, TokenSubType.Integer);

					ushort generationNumber = (ushort)_tokenizer.IntegerValue;

					_tokenizer.SkipWhitespace();
					_tokenizer.Expect(TokenType.Word);

					if (_tokenizer.Token == "n")
						Store.SetPosition(firstObjectNumber + offset, generationNumber, positionOrNumber);
					else if (_tokenizer.Token == "f")
						Store.Free(firstObjectNumber + offset, generationNumber);
					else
						throw new FormatException($"Expected 'n' or 'f' operator, got {_tokenizer.Token} operator instead!");

					_tokenizer.SkipWhitespace();
				}
			}
			_tokenizer.Expect(TokenType.Word, token: "trailer");
			_tokenizer.SkipWhitespace();
		}

		protected override void ReadStartOfCrossReferenceTable()
		{
			_tokenizer.Expect(TokenType.Word, token: "startxref");
			_tokenizer.SkipWhitespace();
			_tokenizer.Expect(TokenType.Number, TokenSubType.Integer);
			int xrefPosition = _tokenizer.IntegerValue;
			_tokenizer.SkipWhitespace();
			_tokenizer.Expect(TokenType.Comment, token: "%EOF");
			_tokenizer.SkipWhitespace(true);
		}

		protected override Stream GetStream(int length)
		{
			return _tokenizer.ReadStream(length);
		}
	}
}
