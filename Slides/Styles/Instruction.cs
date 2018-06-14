using Slides.Components;
using System;
using System.Linq;
using System.Collections.Generic;
using Slides.Interactives.Commands;
using System.Text.RegularExpressions;

namespace Slides.Styles
{
	public class Instruction : Command
	{
		public string Property { get; private set; }
		public StyleValue Value { get; private set; }

		public string RegEx => throw new NotImplementedException();

		public override bool EditsVariables => true;

		public override int LineLength => 1;

		public Instruction(string property, StyleValue value) : base(typeof(StyleValue), 1)
		{
			this.Property = property;
			this.Value = value;
		}
		
		public object Run(Element element)
		{
			//element.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == Property.ToLower()).SetValue(element, Value.Compute());
			return Value.Compute();
		}

		//internal object Compute(List<Variable> variables)
		//{
		//	var variable = variables.FirstOrDefault(v => v.Name == Property);
		//	variable.Value = Value.Compute();
		//	return variable;
		//}

		public new object Run(object parameter)
		{
			return Run((Element)parameter);
		}

		public static bool TryScan(CodeReader reader, out Instruction scanned)
		{
			string line = reader.NextLine().Trim(';');
			return TryScan(line, out scanned);
		}

		public static bool TryScan(string line, out Instruction scanned)
		{
			scanned = null;
			if (!Regex.IsMatch(line, RegExHelper.Variable + "(." + RegExHelper.Variable + @")?: ("
				+ RegExHelper.StyleConstants + "|" + RegExHelper.StyleFunction + "|" + RegExHelper.Value + ")"))
				//Actually too much, only string, number and stylevalue are accepted out of command.value
				return false;
			string property = Regex.Match(line, RegExHelper.Variable + "(." + RegExHelper.Variable + ")?").Value;
			if (Regex.IsMatch(line, RegExHelper.StyleFunction))
			{
				string name = line.Substring(property.Length + 1).Trim().Split('(')[0];
				List<string> parameters = new List<string>();
				foreach (var parameter in line.Substring(line.IndexOf(name) + name.Length).Trim('(', ')', ' ').Split(','))
				{
					parameters.Add(parameter.Trim().Trim('\''));
				}
				scanned = new Instruction(property, new StyleValueFunc(name, parameters.ToArray()));
			}
			else if (Regex.IsMatch(line, RegExHelper.StyleConstants))
			{
				string name = line.Substring(property.Length + 1).Trim().Trim(';');
				scanned = new Instruction(property, new StyleValueVariable(name));
			}
			else if (Regex.IsMatch(line, RegExHelper.StyleValue))
			{
				string instruct = line.Substring(property.Length + 1).Trim().Trim(';');
				string value = Regex.Match(instruct, RegExHelper.Number).Value;
				string unit = instruct.Substring(instruct.IndexOf(value) + value.Length);
				scanned = new Instruction(property, new StyleUnit(float.Parse(value), StyleUnit.FromString(unit)));
			}
			else if (Regex.IsMatch(line, RegExHelper.Value))
			{
				string value = line.Substring(property.Length + 1).Trim().Trim(';');
				scanned = new Instruction(property, new StyleCommandValue(value));
			}
			else
				throw new Exception("Unkwon Command in Line '" + line+ "'.");
			return true;
		}

		protected override object InnerRun(List<Variable> variables)
		{
			return Value.Compute();
		}
	}
}