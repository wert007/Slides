using Slides.Interactives.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides.Patterns
{
	public class Pattern : IScanable
	{
		public string Name { get; private set; }
		public int LineLength => commands.Sum(c => c.Lines) + 2;
		public Variable[] Variables { get; private set; }
		List<Command> commands;


		public static List<Pattern> patterns;
		public static string RegEx => "pattern " + RegExHelper.Variable + ":";
		

		public Pattern(string name)
		{
			Name = name;
			commands = new List<Command>();
			if (patterns == null)
				patterns = new List<Pattern>();
			patterns.Add(this);
		}

		public void AddCommand(Command command)
		{
			commands.Add(command);
		}

		public void Run()
		{
			List<Variable> variables = new List<Variable>();
			foreach (var command in commands)
			{
				var res = command.Run(variables);
				if (res != null)
				{
					if (res is Variable v)
					{
						if (variables.Contains(v))
							variables.Remove(v);
						variables.Add(v);
					}
				}
			}
			Variables = variables.ToArray();
		}

		public static Pattern GetByName(string name)
		{
			return patterns.FirstOrDefault(p => p.Name == name);
		}

		public static bool TryScan(CodeReader reader, out Pattern scanned)
		{
			scanned = null;
			string line = reader.NextLine();
			if (!Regex.IsMatch(line, RegEx))
				return false;
			string name = line.Substring("pattern ".Length).Trim(':');
			scanned = new Pattern(name);
			line = reader.NextLine();
			while (!reader.Done && !reader.EndingKeyword)
			{
				if (Command.TryScan(reader.Copy(), out Command command))
				{
					scanned.AddCommand(command);
					reader.Skip(command.Lines - 1);
					line = reader.NextLine();
				}
				else
					throw new Exception("Unknown Command in Line " + reader.TextLine + ".");
			}
			return true;
		}
	}
}