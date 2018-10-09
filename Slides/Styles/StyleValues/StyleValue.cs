using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Slides.Styles
{
	public abstract class StyleValue : IScanable
	{
		public int LineLength => 1;
		public abstract string RegEx { get; }

		public abstract object Compute();

		public static bool TryScan(string line, out StyleValue value)
		{
			value = null;
			line = line.Trim().Trim(';');
			if (!Regex.IsMatch(line, "(" + RegExHelper.StyleConstants + "|" + RegExHelper.StyleFunction + "|" + RegExHelper.StyleValue + ")"))
				return false;
			if (Regex.IsMatch(line, RegExHelper.StyleFunction))
			{
				string name = line.Split('(')[0];
				List<string> parameters = new List<string>();
				foreach (var parameter in line.Substring(line.IndexOf(name) + name.Length).Trim('(', ')', ' ').Split(','))
				{
					parameters.Add(parameter.Trim().Trim('\''));
				}
				value = new StyleValueFunc(name, parameters.ToArray());
			}
			else if (Regex.IsMatch(line, RegExHelper.StyleConstants))
			{
				value = new StyleValueConstant(line);
			}
			else if (Regex.IsMatch(line, RegExHelper.StyleValue))
			{
				string valueStr = Regex.Match(line, RegExHelper.Number).Value;
				string unit = line.Substring(line.IndexOf(valueStr) + valueStr.Length);
				value = new StyleUnit(float.Parse(valueStr), StyleUnit.FromString(unit));

			}
			else
				return false;
			return true;
		}
	}
}