using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobs.PDF
{
	public class TokenBuffer
	{
		private byte[] _buffer;

		public TokenBuffer()
		{
			Capacity = 256;
			Position = 0;
		}

		public int Capacity { get; private set; }
		public int Position { get; private set; }
		public byte[] Data { get { return _buffer.Take(Position).ToArray(); } }

		public string Value
		{
			get
			{
				AssertSize();
				return Encoding.UTF8.GetString(_buffer, 0, Position);
			}
		}

		public void Add(byte character)
		{
			AssertSize();
			_buffer[Position] = character;
			Position++;
		}

		private void AssertSize()
		{
			if (Position >= Capacity)
				Capacity *= 2;
			Array.Resize(ref _buffer, Capacity);
		}

		internal void Clear()
		{
			Position = 0;
		}
	}
}
