using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bobs.PDF.Objects;

namespace Bobs.PDF
{
	public abstract class Reader
	{
		public Reader()
		{
			_stackOfStacks.Add(new Stack<object>());
		}

		public abstract void Read();

		protected abstract void ReadHeader();

		protected abstract void ReadBody();

		protected abstract void ReadContent(string stopToken);

		protected abstract void ReadCrossReferenceTable();

		protected abstract void ReadStartOfCrossReferenceTable();

		protected abstract Stream GetStream(int length);




		private List<Stack<object>> _stackOfStacks = new List<Stack<object>>(8);
		private int _stackPointer = 0;

		private Stack<object> Stack { get { return _stackOfStacks[_stackPointer]; } }

		public ObjectStore Store { get; } = new ObjectStore();

		protected void BeginStack()
		{
			_stackPointer++;
			if (_stackPointer >= _stackOfStacks.Count)
				_stackOfStacks.Add(new Stack<object>());
			else if (Stack.Count != 0)
				Stack.Clear();
		}

		protected Stack<object> EndStack()
		{
			Stack<object> stack = Stack;
			_stackPointer--;
			return stack;
		}

		protected void Push(object value)
		{
			Stack.Push(value);
		}

		protected T Pop<T>()
		{
			object popped = Stack.Pop();
			if (popped != null)
			{
				Type poppedType = popped.GetType();
				Type returnType = typeof(T);
				if ((poppedType != returnType) && !returnType.GetTypeInfo().IsAssignableFrom(poppedType.GetTypeInfo()))
				{
					throw new InvalidCastException($"Cannot cast object of type {popped.GetType().Name} to {typeof(T).Name}!");
				}
			}
			return (T)popped;
		}

		protected T Peek<T>()
		{
			object peeked = Stack.Peek();
			if ((peeked != null) && (peeked.GetType() != typeof(T)))
				throw new InvalidCastException($"Cannot cast object of type {peeked.GetType().Name} to {typeof(T).Name}!");
			return (T)peeked;
		}

		protected bool IsEmpty
		{
			get { return Stack.Count == 0; }
		}

		protected void Execute(string word)
		{
			switch (word)
			{
			case "obj":
				{
					BeginStack();
					break;
				}
			case "endobj":
				{
					object value = Pop<object>();
					if (!IsEmpty)
						throw new InvalidOperationException("Current stack should be empty!");
					EndStack();
					ushort generationNumber		= (ushort)Pop<int>();
					int objectNumber			= Pop<int>();
					Store.SetValue(objectNumber, generationNumber, value);
					break;
				}
			case "stream":
				{
					PdfDictionary dictionary = Peek<PdfDictionary>();
					int length = dictionary.Get<int>("Length");
					Push(GetStream(length));
					break;
				}
			case "endstream":
				{
					Stream stream				= Pop<Stream>();
					PdfDictionary dictionary	= Pop<PdfDictionary>();
					Push(new PdfStream(dictionary, stream));
					break;
				}
			case "R":
				{
					ushort generationNumber		= (ushort)Pop<int>();
					int objectNumber			= Pop<int>();
					Push(new IndirectReference(objectNumber, generationNumber));
					break;
				}
			case "nul":
				{
					Push(null);
					break;
				}
			case "true":
				{
					Push(true);
					break;
				}
			case "false":
				{
					Push(false);
					break;
				}
			default:
				{
					throw new NotImplementedException($"Don't know how to Execute {word}!");
				}
			}
		}
	}
}
