using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class FunctionCallCommand : AttributeCommand
	{
		string variable;
		string methodName;
		ParameterCommand parameters;

		public FunctionCallCommand(string variable, string method, ParameterCommand parameters) : base(typeof(object), 1)
		{
			this.variable = variable;
			this.methodName = method;
			this.parameters = parameters;
		}

		public static string RegEx => RegExHelper.FunctionCall;
		public override bool EditsVariables => true;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			var obj = variables.FirstOrDefault(v => v.Name == variable);
			var method = obj.Value.GetType().GetMethods().Where(m => m.Name.ToLower() == methodName.ToLower()).First();
			var compParameters = (object[])parameters.Run(variables);
			var result = method.Invoke(obj.Value, compParameters);
			if (methodName == "add")
				return compParameters[0];
			return result;
		}

		public override string ToString()
		{
			return variable + "." + methodName + "(" + parameters + ");\n";
		}
	}
}
