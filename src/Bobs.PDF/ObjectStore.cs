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
		const ushort GenerationFree = 0xFFFF;

		private class ObjectRecord
		{
			public int? Position { get; set; }
			public object Value { get; set; }
			public bool Loaded { get; set; }
			public bool Free { get; set; }

			public override string ToString()
			{
				return $"{Position:0000000000} '{Value}' (L={Loaded}, F={Free})";
			}
		}

		public IEnumerable<KeyValuePair<IndirectReference, object>> Entries
		{
			get
			{
				return _store
					.SelectMany(e
					=> e.Value
						.Where(o => o.Free == false)
						.Select((o, i)
						=> new KeyValuePair<IndirectReference, object>(
							new IndirectReference(e.Key, (ushort)i),
							o.Value)));
			}
		}

		public void SetValue(int objectNumber, ushort generationNumber, object value)
		{
			if (generationNumber == GenerationFree)
				throw new InvalidOperationException();

			ObjectRecord record		= Get(objectNumber, generationNumber);
			record.Value			= value;
			record.Loaded			= true;
			record.Free				= false;
		}

		public void SetPosition(int objectNumber, ushort generationNumber, int value)
		{
			if (generationNumber == GenerationFree)
				throw new InvalidOperationException();

			ObjectRecord record		= Get(objectNumber, generationNumber);
			record.Position			= value;
			record.Free				= false;
		}

		public void Free(int objectNumber, ushort generationNumber)
		{
			if (generationNumber == GenerationFree)
				return;

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
