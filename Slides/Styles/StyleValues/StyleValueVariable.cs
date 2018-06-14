using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Styles
{
	public class StyleValueVariable : StyleValue
	{
		public string Name { get; private set; }

		public override string RegEx =>
			"(red|green|blue|black|white|grey|gray)";

		public StyleValueVariable(string name)
		{
			Name = name;
		}

		public override object Compute()
		{
			switch (Name)
			{
				case "red":
					return Color.Red;
				case "green":
					return Color.Green;
				case "blue":
					return Color.Blue;
				case "black":
					return Color.Black;
				case "white":
					return Color.White;
				case "grey":
				case "gray":
					return Color.Gray;
				default:
					throw new NotImplementedException("No Const named " + Name + " known.");
			}
		}
	}
}
