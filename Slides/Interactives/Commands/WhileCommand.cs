using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class WhileCommand : Command
	{
		Command condition;
		Command body;

		public WhileCommand(Command condition, Command body) : base(null, 2 + body.Lines)
		{
			if (condition.EditsVariables)
				Console.WriteLine("Warning: While-Condition edits Variables");
			this.condition = condition;
			this.body = body;
		}

		public static string Regex => "while\\(" + RegExHelper.Command + "\\):";
		public override bool EditsVariables => body.EditsVariables;

		public override int LineLength => 2 + body.LineLength;

		protected override object InnerRun(List<Variable> variables)
		{
			while (condition.Run(variables).ToString().Equals(true.ToString()))
				body.Run(variables);
			return null;
		}

		public override string ToString()
		{
			return "while(" + condition + "):\n" + body + "endwhile\n";
		}
	}
}
