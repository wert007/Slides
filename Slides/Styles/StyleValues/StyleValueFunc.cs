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

		string[] parameters;

		public StyleValueFunc(string name, string[] parameters)
		{
			this.Name = name;
			this.parameters = parameters;
		}

		public override object Compute()
		{
			switch (Name)
			{
				case "rgb":
					return FromRGB(parameters);
				case "hsv":
					return FromHSV(parameters);
				case "image":
					return StaticFunctions.LoadImage(parameters); //TODO: Internparsing
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
	}
}
