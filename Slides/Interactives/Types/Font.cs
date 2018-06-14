using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Types
{
	public class Font
	{
		public string Name { get; private set; }
		public Generic Type { get; private set; }

		public Font(string name, Generic type = Generic.SansSerif)
		{
			Name = name;
			Type = type;
		}

		public enum Generic
		{
			Serif,
			SansSerif,
			Cursive,
			Monospace,
			Fantasy
		}

		public string GetCSSGeneric()
		{
			switch (Type)
			{
				case Generic.Serif:
					return "serif";
				case Generic.SansSerif:
					return "sans-serif";
				case Generic.Cursive:
					return "cursive";
				case Generic.Monospace:
					return "monospace";
				case Generic.Fantasy:
					return "fantasy";
				default:
					throw new NotImplementedException();
			}
		}
	}
}
