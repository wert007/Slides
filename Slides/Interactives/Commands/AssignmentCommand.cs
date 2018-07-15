using Slides.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class AssignmentCommand : AttributeCommand
	{
		string variableName;
		Command value;

		public AssignmentCommand(string name, Command value, Type returnType) : base(returnType, 1)
		{
			this.variableName = name;
			this.value = value;
		}

		public static string RegEx => RegExHelper.Variable + ": " + RegExHelper.Command;
		public override bool EditsVariables => true;

		public override int LineLength => 1;
		
		protected override object InnerRun(List<Variable> variables)
		{
			var varValue = value.Run(variables);
			Element.SetName(varValue, variableName);
			return new Variable(variableName, varValue);
		}

		public override string ToString()
		{
			return variableName + ": " + value + ";\n";
		}

		public static bool TryScan(CodeReader reader, out AssignmentCommand command)
		{
			command = null;
			string line = reader.NextLine().Trim(';');
			if (!Regex.IsMatch(line, RegEx))
				return false;
			string name = line.Split(':')[0];
			string value = String.Join(":", line.Split(':').Skip(1));
			if (TryScan(value, out Command constructor))
			{
				command = new AssignmentCommand(name, constructor, constructor.ReturnType);
				return true;
			}
			return false;
		}
	}
}
