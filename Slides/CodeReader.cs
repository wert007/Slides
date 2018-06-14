using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides
{
    public class CodeReader
    {
		private FileSystemWatcher fileSystemWatcher;
		private string line;

		public int TextLine { get; private set; }
		public int StartLine { get; private set; }
		public int CurrentLine => StartLine + TextLine;
		private string[] Text { get; set; }
		public bool Done => TextLine == Text.Length;
		public bool EndingKeyword { get
			{
				return line == "endstyle" ||
				line == "endinteractive" ||
				line == "endslide" ||
				line == "endpattern" ||
				line.StartsWith("slide ") ||
				line.StartsWith("interactive ") ||
				line.StartsWith("style ");
			} }

		private CodeReader(string text, int currentLine)
		{
			StartLine = currentLine;
			TextLine = 0;
			Text = text.Split(new char[] { '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Where(str => !str.Trim().StartsWith("//")).ToArray();
		}

		public string NextLine()
		{
			if (Done)
				return String.Empty;
			line = Text[TextLine].Trim().Trim('\r', '\t');
			if(line.Contains("//"))
				line = line.Substring(0, line.IndexOf("//")).Trim().Trim('\r', '\t'); ;
			TextLine++;
			if(String.IsNullOrWhiteSpace(line) && !Done)
				line = NextLine();
			return line;
		}

		public string PeekPreviousLine()
		{
			TextLine--;
			string result = NextLine();
			TextLine--;
			return result;
		}

		public CodeReader SubReader()
		{
			return new CodeReader(string.Join("\n", Text.Skip(TextLine)), CurrentLine);
		}

		public CodeReader Copy()
		{
			return new CodeReader(string.Join("\n", Text.Skip(TextLine - 1)), CurrentLine);
		}

		public static CodeReader FromFile(string path)
		{
			CodeReader result = null;
			using(FileStream fs = new FileStream(path, FileMode.Open))
				using (StreamReader sr = new StreamReader(fs))
					result = FromText(sr.ReadToEnd());
			result.fileSystemWatcher = new FileSystemWatcher();
			result.fileSystemWatcher.Path = Path.GetDirectoryName(Path.GetFullPath(path));
			result.fileSystemWatcher.Filter = Path.GetFileName(Path.GetFullPath(path));
			result.fileSystemWatcher.EnableRaisingEvents = true;
			result.fileSystemWatcher.Changed += result.InternFileChanged;
			return result;
		}

		public void Reload(string file)
		{
			using (FileStream fs = new FileStream(file, FileMode.Open))
				using (StreamReader sr = new StreamReader(fs))
				{
					TextLine = 0;
					Text = sr.ReadToEnd().Split(new char[] { '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Where(str => !str.Trim().StartsWith("//")).ToArray();
				}
		}

		private void InternFileChanged(object sender, FileSystemEventArgs e)
		{
			Reload(e.FullPath);
			FileChanged?.Invoke();
		}

		public delegate void FileChangedEventHandler();
		public event FileChangedEventHandler FileChanged;

		public static CodeReader FromText(string text)
		{
			return new CodeReader(text, 0);
		}

		internal void Skip(int lines)
		{
			TextLine += lines;
		}
	}
}
