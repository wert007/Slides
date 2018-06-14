using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class EnumCommand : Command
	{
		string enumeration;

		public EnumCommand(Type enumType, string enumeration) : base(enumType, 1)
		{
			this.enumeration = enumeration;
		}

		public static string RegEx => RegExHelper.EnumOrProperty;
		public override bool EditsVariables => false;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			return Enum.Parse(ReturnType, enumeration);
		}

		public override string ToString()
		{
			return ReturnType.Name + "." + enumeration;
		}
	}
}
