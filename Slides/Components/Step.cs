using Slides.Interactives.Commands;
using Slides.Interactives.Types;
using Slides.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides.Components
{
	public class Step : Element, IScanable
	{
		public int LineLength => (Name == "normal" ? 0 : 1) + commands.Sum(c => c.Lines);
		protected List<Command> commands;
		protected List<LoopingCommand> loops;
		protected List<Variable> slideVariables;
		public IEnumerable<Variable> Variables => slideVariables;
		
		public IEnumerable<LoopingCommand> Loops => loops;
		public Slide Parent { get; private set; }
		public CurrentTime Time { get; private set; }

		public Step(string name, Slide parent) : base (name)
		{
			Parent = parent;
			commands = new List<Command>();
			//children = new List<Element>();
			loops = new List<LoopingCommand>();
			slideVariables = new List<Variable>();
			Time = new CurrentTime();
		}

		public void Show()
		{
		}

		public void Add(Command command)
		{
			commands.Add(command);
		}

		public void ApplyStyles(List<Variable> variables)
		{
			variables.AddRange(slideVariables);
			foreach (var cmd in commands.Where(c => c is ApplyStyleCommand))
			{
				var res = cmd.Run(variables);
				if (res != null)
				{
					if (res is Variable v)
					{
						bool isOld = variables.Contains(v);
						if (isOld)
						{
							variables.Remove(v);
							slideVariables.Remove(v);
						}
						variables.Add(v);
						slideVariables.Add(v);
					}
				}
			}
		}

		public void CollectVariables(List<Variable> variables)
		{
			foreach (var cmd in commands.Where(c => c.EditsVariables))
			{
				var res = cmd.Run(variables);
				if (res != null)
				{
					if (res is Variable v)
					{
						bool isOld = variables.Contains(v);
						if (isOld)
						{
							variables.Remove(v);
							slideVariables.Remove(v);
						}
						variables.Add(v);
						slideVariables.Add(v);
					}
					else if (res is Pattern p)
					{
						p.Run();
						var newVariables = p.Variables;
						foreach (var var in newVariables)
						{
							variables.Remove(var);
							variables.Add(var);
							slideVariables.Remove(var);
							slideVariables.Add(var);
						}
					}
				}
			}
		}

		public void Run(List<Variable> variables)
		{
			foreach (var command in commands)
			{
				var res = command.Run(variables);
				if (res != null)
				{
					if (res is Variable v)
					{
						bool isOld = slideVariables.Contains(v) || variables.Contains(v);
						if (isOld)
						{
							variables.Remove(v);
						}
						variables.Add(v);
					}
					else
					if (res is LoopingCommand lc)
					{
						if (loops.Contains(lc))
							loops.Remove(lc);
						loops.Add(lc);
					}
				}
			}
		}

		public List<Element> GetElements()
		{
			return slideVariables.Where(v => v.Value is Element).Select((v,e) => (Element)v.Value).ToList();
		}

		public static bool TryScan(CodeReader reader, Slide parent, out Step scanned)
		{
			scanned = null;
			string line = reader.NextLine();
			if (!Regex.IsMatch(line, RegEx))
				return false;
			string name = Regex.Match(line, RegExHelper.String).Value.Trim('\'');
			scanned = new Step(name, parent);
			line = reader.NextLine();

			while (!reader.Done && !reader.EndingKeyword && !Regex.IsMatch(line, RegEx))
			{
				if (String.IsNullOrWhiteSpace(line))
				{
					line = reader.NextLine();
					continue;
				}
				if (Command.TryScan(reader.Copy(), out Command command))
				{
					reader.Skip(command.Lines - 1);
					scanned.Add(command);
				}
				else if (StaticFunctionCallCommand.TryScan(line, out StaticFunctionCallCommand functionCallCommand))
				{
					reader.Skip(functionCallCommand.Lines - 1);
					scanned.Add(functionCallCommand);
				}
				else
					throw new ArgumentException("Unknwon Command in Line " + reader.TextLine + ".");
				line = reader.NextLine();
			}
			return true;
		}
		
		public static string RegEx => "step( " + RegExHelper.String + ")?:";
	}
}
