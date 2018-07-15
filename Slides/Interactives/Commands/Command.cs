using Slides.Interactives.Types;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public abstract class Command : IRunable, IScanable
	{
		public Type ReturnType { get; private set; }
		public int Lines { get; private set; }
		public object Result { get; private set; }

		public Command(Type returnType, int lines)
		{
			ReturnType = returnType;
			Lines = lines;
		}

		public abstract bool EditsVariables { get; }
		public abstract int LineLength { get; }

		protected abstract object InnerRun(List<Variable> variables);
		public object Run(object parameter)
		{
			Result = InnerRun((List<Variable>)parameter);
			return Result;
		}
		public object Run(List<Variable> variables)
		{
			Result = InnerRun(variables);
			return Result;
		}
		public object SingleRun(List<Variable> variables)
		{
			if (Result == null)
				Result = InnerRun(variables);
			return Result;
		}

		public static string RegExWholeLine(string expression) => "^" + expression + "$";

		public static bool TryScan(CodeReader reader, out Command scanned)
		{
			scanned = null;
			string line = reader.NextLine().Trim(';');

			if (Regex.IsMatch(line, LoopCommand.RegEx))
			{
				string loop = line.Split(':')[0];
				string fpsStr = Regex.Match(loop, RegExHelper.Integer + "fps").Value;
				if (!int.TryParse(fpsStr.Trim('f', 'p', 's'), out int fps))
					fps = 30;
				int times = -1;
				string timesStr = Regex.Match(loop, RegExHelper.Integer + "(" + RegExHelper.Time + ")?").Value;
				if (!int.TryParse(Regex.Match(timesStr, RegExHelper.Integer).Value, out times))
					times = -1;
				if (TryScan(line.Substring(line.IndexOf(':') + 1), out Command cmd))
				{
					scanned = new LoopCommand(cmd, fps, times);
					return true;
				}
			}
			if (Regex.IsMatch(line, FunctionCallCommand.RegEx))
			{
				string obj = line.Split('.')[0];
				string method = line.Split('.', '(')[1];
				string parameters = line.Substring(line.IndexOf(method) + method.Length);
				if (TryScan(parameters, out Command cmd))
				{
					scanned = new FunctionCallCommand(obj, method, (ParameterCommand)cmd);
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(SetPropertyCommand.RegEx)))
			{
				string obj = line.Split('.')[0];
				string property = line.Split('.', ':')[1];
				string value = line.Substring(line.IndexOf(property) + property.Length + 1);
				if (TryScan(value, out Command cmd))
				{
					scanned = new SetPropertyCommand(obj, property, cmd);
					return true;
				}
			}
			if (Regex.IsMatch(line, Command.RegExWholeLine(AssignmentCommand.RegEx)))
			{
				if (AssignmentCommand.TryScan(reader.Copy(), out AssignmentCommand cmd))
				{
					scanned = cmd;
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(SetArrayCommand.RegEx)))
			{
				string variable = line.Split('[')[0];
				string number = line.Split('[', ']')[1];
				string command = string.Join(":", line.Split(':').Skip(1)).Trim();
				if (Command.TryScan(command, out Command val))
				{
					if (int.TryParse(number, out int index))
					{
						scanned = new SetArrayCommand(variable, index, val);
						return true;
					}
				}
			}
			if (Regex.IsMatch(line, ForCommand.RegEx))
			{
				if (ForCommand.TryScan(reader.Copy(), out ForCommand cmd))
				{
					scanned = cmd;
					return true;
				}
			}
			if (Regex.IsMatch(line, GotoCommand.RegEx))
				throw new NotImplementedException();
			if (Regex.IsMatch(line, IfCommand.Regex))
			{
				if (TryScan(line.Split('(', ')')[1], out Command condition))
				{
					List<Command> body = new List<Command>();
					while (line != "endif" && !reader.Done)
					{
						line = reader.NextLine();
						if (TryScan(reader.Copy(), out Command cmd))
							body.Add(cmd);
						else
							throw new Exception("Unknown Command at Line " + reader.TextLine + ".");
						line = reader.NextLine().Trim();
					}
					scanned = new IfCommand(condition, new CommandCollection(body.ToArray()));
					return true;
				}
			}
			if (Regex.IsMatch(line, SwitchCommand.Regex))
			{
				if (TryScan(line.Split('(', ')')[1], out Command condition))
				{
					List<CaseCommand> body = new List<CaseCommand>();
					while (line != "endswitch" && !reader.Done)
					{
						if (Regex.IsMatch(line, CaseCommand.RegEx))
						{
							if (CaseCommand.TryScan(reader.Copy(), out CaseCommand cmd))
								body.Add(cmd);
							else
								throw new Exception("Unknown Command at Line " + reader.TextLine + ".");
						}
						line = reader.NextLine().Trim();
					}
					scanned = new SwitchCommand(condition, body.ToArray());
					return true;
				}
			}
			if (Regex.IsMatch(line, WhileCommand.Regex))
				throw new NotImplementedException();
			if (Regex.IsMatch(line, RegExWholeLine(StyleCommand.RegEx)))
			{
				if (StyleValue.TryScan(line, out StyleValue styleValue))
				{
					scanned = new StyleCommand(styleValue);
					return true;
				}
			}
			if (Regex.IsMatch(line, ApplyStyleCommand.RegEx))
			{
				if (line.Count(c => c == '.') > 0)
				{
					Console.WriteLine("ApplyStyleCommand: " + line);
					string lastHalf = line.Split('.').Last();
					string firstHalf = line.Remove(line.Length - lastHalf.Length - 1);
					string name = lastHalf.Trim('(', ')');
					if (TryScan(firstHalf, out Command cmd))
					{
						scanned = new ApplyStyleCommand(Style.GetByName(name), cmd);
						return true;
					}
				}
				else
				{
					Console.WriteLine("ApplyStyleCommand: " + line);
				}
			}
			//if (Regex.IsMatch(line, ValueCommand.RegEx))
			//	throw new NotImplementedException();
			//if (Regex.IsMatch(line, CommandCollection.Regex))
			//	throw new NotImplementedException();
			return false;
		}

		public static bool TryScan(string line, out Command scanned)
		{
			scanned = null;
			line = line.Trim().TrimEnd(';');
			if (Regex.IsMatch(line, RegExWholeLine(AssignmentCommand.RegEx)))
			{
				if (AssignmentCommand.TryScan(CodeReader.FromText(line), out AssignmentCommand cmd))
				{
					scanned = cmd;
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(EnumCommand.RegEx)) ||
				Regex.IsMatch(line, RegExWholeLine(GetPropertyCommand.RegEx)))
			{
				string firstPart = line.Split('.')[0];
				string secondPart = line.Split('.')[1];
				Type enumType = Assembly.GetAssembly(typeof(NoneType)).GetTypes().FirstOrDefault(t => t.Name == firstPart);
				if (enumType != null)
				{
					scanned = new EnumCommand(enumType, secondPart);
					return true;
				}
				else
				{
					scanned = new GetPropertyCommand(firstPart, secondPart);
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(SetPropertyCommand.RegEx)))
			{
				string obj = line.Split('.')[0];
				string property = line.Split('.', ':')[1];
				string value = line.Substring(line.IndexOf(property) + property.Length + 1);
				if (TryScan(value, out Command cmd))
				{
					scanned = new SetPropertyCommand(obj, property, cmd);
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(ConstructorCommand.RegEx)))
			{
				string type = line.Split(' ', '(')[1];
				if (TryScan(line.Substring(line.IndexOf(type) + type.Length), out Command cmd))
				{
					var t = Assembly.GetAssembly(typeof(NoneType)).GetTypes().FirstOrDefault(ty => ty.Name == type);
					scanned = new ConstructorCommand(t, (ParameterCommand)cmd);
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(ParameterCommand.RegEx)))
			{
				List<Command> parameters = new List<Command>();
				foreach (var parameter in line.RemoveBrackets().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					if (TryScan(parameter.Trim(), out Command cmd))
						parameters.Add(cmd);
					else throw new Exception("Some Error occured while passing parameters.");
				}
				scanned = new ParameterCommand(parameters.ToArray());
				return true;
			}
			if (Regex.IsMatch(line, Command.RegExWholeLine(OperationalCommand.Regex)))
			{
				for (int i = 0; i < line.Length; i++)
				{
					switch (line[i])
					{
						case '+':
							{
								if (TryScan(line.Remove(i), out Command left) && TryScan(line.Substring(i + 1), out Command right))
								{
									scanned = new OperationalCommand(Operation.Plus, left, right);
									return true;
								}
								break;
							}
						case '-':
							{
								if (TryScan(line.Remove(i), out Command left) && TryScan(line.Substring(i + 1), out Command right))
								{
									scanned = new OperationalCommand(Operation.Minus, left, right);
									return true;
								}
								break;
							}
						case '*':
							{
								if (TryScan(line.Remove(i), out Command left) && TryScan(line.Substring(i + 1), out Command right))
								{
									scanned = new OperationalCommand(Operation.Multiply, left, right);
									return true;
								}
								break;
							}
						case '/':
							{
								if (TryScan(line.Remove(i), out Command left) && TryScan(line.Substring(i + 1), out Command right))
								{
									scanned = new OperationalCommand(Operation.Divide, left, right);
									return true;
								}
								break;
							}
						case '%':
							{
								if (TryScan(line.Remove(i), out Command left) && TryScan(line.Substring(i + 1), out Command right))
								{
									scanned = new OperationalCommand(Operation.Modulo, left, right);
									return true;
								}
								break;
							}
						case '^':
							{
								if (TryScan(line.Remove(i), out Command left) && TryScan(line.Substring(i + 1), out Command right))
								{
									scanned = new OperationalCommand(Operation.Pow, left, right);
									return true;
								}
								break;
							}
						default:
							break;
					}
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(FunctionCallCommand.RegEx)))
			{
				string obj = line.Split('.')[0];
				string method = line.Split('.', '(')[1];
				string parameters = line.Substring(line.IndexOf(method) + method.Length);
				if (Command.TryScan(parameters, out Command cmd))
				{
					scanned = new FunctionCallCommand(obj, method, (ParameterCommand)cmd);
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(StyleCommand.RegEx)))
			{
				if (StyleValue.TryScan(line, out StyleValue styleValue))
				{
					scanned = new StyleCommand(styleValue);
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(StaticFunctionCallCommand.RegEx)))
			{
				if (StaticFunctionCallCommand.TryScan(line, out StaticFunctionCallCommand cmd))
				{
					scanned = cmd;
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(ValueCommand.RegEx)))
			{
				scanned = new ValueCommand(line);
				return true;
			}
			if (Regex.IsMatch(line, RegExWholeLine(CreateArrayCommand.RegEx)))
			{
				line = line.Trim('[', ']');
				if(int.TryParse(line, out int length))
				{
					scanned = new CreateArrayCommand(length);
					return true;
				}
			}
			if (Regex.IsMatch(line, RegExWholeLine(RegExHelper.Variable)))
			{
				scanned = new VariableCommand(line);
				return true;
			}
			return false;
		}
	}
}
