using System;
using System.Text;

namespace Bobs.PDF.Objects
{
	internal class PdfString
	{
		private static PdfDocEncoding _pdfDocEncoding	= new PdfDocEncoding();
		private static Encoding _utf16beEncoding		= new UnicodeEncoding(true, true, true);

		public PdfString(byte[] data)
		{
			Data = data;
		}

		public bool Unicode
		{
			get
			{
				return (Data.Length >= 2)
					&& (Data[0] == 0xFE)
					&& (Data[1] == 0xFF);
			}
		}

		public byte[] Data { get; }

		public string _text;
		public string Text
		{
			get
			{
				if (_text == null)
				{
					if (Unicode)
						_text = _utf16beEncoding.GetString(Data);
					else
						_text = _pdfDocEncoding.GetString(Data);
				}
				return _text;
			}
		}

		public override string ToString()
		{
			return Text;
		}
	}
}
