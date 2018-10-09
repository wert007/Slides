using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Styles
{
	public class StyleValueFunc : StyleValue
	{
		public string Name { get; private set; }

		public override string RegEx => RegExHelper.Anything; //TODO

		public string[] Parameters { get; private set; }

		public StyleValueFunc(string name, string[] parameters)
		{
			this.Name = name;
			this.Parameters = parameters;
		}

		public override object Compute()
		{
			switch (Name)
			{
				case "rgb":
					return FromRGB(Parameters);
				case "hsv":
					return FromHSV(Parameters);
				case "image":
					return StaticFunctions.LoadImage(Parameters); //TODO: Internparsing
				default:
					throw new ArgumentException("No function named " + Name + ".");
			}
		}

		public static Color FromRGB(params string[] parameters)
		{
			return new Color(parameters[0], parameters[1], parameters[2]);
		}

		public static Color FromRGB(byte r, byte g, byte b)
		{
			return new Color(r, g, b);
		}

		public static Color FromHSV(params string[] parameters)
		{
			return Color.Gray;
		}

		public override string ToString()
		{
			return Name + "(" + string.Join(", ", Parameters) + ")";
		}
	}
}
