using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class CommandCollection : Command
	{
		List<Command> commands;
		List<Variable> variables;

		public CommandCollection(params Command[] commands) : base(commands.Last().ReturnType, commands.Sum(c => c.Lines))
		{
			this.commands = new List<Command>();
			this.commands.AddRange(commands);
			variables = new List<Variable>();
		}

		public static string RegEx => RegExHelper.Anything + ";";

		public override bool EditsVariables => true;

		public override int LineLength => commands.Sum(c => c.LineLength);

		protected override object InnerRun(List<Variable> variables)
		{
			List<Variable> newVariables = new List<Variable>();
			newVariables.AddRange(variables);
			newVariables.AddRange(this.variables);
			for (int i = 0; i < commands.Count - 1; i++)
			{
				var res = commands[i].Run(newVariables);
				if (res != null)
				{
					if (res is Variable v)
					{
						newVariables.Add(v);
						this.variables.Add(v);
					}
				}
			}
			return commands.Last().Run(newVariables);
		}

		public override string ToString()
		{
			return string.Join("\n", commands);
		}
	}
}
