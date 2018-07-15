using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	class SetArrayCommand : Command
	{
		Command value;
		int index;
		string array;

		public SetArrayCommand(string array, int index, Command value) : base(null, 1)
		{
			this.array = array;
			this.index = index;
			this.value = value;
		}

		public override bool EditsVariables => true;

		public static string RegEx => RegExHelper.Variable + "\\[" + RegExHelper.Integer + "\\]: " + RegExHelper.Command;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			var variable = variables.FirstOrDefault(v => v.Name == array);
			if(variable.Value is SlidesArray arr)
			{
				arr.SetAt(index, value.Run(variables));
			}
			return null;
		}
	}
}
