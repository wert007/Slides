using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Types
{
	public class Thickness
	{
		public StyleUnit Top { get; set; }
		public StyleUnit Right { get; set; }
		public StyleUnit Bottom { get; set; }
		public StyleUnit Left { get; set; }
		
		public Thickness()
		{
			Top = 0;
			Right = 0;
			Bottom = 0;
			Left = 0;
		}

		public Thickness(string thickness)
		{
			Top = StyleUnit.Parse(thickness);
			Right = StyleUnit.Parse(thickness);
			Bottom = StyleUnit.Parse(thickness);
			Left = StyleUnit.Parse(thickness);
		}

		public Thickness(string horizontal, string vertical)
		{
			Top = StyleUnit.Parse(vertical);
			Right = StyleUnit.Parse(horizontal);
			Bottom = StyleUnit.Parse(vertical);
			Left = StyleUnit.Parse(horizontal);
		}

		public Thickness(string t, string r, string b, string l)
		{
			Top = StyleUnit.Parse(t);
			Right = StyleUnit.Parse(r);
			Bottom = StyleUnit.Parse(b);
			Left = StyleUnit.Parse(l);
		}

		public Thickness(StyleUnit thickness)
		{
			Top = thickness;
			Right = thickness;
			Bottom = thickness;
			Left = thickness;
		}

		public Thickness(StyleUnit horizontal, StyleUnit vertical)
		{
			Top = vertical;
			Right = horizontal;
			Bottom = vertical;
			Left = horizontal;
		}

		public Thickness(StyleUnit t, StyleUnit r, StyleUnit b, StyleUnit l)
		{
			Top = t;
			Right = r;
			Bottom = b;
			Left = l;
		}


		public static Thickness operator + (Thickness a, Thickness b)
		{
			return new Thickness(a.Top + b.Top, a.Right + b.Right, a.Bottom + b.Bottom, a.Left + b.Left);
		}
	}
}
