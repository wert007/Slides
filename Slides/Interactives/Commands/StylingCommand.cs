using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public abstract class StylingCommand : Command
	{
		public StylingCommand(Type returnType, int lines) : base(returnType, lines)
		{
		}
	}
}
