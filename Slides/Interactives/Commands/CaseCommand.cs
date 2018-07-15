using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Slides.Interactives.Commands
{
	public class CaseCommand : Command
	{
		Command condition;
		Command body;

		public CaseCommand(Command condition, Command body) : base(null, 1 + body.Lines)
		{
			this.condition = condition;
			if (condition.EditsVariables)
				Console.WriteLine("Warning: Case Condition changes Variables");
			this.body = body;
		}

		public override bool EditsVariables => body.EditsVariables;

		public static string RegEx => "case " + RegExHelper.Command + ":";

		public override int LineLength => condition.LineLength + body.LineLength;

		public object GetCondition(List<Variable> variables)
		{
			return condition.Run(variables);
		}

		protected override object InnerRun(List<Variable> variables)
		{
			return body.Run(variables);
		}

		public override string ToString()
		{
			return "case " + condition + ":\n" + body.ToString();
		}

		public static bool TryScan(CodeReader reader, out CaseCommand command)
		{
			command = null;
			string line = reader.NextLine().Trim('\r');
			if (!Regex.IsMatch(line, RegEx))
				return false;
			if (TryScan(line.Remove(line.Length - 1).Substring("case ".Length), out Command condition))
			{
				line = reader.NextLine().Trim('\r');
				List<Command> body = new List<Command>();
				while (line != "endswitch" && !Regex.IsMatch(line, RegEx) && !reader.Done)
				{
					if (TryScan(line, out Command cmd))
					{
						body.Add(cmd);
					}
					else
						throw new Exception("Unexpected Command in switch-case!");
					line = reader.NextLine().Trim('\r');
				}
				command = new CaseCommand(condition, new CommandCollection(body.ToArray()));
				return true;
			}
			return false;
		}
	}
}