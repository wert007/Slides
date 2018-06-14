using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class LoopCommand : Command
	{
		LoopingCommand loop;

		public LoopCommand(Command cmd, int fps, int times) : base(null, 1)
		{
			loop = new LoopingCommand(cmd, fps, times);
		}

		public static string RegEx 
			=> "loop\\((" + RegExHelper.Integer + "fps|" + RegExHelper.Integer + "(" + RegExHelper.Time + ")?" + "|" + RegExHelper.Integer + "fps, " + RegExHelper.Integer + "(" + RegExHelper.Time + ")?" + ")?\\): " + RegExHelper.Line;
		public override bool EditsVariables => loop.Command.EditsVariables;

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			return loop;
		}

		public override string ToString()
		{
			return "loop(" + loop.FPS + "fps, " + loop.Time + "): " + loop.Command + "";
		}
	}

	public struct LoopingCommand
	{
		public Command Command { get; private set; }
		public int FPS { get; private set; }
		public int Time { get; set; }

		public LoopingCommand(Command cmd, int fps, int time)
		{
			Command = cmd;
			FPS = fps;
			Time = time;
		}

		public bool Run(List<Variable> variables)
		{
			if (Time != 0)
			{
				Time--;
				Command.Run(variables);
				return true;
			}
			else return false;
		}
	}
}
