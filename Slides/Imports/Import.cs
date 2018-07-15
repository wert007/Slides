using System;
using System.Text.RegularExpressions;

namespace Slides
{
	public abstract class Import : IScanable
	{
		public string Name { get; private set; }
		public string Alias { get; private set; }
		public abstract object Value { get; }

		public int LineLength => 1;

		public static string RegEx => "import (font|lib)\\(" + RegExHelper.File + "\\) as " + RegExHelper.Variable + ";";

		private Import() { }

		public Import(string name, string alias)
		{
			Name = name;
			Alias = alias;
		}

		public static bool TryScan(CodeReader reader, out Import import)
		{
			import = null;
			bool result = Import.TryScan(reader, out IScanable scanned);
			if (result)
				import = (Import)scanned;
			return result;
		}

		public static bool TryScan(CodeReader reader, out IScanable scanned)
		{
			string line = reader.NextLine();
			return TryScan(line, out scanned);
		}

		public static bool TryScan(string line, out IScanable scanned)
		{
			scanned = null;
			if (!Regex.IsMatch(line, RegEx))
				return false;
			string type = Regex.Match(line, "(font|lib)").Value;
			string name = Regex.Match(line, RegExHelper.File).Value;
			name = name.Trim('\'');
			string alias = Regex.Match(line, "as " + RegExHelper.Variable + ";").Value;
			alias = alias.Substring(3, alias.Length - 4);
			switch (type)
			{
				case "font":
					scanned = new FontImport(name, alias);
					break;
				case "lib":
					scanned = new LibraryImport(name, alias);
					break;
				default:
					throw new NotImplementedException("Unkown Type: " + type + " in Line '" + line + "'.");
			}
			return true;
		}
	}
}