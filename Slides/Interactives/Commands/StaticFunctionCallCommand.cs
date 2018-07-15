using Slides.Components;
using Slides.Interactives.Commands;
using Slides.Interactives.Types;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class StaticFunctionCallCommand : StylingCommand
	{
		string name;
		ParameterCommand parameters;

		public StaticFunctionCallCommand(string name, ParameterCommand parameters) : base(typeof(object), 1)
		{
			this.name = name;
			this.parameters = parameters;
		}

		public static string RegEx => RegExHelper.FunctionCallHeader;
		public override bool EditsVariables
		{
			get
			{
				switch(name)
				{
					case "center":
					case "margin":
					case "padding":
					case "leftHalf":
					case "topLeftQuarter":
					case "leftTopQuarter":
					case "bottomLeftQuarter":
					case "leftBottomQuarter":
					case "rightHalf":
					case "topRightQuarter":
					case "rightTopQuarter":
					case "bottomRightQuarter":
					case "rightBottomQuarter":
					case "topHalf":
					case "bottomHalf":
					case "resetMargin":
					case "pattern":
						return true;
					case "image":
					case "youtube":
					case "noPattern":
						return false;
					default:
						Console.WriteLine("Unknown command " + this);
						return true;
				}
			}
		}

		public override int LineLength => 1;

		protected override object InnerRun(List<Variable> variables)
		{
			Element element = null;
			object[] parameterValues = (object[])parameters.Run(variables);
			switch (name)
			{
				case "center":
					StaticFunctions.Center(parameterValues);
					return null;
				case "margin":
					StaticFunctions.Margin(parameterValues);
					return null;
				case "padding":
					StaticFunctions.Padding(parameterValues);
					return null;
				case "leftHalf":
					element = (Element)parameterValues[0];
					element.Margin += new Thickness("0px", "50%", "0px", "0px");
					return null;
				case "topLeftQuarter":
				case "leftTopQuarter":
					element = (Element)parameterValues[0];
					element.Margin += new Thickness("0px", "50%", "50%", "0px");
					return null;
				case "bottomLeftQuarter":
				case "leftBottomQuarter":
					element = (Element)parameterValues[0];
					element.Margin += new Thickness("50%", "50%", "0px", "0px");
					return null;
				case "rightHalf":
					element = (Element)parameterValues[0];
					element.Margin += new Thickness("0px", "0px", "0px", "50%");
					return null;
				case "topRightQuarter":
				case "rightTopQuarter":
					element = (Element)parameterValues[0];
					element.Margin += new Thickness("0px", "0px", "50%", "50%");
					return null;
				case "bottomRightQuarter":
				case "rightBottomQuarter":
					element = (Element)parameterValues[0];
					element.Margin += new Thickness("50%", "0px", "0px", "50%");
					return null;
				case "topHalf":
					element = (Element)parameterValues[0];
					element.Margin += new Thickness("0px", "0px", "50%", "0px");
					return null;
				case "bottomHalf":
					element = (Element)parameterValues[0];
					element.Margin += new Thickness("50%", "0px", "0px", "0px");
					return null;
				case "resetMargin":
					StaticFunctions.ResetMargin(parameterValues);
					return null;
				case "image":
					return StaticFunctions.LoadImage(parameterValues);
				case "youtube":
					return StaticFunctions.LoadYoutubeVideo(parameterValues);
				case "pattern":
					return Patterns.Pattern.GetByName(((VariableCommand)parameters.Parameters[0]).Name);
				case "noPattern":
					return null;
				default:
					throw new ArgumentException("No static function named " + name + ".");
			}
		}

		public static bool TryScan(string line, out StaticFunctionCallCommand command)
		{
			command = null;
			line = line.Trim().Trim(';');
			if (!Regex.IsMatch(line, RegExWholeLine(RegEx)))
				return false;
			string name = line.Split('(')[0];
			string parameters = line.Substring(line.IndexOf(name) + name.Length);
			if (TryScan(parameters, out Command cmd))
			{
				command = new StaticFunctionCallCommand(name, (ParameterCommand)cmd);
				return true;
			}
			return false;
		}
	}
}
