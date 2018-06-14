using Slides.Components;
using Slides.Interactives.Commands;
using Slides.Interactives.Types;
using Slides.Patterns;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides
{
	public static class Parser
	{
		public static Presentation Parse(CodeReader reader)
		{
			Presentation presentation = new Presentation();
			while (!reader.Done)
			{
				var line = reader.NextLine();

				if (Import.TryScan(reader.Copy(), out Import import))
				{
					presentation.AddImport(import);
					reader.Skip(import.LineLength - 1);
				}
				else if (Style.TryScan(reader.Copy(), out Style style))
				{
					presentation.AddStyle(style);
					reader.Skip(style.LineLength - 1);
				}
				else if (Interactive.TryScan(reader.Copy(), out Interactive interactive))
				{
					presentation.AddInteractive(interactive);
					reader.Skip(interactive.LineLength - 1);
				}
				else if (Pattern.TryScan(reader.Copy(), out Pattern pattern))
				{
					presentation.AddPattern(pattern);
					reader.Skip(pattern.LineLength - 1);
				}
				else if (Slide.TryScan(reader.Copy(), presentation, out Slide slide))
				{
					presentation.AddSlide(slide);
					reader.Skip(slide.LineLength - 1);
				}
				else
					throw new Exception("Unknown Command in Line " + reader.CurrentLine + ".");

				Console.WriteLine(line);
			}
			return presentation;
		}

		public static string RemoveBrackets(this string str)
		{
			string res = str;
			if (res[0] == '(')
				res = res.Substring(1);
			if (res[res.Length - 1] == ')')
				res = res.Remove(res.Length - 1);
			return res;
		}
	}
}
