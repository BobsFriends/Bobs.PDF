using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Bobs.PDF;

namespace Bobs.PDF.Tests
{
	public class Tokenizer_Multiple_Token_Tests
	{
		[Test]
		public void Reads_Multiple_Tokens()
		{
			// Arrange
			Tokenizer tokenizer	= new Tokenizer(@"%PDF-1.4
1 0 obj
<< /Type /Catalog
 /Outlines 2 0 R
 /Pages 3 0 R
>>
endobj
[nul]
<</Single/Line>>
%%EOF");

			// Act / Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.StartOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Comment));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("PDF-1.4"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\r\n"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Integer));
			Assert.That(tokenizer.Token, Is.EqualTo("1"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Integer));
			Assert.That(tokenizer.Token, Is.EqualTo("0"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Word));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("obj"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\r\n"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.StartOfDictionary));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Name));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("Type"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Name));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("Catalog"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\r\n "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Name));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("Outlines"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Integer));
			Assert.That(tokenizer.Token, Is.EqualTo("2"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Integer));
			Assert.That(tokenizer.Token, Is.EqualTo("0"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Word));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("R"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\r\n "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Name));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("Pages"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Integer));
			Assert.That(tokenizer.Token, Is.EqualTo("3"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Integer));
			Assert.That(tokenizer.Token, Is.EqualTo("0"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(" "));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Word));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("R"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\r\n"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfDictionary));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\r\n"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Word));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("endobj"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\r\n"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.StartOfArray));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Word));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("nul"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfArray));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\r\n"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.StartOfDictionary));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Name));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("Single"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Name));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("Line"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfDictionary));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\r\n"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Comment));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("%EOF"));

			tokenizer.MoveNext();

			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
		}
	}
}
