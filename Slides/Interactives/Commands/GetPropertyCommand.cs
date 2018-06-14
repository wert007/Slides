using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class GetPropertyCommand : Command
	{
		string variable;
		string property;

		public GetPropertyCommand(string variable, string property) : base(typeof(string), 1)
		{
			this.variable = variable;
			this.property = property;
		}

		public static string RegEx => RegExHelper.EnumOrProperty;
		public override bool EditsVariables => false;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			var obj = variables.FirstOrDefault(v => v.Name == variable);
			return obj.Value.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == property.ToLower()).GetValue(obj.Value);
		}

		public override string ToString()
		{
			return variable + "." + property;
		}
	}
}
