using Slides.Interactives.Types;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Components
{ 
	public class Element
	{
		public Vector Pos { get; set; }
		public Color Border { get; set; }
		public Thickness BorderThickness { get; set; }
		public Background Background { get; set; }
		public Color Foreground { get; set; }
		public Horizontal HorizontalAlignment { get;
			set; }
		public Vertical VerticalAlignment { get;
			set; }
		public Thickness Margin { get; set; }
		public Thickness Padding { get; set; }
		public object Orientation
		{
			set
			{
				if (value is Horizontal h)
					HorizontalAlignment = h;
				else if (value is Vertical v)
					VerticalAlignment = v;
				else
					throw new ArgumentException();
			}
		}
		public string Name { get; private set; }

		public Element(string name)
		{
			this.Name = name;
			Pos = new Vector();
			Border = null;
			BorderThickness = new Thickness();
			Background = null;
			Margin = new Thickness();
			Padding = new Thickness();
			HorizontalAlignment = Horizontal.Left;
			VerticalAlignment = Vertical.Top;
		}

		public static void SetName(object element, string name)
		{
			if (element is Element e)
				e.Name = name;
		}
	}

	public enum Vertical
	{
		Top,
		Center,
		Bottom
	}

	public enum Horizontal
	{
		Left,
		Center,
		Right
	}
}
