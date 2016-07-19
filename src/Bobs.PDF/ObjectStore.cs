using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bobs.PDF.Objects;

namespace Bobs.PDF
{
	public class ObjectStore
	{
		private Dictionary<int, List<ObjectRecord>> _store = new Dictionary<int, List<ObjectRecord>>();

		private class ObjectRecord
		{
			public int? Position { get; set; }
			public object Value { get; set; }
			public bool Loaded { get; set; }
			public bool Free { get; set; }
		}

		public void SetValue(int objectNumber, ushort generationNumber, object value)
		{
			ObjectRecord record		= Get(objectNumber, generationNumber);
			record.Value			= value;
			record.Loaded			= true;
			record.Free				= false;
		}

		public void SetPosition(int objectNumber, ushort generationNumber, int value)
		{
			ObjectRecord record		= Get(objectNumber, generationNumber);
			record.Position			= value;
			record.Free				= false;
		}

		public void Free(int objectNumber, ushort generationNumber)
		{
			ObjectRecord record		= Get(objectNumber, generationNumber);
			record.Free				= true;
		}

		private ObjectRecord Get(int objectNumber, ushort generationNumber)
		{
			List<ObjectRecord> objects;
			if (!_store.TryGetValue(objectNumber, out objects))
			{
				objects = new List<ObjectRecord>();
				_store[objectNumber] = objects;
			}

			while (generationNumber >= objects.Count)
				objects.Add(new ObjectRecord() { Free = true, });

			return objects[generationNumber];
		}
	}
}
