using Slides.Interactives.Types;

namespace Slides.Components
{
	public class Title : Stack
	{
		public Label Main { get; set; }
		public Label Subtitle { get; set; }

		public string Text
		{
			get { return MainText; }
			set { MainText = value; }
		}
		public string MainText
		{
			get { return Main.Content; }
			set { Main.Content = value; }
		}
		public string SubtitleText
		{
			get { return Subtitle.Content; }
			set { Subtitle.Content = value; }
		}

		public Color ColorTitle
		{
			get { return Main.Foreground; }
			set { Main.Foreground = value; }
		}
		public Color ColorSubtitle
		{
			get { return Subtitle.Foreground; }
			set { Subtitle.Foreground = value; }
		}

		public bool HasSubtitle => !string.IsNullOrEmpty(SubtitleText);

		public Title(string text) : base("v")
		{
			Main = new Label(text);
			Add(Main);
			Subtitle = new Label("");
		}

		public void AddSubtitle(string text)
		{
			Subtitle.Content = text;
			Add(Subtitle);
		}

		public void FontsizeTitle(float size)
		{
			Main.Fontsize = new Styles.StyleUnit(size, Styles.Unit.Pixel);
		}

		public void FontsizeSubtitle(float size)
		{
			Subtitle.Fontsize = new Styles.StyleUnit(size, Styles.Unit.Pixel);
		}
	}
}