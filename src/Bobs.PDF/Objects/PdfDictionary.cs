using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bobs.PDF.Objects
{
	public class PdfDictionary : Dictionary<string, object>
	{
		internal T Get<T>(string key)
		{
			object value = this[key];
			if ((value != null) && (value.GetType() != typeof(T)))
				throw new InvalidCastException($"Cannot cast object of type {value.GetType().Name} to {typeof(T).Name}!");
			return (T)value;
		}
	}
}
