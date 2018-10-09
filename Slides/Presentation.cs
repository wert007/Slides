using Slides.Components;
using Slides.Patterns;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides
{
    public class Presentation
    {
		List<Slide> slides;
		List<Import> imports;
		List<Style> styles;
		List<Interactive> interactives;
		List<Pattern> patterns;

		public int Index;
		public int SlidesCount => slides.Count;
		public Style Standard { get; private set; }
		public IEnumerable<Import> Imports => imports;

		public static int Width => 1280;
		public static int Height => 720;

		public Presentation()
		{
			slides = new List<Slide>();
			imports = new List<Import>();
			styles = new List<Style>();
			interactives = new List<Interactive>();
			patterns = new List<Pattern>();
			Index = 0;
		}

		public void AddSlide(Slide slide)
		{
			slides.Add(slide);
		}

		public void AddInteractive(Interactive interactive)
		{
			interactives.Add(interactive);
		}

		public void AddImport(Import import)
		{
			imports.Add(import);
		}

		public void AddStyle(Style style)
		{
			if (style.Name == "std")
				Standard = style;
			else
				styles.Add(style);
		}

		public void AddPattern(Pattern pattern)
		{
			patterns.Add(pattern);
		}

		public void Run()
		{
			foreach (var slide in slides)
			{
				slide.CollectVariables(GetStandardVariables());
				Standard.Apply(slide);
				slide.ApplyStyles(GetStandardVariables());
				slide.Run(GetStandardVariables());
			}
		}

		public List<Variable> GetStandardVariables()
		{
			List<Variable> result = new List<Variable>();
			foreach (var import in imports)
			{
				result.Add(new Variable(import.Alias, import.Value));
			}
			return result;
		}

		public PresentationSession CreateSession()
		{
			return new PresentationSession(this, slides.ToArray());
		}
	}
}
