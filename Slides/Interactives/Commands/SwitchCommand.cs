using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class SwitchCommand : Command
	{
		List<CaseCommand> cases;
		Command condition;

		public SwitchCommand(Command condition, params CaseCommand[] cases) : base(null, 2 + cases.Sum(c => c.Lines))
		{
			this.condition = condition;
			if (condition.EditsVariables)
				Console.WriteLine("Warning: Switch-Condition edits Variables");
			this.cases = new List<CaseCommand>();
			this.cases.AddRange(cases);
		}

		public static string Regex => "switch\\(" + RegExHelper.Line + "\\):";
		public override bool EditsVariables => cases.Any(c => c.EditsVariables);

		public override int LineLength => 2 + cases.Sum(c => c.LineLength);

		protected override object InnerRun(List<Variable> variables)
		{
			var value = condition.Run(variables);
			foreach (var c in cases)
				if (c.GetCondition(variables).ToString().Equals(value.ToString()))
					return c.Run(variables);
			return null;
		}

		public override string ToString()
		{
			return "switch(" + condition + "):\n" + string.Join("\n", cases) + "endswitch\n";
		}
	}
}
