using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Slides.Interactives.Commands
{
	public class ValueCommand : Command
	{
		object value;

		public ValueCommand(object value) : base(value.GetType(), 1)
		{
			this.value = value;
			if (value is String str)
			{
				if (Regex.IsMatch(str, RegExHelper.String))
				{
					if (str.StartsWith("@"))
						str = str.Replace("\\", "\\\\").Substring(1);
					this.value = str.Trim('\'');
				}
				else if(int.TryParse(str, out int i))
					this.value = i;
				else if(float.TryParse(str, out float f))
					this.value = f;
			}
		}

		public static string RegEx => "(" + RegExHelper.Number + "|" + RegExHelper.String + "|" + RegExHelper.StyleValue + ")";
		public override bool EditsVariables => false;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			return value;
		}

		public override string ToString()
		{
			return value.ToString();
		}
	}
}
