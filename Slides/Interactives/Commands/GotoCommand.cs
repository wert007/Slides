using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class GotoCommand : Command
	{
		GotoPositionCommand position;

		public GotoCommand(GotoPositionCommand position) : base(typeof(GotoPositionCommand), 1)
		{
			this.position = position;
		}

		public static string RegEx => "goto " + RegExHelper.Variable;
		public override bool EditsVariables => position.EditsVariables;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			return position;
		}

		public override string ToString()
		{
			return "goto " + position;
		}
	}
}
