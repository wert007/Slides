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
			CodeReader r = reader.Copy();
			Presentation presentation = new Presentation();
			DefinitionCollection dc = DefinitionCollection.Create(r);
			r = dc.CreateReader(r);
			while (!r.Done)
			{
				var line = reader.NextLine();

				if (Import.TryScan(r.Copy(), out Import import))
				{
					presentation.AddImport(import);
					r.Skip(import.LineLength - 1);
				}
				else if (Style.TryScan(r.Copy(), out Style style))
				{
					presentation.AddStyle(style);
					r.Skip(style.LineLength - 1);
				}
				else if (Interactive.TryScan(r.Copy(), out Interactive interactive))
				{
					presentation.AddInteractive(interactive);
					r.Skip(interactive.LineLength - 1);
				}
				else if (Pattern.TryScan(r.Copy(), out Pattern pattern))
				{
					presentation.AddPattern(pattern);
					r.Skip(pattern.LineLength - 1);
				}
				else if (Slide.TryScan(r.Copy(), presentation, out Slide slide))
				{
					presentation.AddSlide(slide);
					r.Skip(slide.LineLength - 1);
				}
				else
					throw new Exception("Unknown Command in Line " + r.CurrentLine + ": " +r.PeekLine());

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
