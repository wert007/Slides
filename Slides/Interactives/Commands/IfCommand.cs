using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class IfCommand : Command
	{
		Command condition;
		Command body;

		public IfCommand(Command condition, Command body) : base(null, 2 + body.Lines)
		{
			this.condition = condition;
			if (condition.EditsVariables)
				Console.WriteLine("Warning: If-Condition changes Variables");
			this.body = body;
		}

		public static string Regex => "if\\(" + RegExHelper.Line + "\\):";
		public override bool EditsVariables => body.EditsVariables;

		public override int LineLength => 2 + body.LineLength;

		protected override object InnerRun(List<Variable> variables)
		{
			if (condition.Run(variables).ToString().Equals(true.ToString()))
				return body.Run(variables);
			return null;
		}

		public override string ToString()
		{
			return "if(" + condition + "):\n" + body + "endif\n";
		}
	}
}
