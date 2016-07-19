using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Bobs.PDF.Tests
{
	public class Reader_Tests
	{
		[Test]
		public void Read_TestFile()
		{
			string testFileName = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Hello World.pdf");
			using (Stream testFile = File.Open(testFileName, FileMode.Open))
			{
				// Arrange
				Reader reader = ReaderFactory.Create(testFile);

				// Act / Assert
				reader.Read();
			}
		}
	}
}
