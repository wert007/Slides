using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Components
{
	public class List : Stack
	{
		ListType listType;
		public string Symbol { get; set; }
		public bool NoSpace { get; set; } = false;
		public bool Intend { get; set; } = true;

		public List(string type) : base("v")
		{
			if (type == "u")
			{
				listType = ListType.Unordered;
			}
			else if (type == "o")
			{
				listType = ListType.Ordered;
			}
			else throw new Exception("Use Arguments \"u[nordered]\" or \"o[rdered]\"");
		}

		public void Fill(object array)
		{
			if (array is SlidesArray arr)
			{
				Fill(arr);
			}
			else throw new ArgumentException();
		}

		public void Fill(SlidesArray array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				Add(new Label(Symbol + " " + array.GetAt(i)));
			}
		}
	}

	public enum ListType
	{
		Ordered,
		Unordered
	}
}
