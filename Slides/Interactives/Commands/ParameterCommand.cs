using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class ParameterCommand : Command
	{
		List<Command> parameters;
		public Command[] Parameters => parameters.ToArray();

		public int Length => parameters.Count;

		public ParameterCommand(params Command[] parameters) : base(typeof(object[]), 1)
		{
			this.parameters = new List<Command>();
			this.parameters.AddRange(parameters);
			if (parameters.Any(p => p.EditsVariables))
				Console.WriteLine("Parameters edit Variables");
		}

		public override bool EditsVariables => parameters.Any(p => p.EditsVariables);

		public static string RegEx => @"\((" + RegExHelper.ValueOrConstructor + "(, " + RegExHelper.ValueOrConstructor + @")*)?\)";

		public override int LineLength => throw new NotImplementedException();

		protected override object InnerRun(List<Variable> variables)
		{
			List<object> result = new List<object>();
			foreach (var parameter in parameters)
			{
				result.Add(parameter.Run(variables));
			}
			return result.ToArray();
		}

		public override string ToString()
		{
			return string.Join(", ", parameters);
		}
	}
}
