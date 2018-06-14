using Slides.Interactives.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides
{
	public class Interactive: IScanable
	{
		public string Name { get; private set; }
		public int LineLength => commands.Sum(c => c.Lines) + 2;

		public static string RegEx => @"interactive (transistion|" + RegExHelper.Function + "):";

		public List<Command> commands;
		public List<string> parameters;

		public Interactive(string name, params string[] parameters)
		{
			Name = name;
			commands = new List<Command>();
			this.parameters = new List<string>
			{
				"this"
			};
			this.parameters.AddRange(parameters);
		}

		public void AddCommand(Command command)
		{
			commands.Add(command);
		}

		public void Run(params object[] parameterValues)
		{
			List<Variable> variables = new List<Variable>();
			for (int i = 0; i < parameters.Count; i++)
			{
				variables.Add(new Variable(parameters[i].Trim(), parameterValues[i]));
			}
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
					else if(res is Command c)
					{
						throw new NotImplementedException();
					}
				}
			}
		}

		public static bool TryScan(CodeReader reader, out Interactive scanned)
		{
			scanned = null;
			string line = reader.NextLine();
			if (!Regex.IsMatch(line, RegEx))
				return false;
			string name = Regex.Match(line, @"(transistion:|" + RegExHelper.Variable + @"\()").Value;
			name = name.Remove(name.Length - 1);
			string parameters = Regex.Match(line, "\\(" + RegExHelper.FunctionParameters + "\\)").Value.Trim('(', ')');

			scanned = new Interactive(name, String.IsNullOrWhiteSpace(parameters) ? new string[0] : parameters.Split(','));
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
