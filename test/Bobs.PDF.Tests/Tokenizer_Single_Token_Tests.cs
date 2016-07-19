using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Bobs.PDF;

namespace Bobs.PDF.Tests
{
	public class Tokenizer_Single_Token_Tests
	{
		[Test]
		public void Begins_At_Start_Of_File()
		{
			// Arrange
			Tokenizer tokenizer	= new Tokenizer("");

			// Act

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.StartOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
		}

		[Test]
		public void MoveNext_Reads_End_Of_File()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("");

			// Act
			bool success		= tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Comment()
		{
			// Arrange
			Tokenizer tokenizer	= new Tokenizer("%Blah");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Comment));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("Blah"));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Whitespace()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("\t\r\n");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("\t\r\n"));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Positive_Real()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("1.234");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Real));
			Assert.That(tokenizer.Token, Is.EqualTo("1.234"));
			Assert.That(tokenizer.IntegerValue, Is.EqualTo(1));
			Assert.That(tokenizer.RealValue, Is.EqualTo(1.234));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Negative_Real()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("-1.234");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Real));
			Assert.That(tokenizer.Token, Is.EqualTo("-1.234"));
			Assert.That(tokenizer.IntegerValue, Is.EqualTo(-1));
			Assert.That(tokenizer.RealValue, Is.EqualTo(-1.234));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Positive_Integer()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("1234");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Integer));
			Assert.That(tokenizer.Token, Is.EqualTo("1234"));
			Assert.That(tokenizer.IntegerValue, Is.EqualTo(1234));
			Assert.That(tokenizer.RealValue, Is.EqualTo(1234));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Negative_Integer()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("-1234");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Number));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Integer));
			Assert.That(tokenizer.Token, Is.EqualTo("-1234"));
			Assert.That(tokenizer.IntegerValue, Is.EqualTo(-1234));
			Assert.That(tokenizer.RealValue, Is.EqualTo(-1234));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Word()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("hello");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Word));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("hello"));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_LiteralString()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer(@"(Hello\r \053 
(World))");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.String));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Literal));
			Assert.That(tokenizer.Token, Is.EqualTo("Hello\r + \n(World)"));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_HexString()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("<48656C6C6F>");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.String));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.Hex));
			Assert.That(tokenizer.Token, Is.EqualTo("Hello"));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Name()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("/Hello");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Name));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("Hello"));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Name_With_Hash()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("/Hello#20World");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.Name));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo("Hello World"));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Start_Of_Array()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("[");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.StartOfArray));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_End_Of_Array()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("]");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfArray));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_Start_Of_Dictionary()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer("<<");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.StartOfDictionary));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}

		[Test]
		public void MoveNext_Reads_End_Of_Dictionary()
		{
			// Arrange
			Tokenizer tokenizer = new Tokenizer(">>");

			// Act
			tokenizer.MoveNext();

			// Assert
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfDictionary));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(tokenizer.Token, Is.EqualTo(""));
			bool success        = tokenizer.MoveNext();
			Assert.That(tokenizer.TokenType, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokenizer.SubType, Is.EqualTo(TokenSubType.None));
			Assert.That(success, Is.False);
		}
	}
}
