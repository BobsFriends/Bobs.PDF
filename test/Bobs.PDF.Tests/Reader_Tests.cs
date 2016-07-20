using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Bobs.PDF.Tests
{
	public class Reader_Tests
	{
		[Test]
		public void Read_TestFile()
		{
			string testFileName = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Data", "Hello World.pdf");
			using (Stream testFile = File.Open(testFileName, FileMode.Open))
			{
				// Arrange
				Reader reader = ReaderFactory.Create(testFile);

				// Act / Assert
				reader.Read();
			}
		}
		[Test]
		public void Read_Reference()
		{
			string testFileName = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Data", "pdf_reference_1-7.pdf");
			using (Stream testFile = File.Open(testFileName, FileMode.Open))
			{
				// Arrange
				Reader reader = ReaderFactory.Create(testFile);

				// Act / Assert
				reader.Read();
			}
		}

		public static IEnumerable<string> TestFiles
		{
			get
			{
				string assemblyLocation = Path.GetDirectoryName(typeof(Reader_Tests).GetTypeInfo().Assembly.Location);
				string dataDirectory = Path.GetFullPath(Path.Combine(assemblyLocation, "..", "..", "..", "Data"));
				int prefix = dataDirectory.Length + 1;
				return Directory.EnumerateFiles(dataDirectory, "*.pdf").Select(pdf => pdf.Substring(prefix));
			}
		}

		[Test]
		public void Read([ValueSource(nameof(TestFiles))] string testFileName)
		{
			string assemblyLocation = Path.GetDirectoryName(typeof(Reader_Tests).GetTypeInfo().Assembly.Location);
			string dataDirectory = Path.GetFullPath(Path.Combine(assemblyLocation, "..", "..", "..", "Data"));
			using (Stream testFile = File.Open(Path.Combine(dataDirectory, testFileName), FileMode.Open))
			{
				// Arrange
				Reader reader = ReaderFactory.Create(testFile);

				// Act / Assert
				reader.Read();
			}
		}
	}
}
