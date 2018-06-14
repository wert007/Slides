using Slides.Interactives.Types;

namespace Slides
{
	public class FontImport : Import
	{
		public FontImport(string name, string alias): base(name, alias)
		{
		}

		public override object Value => new Font(Name);
	}
}