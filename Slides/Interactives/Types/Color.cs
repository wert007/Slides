using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Types
{
	public class Color
	{
		byte red;
		byte green;
		byte blue;

		public string R
		{
			get { return red.ToString(); }
			set { red = byte.Parse(value); }
		}
		public string G
		{
			get { return green.ToString(); }
			set { green = byte.Parse(value); }
		}
		public string B
		{
			get { return blue.ToString(); }
			set { blue = byte.Parse(value); }
		}

		public Color(byte red, byte green, byte blue)
		{
			this.red = red;
			this.green = green;
			this.blue = blue;
		}

		public Color(string red, string green, string blue)
		{
			R = red;
			G = green;
			B = blue;
		}

		public static Color Red => new Color(255, 0, 0);
		public static Color Green => new Color(0, 255, 0);
		public static Color Blue => new Color(0, 0, 255);
		public static Color White => new Color(255, 255, 255);
		public static Color Gray => new Color(127, 127, 127);
		public static Color Black => new Color(0, 0, 0);
	}
}
