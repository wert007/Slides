using Slides;
using Slides.Components;
using Slides.Interactives.Types;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidesToReact
{
    public class PresentationReact
    {
		PresentationSession session;
		string path;

		public PresentationReact(Presentation presentation, string path)
		{
			this.session = presentation.CreateSession();
			this.path = path;
		}

		public void Parse()
		{
			using (StreamWriter sw = new StreamWriter(path))
			{
				sw.WriteLine(GetHeader());
				Step current = null;
				Step old = null;
				bool firstStep = true;
				bool firstSlide = true;
				do
				{
					old = current;
					current = session.RequestNext();
					if (current == null)
					{
						if (!firstStep)
							sw.WriteLine("</div>");
						sw.WriteLine("</section>");
						break;
					}
					if (current is Slide)
					{
						if (!firstSlide)
							sw.WriteLine("</section>");
						sw.WriteLine("<section id=\"" + current.Name + "\" data-background=\"" + Convert(current.Background) + "\">");
						firstStep = true;
						firstSlide = false;
					}
					else if(current is Step)
					{
						if (!firstStep)
							sw.WriteLine("</div>");
						//TODO id of steps
						sw.WriteLine("<div id=\"" + current.Name + "\" class=\"fragment\">");
						firstStep = false;
					}
					foreach (var element in current.GetElements())
					{
						sw.WriteLine(Parse(element));
					}
				} while (current != null);
				sw.WriteLine(GetFooter());
			}
		}

		private string Parse(Element element, bool isChild = false)
		{
			string result = "";
			if (element is Title title)
			{
				if (title.HasSubtitle)
				{
					result += "<div id=\"" + title.Name + "\" style=\"" + ParseStyle(title, isChild) + "\">\n";
					result += "<h1 style=\"" + ParseStyle(title.Main, true) + "\">" + title.MainText + "</h1>\n";
					result += "<h2 style=\"" + ParseStyle(title.Subtitle, true) + "\">" + title.SubtitleText + "</h2>\n";
					result += "</div>";
				}
				else
				{
					result += "<h1 id=\"" + title.Name + "\" style=\"" + ParseStyle(title, isChild) + "\">" + title.MainText + "</h1>";
				}
			}
			else if (element is Label label)
			{
				result += "<p id=\"" + label.Name + "\" style=\"" + ParseStyle(label, isChild) + "\">" + label.Content + "</p>";
			}
			else if (element is Image image)
			{
				result += "<img id=\"" + image.Name + "\" style=\"" + ParseStyle(image, isChild) + "\" src=\"" + image.Source.Path + "\">";
			}
			else if (element is YouTubeVideo vid)
			{
				result += "<iframe id=\"" + vid.Name + "\" style=\"" + ParseStyle(vid, isChild) + "\" src=\"https://www.youtube.com/embed/" + vid.Id + "?rel=0\" frameborder=\"0\" allow=\"autoplay; encrypted-media\" allowfullscreen></iframe>";
			}
			else if (element is Stack stack)
			{
				result += "<div id=\"" + stack.Name + "\" style=\"" + ParseStyle(stack, isChild) + "\">\n";
				foreach (var child in stack.Content.Children)
				{
					result += Parse(child, true) + "\n";
				}
				result += "</div>";

			}
			else throw new NotImplementedException();
			return result;
		}

		private string ParseStyle(Element element, bool isChild)
		{
			string result = "\n";
			if(element.Background !=null)
				result +="background: " + Convert(element.Background) + ";\n";
			if (element.Foreground != null)
				result += "color: " + Convert(element.Foreground) + ";\n";
			switch (element.HorizontalAlignment)
			{
				case Horizontal.Left:
					if (element.Margin.Left.Value != 0)
						result += "left: " + Convert(element.Margin.Left.Value) + ";\n";
					if (element is Label || element is Title)
						result += "text-align: left;\n";
					break;
				case Horizontal.Center:
					if (element is Label || element is Title)
						result += "text-align: center;\n";
					break;
				case Horizontal.Right:
					if (element.Margin.Right.Value != 0)
						result += "right: " + Convert(element.Margin.Right.Value) + ";\n";
					if (element is Label || element is Title)
						result += "text-align: right;\n";
					break;
				default: throw new NotImplementedException();
			}
			switch (element.VerticalAlignment)
			{
				case Vertical.Top:
					if (element.Margin.Top.Value != 0)
						result += "top: " + Convert(element.Margin.Top) + ";\n";
					break;
				case Vertical.Center:
					break;
				case Vertical.Bottom:
					if(element.Margin.Bottom.Value != 0)
					result += "bottom: " + Convert(element.Margin.Bottom) + ";\n";
					break;
				default: throw new NotImplementedException();
			}
			if (element.HorizontalAlignment == Horizontal.Center ||
				element.VerticalAlignment == Vertical.Center ||
				isChild)
				result += "position: relative;\n";
			else
				result += "position: fixed;\n";
			if (element is Image)
			{
				result += "max-width: -webkit-fill-available;\n";
				result += "height: auto;\n";
				result += "width: auto;\n";
				result += "border: transparent;\n";
				result += "box-shadow: none;\n";
			}
			else if (element is YouTubeVideo)
			{
				result += "width: -webkit-fill-available;\n";
				result += "height: -webkit-fill-available;\n";
			}

			if (element is Label label)
			{
				if (label.Font != null)
					result += "font-family: '" + label.Font.Name + "', " + label.Font.GetCSSGeneric() + ";\n";
				result += "font-size: " + Convert(label.Fontsize) + ";\n";
			}
			if(element.Margin.Bottom.Value != 0 || element.Margin.Top.Value != 0 || element.Margin.Left.Value != 0 || element.Margin.Right.Value != 0)
				result += "margin: " + Convert(element.Margin, element.VerticalAlignment, element.HorizontalAlignment) + ";\n";
			if (element.Padding.Bottom.Value != 0 || element.Padding.Top.Value != 0 || element.Padding.Left.Value != 0 || element.Padding.Right.Value != 0)
				result += "padding: " + Convert(element.Padding) + ";\n";
			result = result.Trim('\n');
			return result;
		}

		private string Convert(Thickness thickness, Vertical vertical, Horizontal horizontal)
		{
			string result = "";
			if (vertical != Vertical.Top)
				result += Convert(thickness.Top) + " ";
			else
				result += "0 ";
			if (horizontal != Horizontal.Right)
				result += Convert(thickness.Right) + " ";
			else
				result += "0 ";
			if (vertical != Vertical.Bottom)
				result += Convert(thickness.Bottom) + " ";
			else
				result += "0 ";
			if (horizontal != Horizontal.Left)
				result += Convert(thickness.Left) + " ";
			else
				result += "0 ";
			return result;
		}

		private string Convert(Thickness thickness)
		{
			return Convert(thickness.Top) + " " + Convert(thickness.Right) + " " + Convert(thickness.Bottom) + " " + Convert(thickness.Left);
		}

		private string Convert(StyleUnit value)
		{
			switch (value.Unit)
			{
				case Unit.Pixel:
					return value.Value + "px";
				case Unit.Percent:
					return value.Value + "%";
				default: throw new NotImplementedException();
			}
		}

		private string Convert(Color color)
		{
			return "rgb(" + color.R + ", " + color.G + ", " + color.B + ")";
		}

		private string Convert(Background background)
		{
			if (background == null)
				return "transparent";
			if (background.Image != null)
			{
				return background.Image.Path.Replace('\\', '/');
			}
			else if (background.Color != null)
			{
				return Convert(background.Color);
			}
			else
				throw new NotImplementedException();
		}

		public static string GetHeader()
		{
			return "<!doctype html>\n<html lang=\"de\">\n<head>\n<meta charset = \"utf-8\">\n<title> Title </title>\n<meta name = \"description\" content = \"Beschreibung\">\n<meta name = \"author\" content = \"Autor\">\n<meta name = \"apple-mobile-web-app-capable\" content = \"yes\">\n<meta name = \"apple-mobile-web-app-status-bar-style\" content = \"black-translucent\">\n<meta name = \"viewport\" content = \"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no\"><link rel = \"stylesheet\" href = \"css/reveal.css\"><link rel = \"stylesheet\" href = \"css/theme/own-theme.css\" id = \"theme\"><link rel = \"stylesheet\" href = \"css/general.css\"><script src = \"js/general.js\"></script><!--Theme used for syntax highlighting of code--><link rel = \"stylesheet\" href = \"lib/css/zenburn.css\"><!--Printing and PDF exports--><script>var link = document.createElement('link');link.rel = 'stylesheet';link.type = 'text/css';link.href = window.location.search.match(/print-pdf/gi) ? 'css/print/pdf.css' : 'css/print/paper.css';document.getElementsByTagName('head')[0].appendChild(link);</script><!--[if lt IE 9]><script src = \"lib/js/html5shiv.js\"></script><![endif]--></head><body onload = \"main\"><div class=\"reveal\"><div class=\"slides\">";
		}

		public static string GetFooter()
		{
			return "<script src=\"lib/js/head.min.js\"></script><script src = \"js/reveal.js\"></script><script>// More info https://github.com/hakimel/reveal.js#configuration \n Reveal.initialize({history: true,transition: 'zoom',transitionSpeed: 'slow',backgroundTransition: 'slide',previewLinks: false,\n// More info https://github.com/hakimel/reveal.js#dependencies \ndependencies: [{ src: 'lib/js/classList.js', condition: function() { return !document.body.classList; } },{ src: 'plugin/markdown/marked.js', condition: function() { return !!document.querySelector('[data-markdown]'); } },{ src: 'plugin/markdown/markdown.js', condition: function() { return !!document.querySelector('[data-markdown]'); } },{ src: 'plugin/highlight/highlight.js', async: true, callback: function() { hljs.initHighlightingOnLoad(); } },{ src: 'plugin/zoom-js/zoom.js', async: true },{ src: 'plugin/notes/notes.js', async: true }]});Reveal.configure({keyboard:{13: toBase(),}});</script></body></html> ";
		}
	}
}
