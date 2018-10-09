using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Imports
{
	public class GoogleFontImport : Import
	{
		public GoogleFontImport(string name, string alias) : base(name, alias)
		{
		}

		public override object Value => new GoogleFont(Name);
	}
}
