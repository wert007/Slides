using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class ConstructorCommand : AttributeCommand
	{
		ParameterCommand parameters;

		public ConstructorCommand(Type returnType, ParameterCommand parameters) : base(returnType, 1)
		{
			this.parameters = parameters;
		}

		public static string RegEx => RegExHelper.ConstructorCall;

		public override bool EditsVariables => false;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			Type[] types = new Type[parameters.Length];
			var p = (object[])parameters.Run(variables);
			for (int i = 0; i < types.Length; i++)
			{
				types[i] = p[i].GetType();
			}
			return ReturnType.GetConstructor(types).Invoke(p);
		}

		public override string ToString()
		{
			return "new " + ReturnType.Name + "(" + parameters + ")";
		}
	}
}
