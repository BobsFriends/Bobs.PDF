using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bobs.PDF
{
	public static class Character
	{
		// Whitespace
		public const byte Null					= 0x00;
		public const byte Backspace				= 0x08;
		public const byte Tab					= 0x09;
		public const byte LineFeed				= 0x0A;
		public const byte FormFeed				= 0x0C;
		public const byte CarriageReturn		= 0x0D;
		public const byte Space					= 0x20;

		// Comment
		public const byte Percent				= (byte)'%';

		// Name
		public const byte Slash					= (byte)'/';
		public const byte Hash					= (byte)'#';

		// Numbers
		public const byte Plus					= (byte)'+';
		public const byte Minus					= (byte)'-';
		public const byte Period				= (byte)'.';
		public const byte Zero					= (byte)'0';
		public const byte One					= (byte)'1';
		public const byte Two					= (byte)'2';
		public const byte Three					= (byte)'3';
		public const byte Four					= (byte)'4';
		public const byte Five					= (byte)'5';
		public const byte Six					= (byte)'6';
		public const byte Seven					= (byte)'7';
		public const byte Eight					= (byte)'8';
		public const byte Nine					= (byte)'9';

		// Literal Strings
		public const byte ParenthesisLeft		= (byte)'(';
		public const byte ParenthesisRight		= (byte)')';
		public const byte Backslash				= (byte)'\\';

		// Hexadecimal String (Single) / Dictionary (Double)
		public const byte AngleBracketLeft		= (byte)'<';
		public const byte AngleBracketRight		= (byte)'>';

		// Array
		public const byte SquareBracketLeft		= (byte)'[';
		public const byte SquareBracketRight	= (byte)']';

		// Function
		public const byte CurlyBracketLeft		= (byte)'{';
		public const byte CurlyBracketRight		= (byte)'}';

		// Hex
		public const byte A						= (byte)'A';
		public const byte F						= (byte)'F';
		public const byte a						= (byte)'a';
		public const byte f						= (byte)'f';
	}
}
