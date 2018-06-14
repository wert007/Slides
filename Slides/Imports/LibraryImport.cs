using System;

namespace Slides
{
	public class LibraryImport : Import
	{
		public LibraryImport(string name, string alias) : base(name, alias)
		{
		}

		public override object Value => throw new NotImplementedException();
	}
}