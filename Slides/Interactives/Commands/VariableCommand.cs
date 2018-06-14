using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class VariableCommand : Command
	{
		public string Name { get; private set; }

		public VariableCommand(string name) : base(typeof(object), 1)
		{
			this.Name = name;
		}


		public override bool EditsVariables => false;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			return variables.FirstOrDefault(v => v.Name == Name)?.Value;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
