using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class ForCommand : Command
	{
		Variable variable;
		int min;
		int max;
		int increment;
		Command body;

		public ForCommand(string name, Command body, int min, int max,int increment) : base(null, 2 + body.Lines)
		{
			variable = new Variable(name, min);
			this.body = body;
			this.min = min;
			this.max = max;
			this.increment = increment;
		}

		public static string RegEx
			=> RegExHelper.Variable + ": for\\(" + RegExHelper.Integer + " to " + RegExHelper.Integer + " in " + RegExHelper.Integer + "\\):";
		public override bool EditsVariables => body.EditsVariables;

		public override int LineLength => body.LineLength + 2;

		protected override object InnerRun(List<Variable> variables)
		{
			var newVariables = new List<Variable>();
			newVariables.AddRange(variables);
			newVariables.Add(variable);
			for (int i = min; i < max; i+=increment)
			{
				variable.Value = i;
				body.Run(newVariables);
			}
			return null;
		}

		public static bool TryScan(CodeReader reader, out ForCommand command)
		{
			command = null;
			string line = reader.NextLine();
			if (!Regex.IsMatch(line, RegEx))
				return false;
			string variable = line.Split(':')[0];
			int min = int.Parse(line.Split('(', ' ')[2]);
			int max = int.Parse(Regex.Match(line, " to " + RegExHelper.Integer + " in ").Value.Split(new string[] { " to ", " in " }, StringSplitOptions.RemoveEmptyEntries)[0]);
			int increment = 1; //TODO
			List<Command> commands = new List<Command>();
			line = reader.NextLine();
			while (!reader.Done && line != "endfor")
			{
				if (TryScan(line, out Command cmd))
					commands.Add(cmd);
				else throw new ArgumentException();
				line = reader.NextLine();
			}
			command = new ForCommand(variable, new CommandCollection(commands.ToArray()), min, max, increment);
			return true;
		}

		public override string ToString()
		{
			return variable.Name + ": for(" + min + " to " + max + " in " + increment + "):\n" + body + "endfor\n";
		}
	}
}
