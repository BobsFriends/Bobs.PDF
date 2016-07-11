using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobs.PDF
{
	public class Tokenizer : IDisposable
	{
		private readonly Stream _stream;
		private readonly bool _streamIsExternal;

		public Tokenizer(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			_streamIsExternal	= true;
			_stream				= stream;
			Reset();
		}

		public Tokenizer(byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			_streamIsExternal	= false;
			_stream				= new MemoryStream(data, false);
			Reset();
		}

		public Tokenizer(string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));


			_streamIsExternal	= false;
			_stream				= new MemoryStream(Encoding.UTF8.GetBytes(text), false);
			Reset();
		}

		public bool Continue { get; private set; }

		public byte CurrentCharacter { get; private set; }

		public TokenType TokenType { get; private set; }
		public TokenSubType SubType { get; private set; }

		public string Token
		{
			get { return Buffer.Value; }
		}

		public TokenBuffer Buffer { get; } = new TokenBuffer();

		public decimal RealValue
		{
			get; private set;
		}
		public decimal IntegerValue
		{
			get; private set;
		}

		private bool ReadNextCharacter()
		{
			if (Continue)
			{
				int nextCharacter   = _stream.ReadByte();
				Continue            = (nextCharacter != -1);
				CurrentCharacter    = (byte)nextCharacter;
			}
			return Continue;
		}

		private bool IsDelimiter
		{
			get
			{
				switch (CurrentCharacter)
				{
				case Character.Percent:
				case Character.Slash:
				case Character.ParenthesisLeft:
				case Character.ParenthesisRight:
				case Character.AngleBracketLeft:
				case Character.AngleBracketRight:
				case Character.SquareBracketLeft:
				case Character.SquareBracketRight:
				case Character.CurlyBracketLeft:
				case Character.CurlyBracketRight:
					return true;
				default:
					return false;
				}
			}
		}

		private bool IsWhitespace
		{
			get
			{
				switch (CurrentCharacter)
				{
				case Character.Null:
				case Character.CarriageReturn:
				case Character.LineFeed:
				case Character.FormFeed:
				case Character.Space:
				case Character.Tab:
					return true;
				default:
					return false;
				}
			}
		}

		private bool IsNumberCharacter
		{
			get
			{
				switch (CurrentCharacter)
				{
				case Character.Plus:
				case Character.Minus:
				case Character.Period:
					return true;
				default:
					return (CurrentCharacter >= Character.Zero)
						&& (CurrentCharacter <= Character.Nine);
				}
			}
		}


		private bool IsRegular
		{
			get
			{
				return !IsWhitespace && !IsDelimiter;
			}
		}

		public bool MoveNext()
		{
			SubType		= TokenSubType.None;

			if (!Continue)
			{
				TokenType	= TokenType.EndOfFile;
				return false;
			}

			TokenType	= TokenType.Unknown;

			Buffer.Clear();
			switch (CurrentCharacter)
			{
			default:
				ReadWord();
				break;
			case Character.Percent:
				ReadComment();
				break;
			case Character.Slash:
				ReadName();
				break;
			case Character.Null:
			case Character.CarriageReturn:
			case Character.LineFeed:
			case Character.FormFeed:
			case Character.Space:
			case Character.Tab:
				ReadWhitespace();
				break;
			case Character.Plus:
			case Character.Minus:
			case Character.Period:
			case Character.Zero:
			case Character.One:
			case Character.Two:
			case Character.Three:
			case Character.Four:
			case Character.Five:
			case Character.Six:
			case Character.Seven:
			case Character.Eight:
			case Character.Nine:
				ReadNumber();
				break;
			case Character.ParenthesisLeft:
				ReadLiteralString();
				break;
			case Character.ParenthesisRight:
				throw new FormatException("Unexpected character ')'!");
			case Character.AngleBracketLeft:
				ReadHexStringOrStartOfDictionary();
				break;
			case Character.AngleBracketRight:
				ReadEndOfDictionary();
				break;
			case Character.SquareBracketLeft:
				ReadStartOfArray();
				break;
			case Character.SquareBracketRight:
				ReadEndOfArray();
				break;
			case Character.CurlyBracketLeft:
			case Character.CurlyBracketRight:
				throw new NotImplementedException();
			}
			return true;
		}

		private void ReadWord()
		{
			TokenType	= TokenType.Word;

			Buffer.Add(CurrentCharacter);
			while (ReadNextCharacter())
			{
				if (IsDelimiter || IsWhitespace)
					return;
				Buffer.Add(CurrentCharacter);
			}
		}

		private void ReadName()
		{
			TokenType	= TokenType.Name;

			while (ReadNextCharacter())
			{
				if (!IsRegular)
					return;
				if (CurrentCharacter == Character.Hash)
				{
					if (!ReadNextCharacter() || !ReadHexPair())
						throw new FormatException("2-digit hexadecimal code expected!");
				}
				else
				{
					Buffer.Add(CurrentCharacter);
				}
			}
		}

		private bool ReadHexPair()
		{
			byte character;
			if ((CurrentCharacter >= Character.Zero) && (CurrentCharacter <= Character.Nine))
				character = (byte)((CurrentCharacter - Character.Zero) << 4);
			else if ((CurrentCharacter >= Character.A) && (CurrentCharacter <= Character.F))
				character = (byte)((CurrentCharacter - Character.A + 10) << 4);
			else if ((CurrentCharacter >= Character.a) && (CurrentCharacter <= Character.f))
				character = (byte)((CurrentCharacter - Character.a + 10) << 4);
			else
				return false;

			try
			{
				if (!ReadNextCharacter())
					return false;

				if ((CurrentCharacter >= Character.Zero) && (CurrentCharacter <= Character.Nine))
					character += (byte)((CurrentCharacter - Character.Zero));
				else if ((CurrentCharacter >= Character.A) && (CurrentCharacter <= Character.F))
					character += (byte)((CurrentCharacter - Character.A + 10));
				else if ((CurrentCharacter >= Character.a) && (CurrentCharacter <= Character.f))
					character += (byte)((CurrentCharacter - Character.a + 10));
				else
					return false;

				return true;
			}
			finally
			{
				Buffer.Add(character);
			}
		}

		private void ReadComment()
		{
			TokenType	= TokenType.Comment;

			while (ReadNextCharacter())
			{
				switch (CurrentCharacter)
				{
				case Character.Null:
				case Character.CarriageReturn:
				case Character.LineFeed:
				case Character.FormFeed:
					return;
				}
				Buffer.Add(CurrentCharacter);
			}
		}

		private void ReadWhitespace()
		{
			TokenType	= TokenType.Whitespace;

			Buffer.Add(CurrentCharacter);
			while (ReadNextCharacter())
			{
				switch (CurrentCharacter)
				{
				case Character.Null:
				case Character.CarriageReturn:
				case Character.LineFeed:
				case Character.FormFeed:
				case Character.Space:
				case Character.Tab:
					break;
				default:
					return;
				}
				Buffer.Add(CurrentCharacter);
			}
		}

		private void ReadNumber()
		{
			TokenType		= TokenType.Number;
			SubType			= TokenSubType.Integer;

			Buffer.Add(CurrentCharacter);

			decimal value	= 0;
			decimal factor	= 1;
			bool negate		= false;

			switch (CurrentCharacter)
			{
			case Character.Plus:
				break;
			case Character.Minus:
				negate			= true;
				break;
			case Character.Period:
				SubType			= TokenSubType.Real;
				IntegerValue	= 0;
				break;
			default:
				value			= CurrentCharacter - Character.Zero;
				break;
			}

			while (ReadNextCharacter() && IsNumberCharacter)
			{
				switch (CurrentCharacter)
				{
				case Character.Plus:
					throw new FormatException("Plus is expected at the beginning of the number.");
				case Character.Minus:
					throw new FormatException("Minus is expected at the beginning of the number.");
				case Character.Period:
					if (SubType == TokenSubType.Real)
						throw new FormatException("A number can only have one period.");
					SubType			= TokenSubType.Real;
					IntegerValue	= negate ? -value : value;
					break;
				default:
					value			= (value * 10) + (CurrentCharacter - Character.Zero);
					if (SubType == TokenSubType.Real)
						factor			*= 0.1m;
					break;
				}
				Buffer.Add(CurrentCharacter);
			}

			if (negate)
				value			= -value;
			RealValue		= value * factor;
			if (SubType == TokenSubType.Integer)
				IntegerValue	= value;
		}

		private void ReadLiteralString()
		{
			TokenType	= TokenType.String;
			SubType		= TokenSubType.Literal;

			int nestingLevel = 0;
			while (ReadStringCharacter(ref nestingLevel))
				;
		}

		private bool ReadStringCharacter(ref int nestingLevel)
		{
			switch (CurrentCharacter)
			{
			case Character.CarriageReturn:
				if (!ReadNextCharacter())
					throw new FormatException("Unexpected end of file in literal string");
				if (CurrentCharacter != Character.LineFeed)
				{
					Buffer.Add(Character.LineFeed);
					return true;
				}
				Buffer.Add(CurrentCharacter);
				break;
			case Character.LineFeed:
			default:
				Buffer.Add(CurrentCharacter);
				break;
			case Character.ParenthesisLeft:
				if (nestingLevel > 0)
					Buffer.Add(CurrentCharacter);
				nestingLevel++;
				break;
			case Character.ParenthesisRight:
				nestingLevel--;
				if (nestingLevel == 0)
				{
					ReadNextCharacter();
					return false;
				}
				Buffer.Add(CurrentCharacter);
				break;
			case Character.Backslash:
				if (!ReadNextCharacter())
					throw new FormatException("Unexpected end of file in literal string");

				switch (CurrentCharacter)
				{
				case (byte)'b':
					Buffer.Add(Character.Backspace);
					break;
				case (byte)'t':
					Buffer.Add(Character.Tab);
					break;
				case (byte)'n':
					Buffer.Add(Character.LineFeed);
					break;
				case (byte)'f':
					Buffer.Add(Character.FormFeed);
					break;
				case (byte)'r':
					Buffer.Add(Character.CarriageReturn);
					break;
				case Character.ParenthesisLeft:
					Buffer.Add(Character.ParenthesisLeft);
					break;
				case Character.ParenthesisRight:
					Buffer.Add(Character.ParenthesisRight);
					break;
				case Character.Backslash:
					Buffer.Add(Character.Backslash);
					break;
				case Character.Zero:
				case Character.One:
				case Character.Two:
				case Character.Three:
				case Character.Four:
				case Character.Five:
				case Character.Six:
				case Character.Seven:
					int code = CurrentCharacter - Character.Zero;
					if (!ReadNextCharacter())
						throw new FormatException("Unexpected end of file in literal string");
					if ((CurrentCharacter >= Character.Zero)
						&& (CurrentCharacter <= Character.Seven))
					{
						code = (code << 3) | (CurrentCharacter - Character.Zero);
						if (!ReadNextCharacter())
							throw new FormatException("Unexpected end of file in literal string");
						if ((CurrentCharacter >= Character.Zero)
							&& (CurrentCharacter <= Character.Seven))
						{
							code = (code << 3) | (CurrentCharacter - Character.Zero);
							if (!ReadNextCharacter())
								throw new FormatException("Unexpected end of file in literal string");
						}
					}
					Buffer.Add((byte)code);
					return true;
				default:
					Buffer.Add(CurrentCharacter);
					break;
				}
				break;
			}
			if (!ReadNextCharacter())
				throw new FormatException("Unexpected end of file in literal string");
			return true;
		}

		private void ReadHexStringOrStartOfDictionary()
		{
			if (!ReadNextCharacter())
				return;
			if (CurrentCharacter == Character.AngleBracketLeft)
				ReadStartOfDictionary();
			else
				ReadHexString();
		}

		private void ReadHexString()
		{
			TokenType	= TokenType.String;
			SubType		= TokenSubType.Hex;

			while (ReadHexPair())
			{
				if (!ReadNextCharacter())
					break;
			}
			if (CurrentCharacter != Character.AngleBracketRight)
				throw new FormatException("Expected character '>'!");
		}

		private void ReadStartOfDictionary()
		{
			TokenType	= TokenType.StartOfDictionary;
			ReadNextCharacter();
		}

		private void ReadEndOfDictionary()
		{
			TokenType	= TokenType.EndOfDictionary;
			if (!ReadNextCharacter()
				|| (CurrentCharacter != Character.AngleBracketRight))
				throw new FormatException("Expected character '>'!");
			ReadNextCharacter();
		}

		private void ReadStartOfArray()
		{
			TokenType	= TokenType.StartOfArray;
			ReadNextCharacter();
		}

		private void ReadEndOfArray()
		{
			TokenType	= TokenType.EndOfArray;
			ReadNextCharacter();
		}

		public void Reset()
		{
			_stream.Seek(0, SeekOrigin.Begin);
			TokenType		= TokenType.StartOfFile;
			SubType			= TokenSubType.None;
			Continue		= true;
			ReadNextCharacter();
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// dispose managed state (managed objects).
					if (!_streamIsExternal)
						_stream.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				disposedValue	= true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Tokenizer() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion
	}
}
