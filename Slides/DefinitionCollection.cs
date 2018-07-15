using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Slides
{
	public class DefinitionCollection
	{
		List<Definition> definitions;

		public static DefinitionCollection Create(CodeReader reader)
		{
			CodeReader r = reader.Copy();
			DefinitionCollection result = new DefinitionCollection();
			result.definitions = new List<Definition>();
			List<string> definitions = new List<string>();
			while(!r.Done)
			{
				string line = r.NextLine();
				if (line.StartsWith("def "))
					definitions.Add(line);
			}
			foreach (var def in definitions)
			{
				int mid = def.IndexOf(" as ");
				string patternIn = def.Remove(mid).Substring(4);
				string patternOut = def.Substring(mid + 4);
				Definition d = new Definition(patternIn, patternOut.TrimEnd(';'));
				result.definitions.Add(d);
			}
			return result;
}

		internal CodeReader CreateReader(CodeReader reader)
		{
			string text = "";
			CodeReader r = reader.Copy();
			while (!r.Done)
			{
				string line = r.NextLine();
				if (line.StartsWith("def "))
					continue;

				var matches = definitions.Where(d => d.IsMatch(line));
				foreach (var match in matches)
				{
					line = match.Apply(line);
				}
				text += line + "\n";
			}
			return CodeReader.FromText(text);
		}
	}

	public class Definition
	{
		Regex regexPatternInput;
		Regex regexPatternOutput;
		string patternInput;
		string patternOutput;
		string[] variables;


		public Definition(string patternIn, string patternOut)
		{
			patternInput = patternIn;
			patternOutput = patternOut;
			regexPatternInput = CreateRegex(patternIn);

			string tmpRegexPatternOutput = Regex.Escape(patternOut);
			foreach (var variable in variables)
				tmpRegexPatternOutput = tmpRegexPatternOutput.Replace(variable, RegExHelper.ValueOrVariableOrConstructor);
			regexPatternOutput = new Regex(tmpRegexPatternOutput);
		}

		private Regex CreateRegex(string pattern)
		{
			string result = pattern;
			result = Regex.Escape(result);
			Regex ptn = new Regex("(<" + RegExHelper.Variable + ">|>[a-zA-Z_])");
			List<string> variables = new List<string>();
			foreach(Capture match in ptn.Matches(result))
				variables.Add(match.Value.Trim('<', '>'));
			result = ptn.Replace(result, RegExHelper.ValueOrVariableOrConstructor);
			this.variables = variables.ToArray();
			return new Regex(result);
		}

		internal string Apply(string line)
		{
			string result = line;
			foreach (Capture match in regexPatternInput.Matches(line))
			{
				int origIndex = 0;
				int dif = 0;
				List<string> values = new List<string>();
				for(int i = 0; patternInput.Substring(origIndex).Contains('<') ||
									patternInput.Substring(origIndex).Contains('>'); i++)
				{

					int longIndex = patternInput.IndexOf('<', origIndex);
					int shortIndex = patternInput.IndexOf('>', origIndex);
					origIndex = longIndex;
					if (shortIndex >= 0 && (origIndex > shortIndex || origIndex < 0)) origIndex = shortIndex;
					int offset = origIndex + dif - (match.Index > 0 ? 1 : 0);
					var varMatch = Regex.Match(
						line.Substring(match.Index + offset, match.Length - offset),
						RegExHelper.ValueOrVariableOrConstructor);

					values.Add(varMatch.Value);
					origIndex += variables[i].Length + 2;
					dif += varMatch.Index + varMatch.Length - (variables[i].Length + 3);
				}
				string replace = patternOutput;
				for (int i = 0; i < variables.Length; i++)
				{
					replace = replace.Replace(variables[i], values[i]);
				}
				result= regexPatternInput.Replace(result, replace);
			}
			return result;

		}

		internal bool IsMatch(string line)
		{
			return regexPatternInput.IsMatch(line);
		}
	}
}