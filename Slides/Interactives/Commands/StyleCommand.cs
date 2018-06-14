using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class StyleCommand : Command
	{
		StyleValue instruction;

		public StyleCommand(StyleValue instruction) : base(null, 1)
		{
			this.instruction = instruction;
		}

		public static string RegEx =>// Style.Variable + "(." + Style.Variable + @")?: " + 
			"(" + RegExHelper.StyleConstants + "|" + RegExHelper.StyleFunction + "|" + RegExHelper.StyleValue + ")";
		public override bool EditsVariables => true;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			return instruction.Compute();
		}

		public override string ToString()
		{
			return instruction.ToString();
		}
	}
}
/*
 * 		public string Name { get; private set; }
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
				default:
					throw new ArgumentException("No function named " + Name + ".");
			}
		}

		public static Color FromRGB(params string[] parameters)
		{
			return Color.White;
		}

		public static Color FromHSV(params string[] parameters)
		{
			return Color.Gray;
		}
 * */
