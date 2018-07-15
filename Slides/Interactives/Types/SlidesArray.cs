using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Types
{
	public class SlidesArray
	{
		public int Length { get; private set; }
		object[] children;
		public SlidesArray(int length)
		{
			Length = length;
			children = new object[length];
		}

		internal void SetAt(int index, object obj)
		{
			children[index] = obj;
		}

		public object GetAt(int index) => children[index];
	}
}
