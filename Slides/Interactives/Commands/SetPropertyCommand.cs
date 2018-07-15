using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class SetPropertyCommand : AttributeCommand
	{
		string variable;
		string property;
		Command value;

		public SetPropertyCommand(string variable, string property, Command value) : base(typeof(Variable), 1)
		{
			this.variable = variable;
			this.property = property;
			this.value = value;
		}

		public static string RegEx => RegExHelper.EnumOrProperty + ": " + RegExHelper.Command;
		public override bool EditsVariables => true;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			var obj = variables.FirstOrDefault(v => v.Name == variable);
			var prop = obj.Value.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == property.ToLower());
			prop.SetValue(obj.Value, value.Run(variables));
			return null; // obj;
		}

		public override string ToString()
		{
			return variable + "." + property + ": " + value + ";\n";
		}
	}
}
