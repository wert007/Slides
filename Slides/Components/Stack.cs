using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Components
{
	public class Stack : Element
	{
		public Orientation ContentOrientation { get; set; }
		public new Horizontal HorizontalAlignment
		{
			get { return base.HorizontalAlignment; }
			set
			{
				content.ForEach(c => c.HorizontalAlignment = value);
				base.HorizontalAlignment = value;
			}
		}
		public new Vertical VerticalAlignment
		{
			get { return base.VerticalAlignment; }
			set
			{
				content.ForEach(c => c.VerticalAlignment = value);
				base.VerticalAlignment = value;
			}
		}
		public new object Orientation
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
		public new Color Foreground
		{
			get { return base.Foreground; }
			set
			{
				base.Foreground = value;
				foreach (var item in content)
				{
					item.Foreground = value;
				}
			}
		}
		protected List<Element> content;
		public ElementCollection Content => new ElementCollection(content.ToArray());

		public Stack(string orientation) : base("")
		{
			content = new List<Element>();
			switch (orientation)
			{
				case "v":
				case "vertical":
					ContentOrientation = Components.Orientation.Vertical;
					break;
				case "h":
				case "horizontal":
					ContentOrientation = Components.Orientation.Horizontal;
					break;
				default:
					throw new ArgumentException("Orientation " + orientation + " is not accepted. Try <horizontal>, <vertical>, <h> or <v>.");
			}
		}

		public void Add(Element element)
		{
			content.Add(element);
		}
	}

	public enum Orientation
	{
		Horizontal,
		Vertical
	}
}
