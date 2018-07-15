using Slides.Components;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class ApplyStyleCommand : AttributeCommand
	{
		Style style;
		Command element;

		public ApplyStyleCommand(Style style, Command element) : base(null, 1)
		{
			this.style = style;
			this.element = element;
		}

		public override bool EditsVariables => true;
		public static string RegEx => RegExHelper.Variable + "." + RegExHelper.Variable + "\\(\\)";

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			style.Apply((Element)(element.SingleRun(variables)));
			return null;
		}
	}
}
