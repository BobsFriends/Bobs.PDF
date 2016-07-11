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
		const string PdfSnippet = @"%PDF-1.4
1 0 obj
<< /Type /Catalog
 /Outlines 2 0 R
 /Pages 3 0 R
>>
endobj
[nul]
%%EOF";

		[Test]
		public void Read_Document()
		{
			// Arrange
			Reader reader = ReaderFactory.Create(PdfSnippet);

			// Act / Assert
			reader.Read();
		}

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
