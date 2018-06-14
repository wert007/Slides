using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class GotoPositionCommand : Command
	{
		string name;

		public GotoPositionCommand(string name) : base(typeof(Command), 1)
		{
			this.name = name;
		}

		public override bool EditsVariables => throw new NotImplementedException();

		public override int LineLength => throw new NotImplementedException();

		protected override object InnerRun(List<Variable> variables)
		{
			return null;
		}

		public override string ToString()
		{
			return name;
		}
	}
}
