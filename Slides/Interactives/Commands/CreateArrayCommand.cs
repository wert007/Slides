using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slides.Interactives.Types;

namespace Slides.Interactives.Commands
{
	class CreateArrayCommand : Command
	{
		SlidesArray array;

		public CreateArrayCommand(int length) : base(null, 1)
		{
			array = new SlidesArray(length);
		}

		public override bool EditsVariables => false;

		public static string RegEx => "\\[" + RegExHelper.Integer + "\\]";

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			return array;
		}
	}
}
