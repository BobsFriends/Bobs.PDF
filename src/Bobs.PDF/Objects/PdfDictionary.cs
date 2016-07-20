using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bobs.PDF.Objects
{
	public class PdfDictionary
	{
		Dictionary<string, object> _values = new Dictionary<string, object>();

		internal void Add(string name, object value)
		{
			_values.Add(name, value);
		}

		internal T Get<T>(string key)
		{
			object value;
			if (!_values.TryGetValue(key, out value))
				return default(T);
			if ((value != null) && (value.GetType() != typeof(T)))
				throw new InvalidCastException($"Cannot cast object of type {value.GetType().Name} to {typeof(T).Name}!");
			return (T)value;
		}

		public override string ToString()
		{
			string type = Get<string>("Type");
			string subType = Get<string>("Subtype");
			int length = Get<int>("Length");
			string filter = Get<string>("Filter");
			return $"<<{(type != null ? "/Type /" + type + " " : "")}{(subType != null ? "/Subtype /" + subType + " " : "")}{(length != 0 ? "/Length " + length + " " : "")}{(filter != null ? "/Filter /" + filter + " " : "")}... [{_values.Count}]>>";
		}
	}
}
