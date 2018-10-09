using Slides;
using Slides.Interactives.Commands;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slides.Components;
using Slides.Imports;
using Slides.Interactives.Types;

namespace SlidesToHTML
{
	public class PresentationHTML
	{
		public static void Create(string path, Presentation presentation)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			using (FileStream fscustomcss = new FileStream(Path.Combine(path, "std.css"), FileMode.Create))
			using (StreamWriter swcustomcss = new StreamWriter(fscustomcss))
			{
				swcustomcss.Write(ToCSS(presentation.Standard));
				foreach (var slide in presentation.CreateSession().IterateSlides())
				{
					swcustomcss.Write(ToCSS(slide, presentation.Standard));
					foreach (var step in slide.IterateSteps())
					{
						foreach (var element in step.GetElements())
						{
							swcustomcss.Write(ToCSS(element, slide, presentation.Standard));
							if(element is Title t) //TODO: Handle Children of an element
							{

							}
						}
					}
				}
				swcustomcss.Flush();
			}

			StringBuilder htmlPage = new StringBuilder();
			htmlPage.AppendLine("<head>\n<link href=\"std.css\" rel=\"stylesheet\">\n<link href=\"core.css\" rel=\"stylesheet\">\n<script type=\"text/javascript\" src=\"core.js\" ></script>");
			foreach (var import in presentation.Imports)
			{
				if (import is GoogleFontImport gfontImport)
				{
					htmlPage.AppendLine(ToHeaderReference((GoogleFont)gfontImport.Value));
				}
			}
			htmlPage.AppendLine("</head>\n<body onload=\"load()\" onkeydown=\"keyDown(event);\">");
			foreach (var slide in presentation.CreateSession().IterateSlides())
			{
				htmlPage.AppendLine("<section id=\"" + slide.Name + "\" class=\"slide\">");
				foreach (var step in slide.IterateSteps())
				{
					htmlPage.AppendLine("<div class=\"Step\" data-slide-id=\"" + slide.Name + "\">");
					foreach (var element in step.GetElements())
					{
						string[] divs = CreateDivContainers(element);
						if(divs.Length > 0)
						htmlPage.AppendLine(divs[0]);
						htmlPage.AppendLine(ToHTML(slide, element));
						if(divs.Length > 0)
						htmlPage.AppendLine(divs[1]);
					}
					htmlPage.AppendLine("</div>");
				}
				htmlPage.AppendLine("</section>");
			}
			htmlPage.AppendLine("</body>");
			using (FileStream fs = new FileStream(Path.Combine(path, "haehhpresentation.html"), FileMode.Create))
			{
				using (StreamWriter sw = new StreamWriter(fs))
				{
					sw.Write(htmlPage.ToString());
					sw.Flush();
					sw.Close();
				}
			}
		}

		private static string[] CreateDivContainers(Element element)
		{
			string result0 = "<div class=\"outer\">\n";
			string result1 = "</div>\n";
			switch (element.VerticalAlignment)
			{
				case Vertical.Top:
					result0 += "<div class=\"middleTop\">\n";
					break;
				case Vertical.Center:
					result0 += "<div class=\"middleCenter\">\n";
					break;
				case Vertical.Bottom:
					result0 += "<div class=\"middleBottom\">\n";
					break;
				default:
					throw new NotImplementedException();
			}
			result1 += "</div>\n";
			switch (element.HorizontalAlignment)
			{
				case Horizontal.Left:
					result0 += "<div class=\"innerLeft\">\n";
					break;
				case Horizontal.Center:
					result0 += "<div class=\"innerCenter\">\n";
					break;
				case Horizontal.Right:
					result0 += "<div class=\"innerRight\">\n";
					break;
				default: throw new NotImplementedException();
			}
			result1 += "</div>\n";
			return new string[] { result0, result1 };
		}

		private static string ToHTML(Slide parent, Element element)
		{
			string id = parent.Name + "-" + element.Name;
			switch (element.VerticalAlignment)
			{
				case Vertical.Top:
					break;
				case Vertical.Center:
					break;
				case Vertical.Bottom:
					break;
				default:
					throw new NotImplementedException();
			}
			switch (element.HorizontalAlignment)
			{
				case Horizontal.Left:
					break;
				case Horizontal.Center:
					break;
				case Horizontal.Right:
					break;
				default:
					throw new NotImplementedException();
			}
			if (element is Image img)
			{
				return CreateHTMLNoContentTag("img", "Image", id, img.Source.Path, string.Empty);
			}
			else if (element is Label lbl)
			{
				return CreateHTMLContentTag("p", "Label", id, lbl.Content, String.Empty);
			}
			else if (element is List list)
			{
				if (list.ListType == ListType.Ordered)
				{
					return CreateHTMLContentTag("ol", "OrderedList", id, ToHTMLListElements(parent, list.Content), string.Empty);
				}
				else if (list.ListType == ListType.Unordered)
				{
					return CreateHTMLContentTag("ul", "UnorderedList", id, ToHTMLListElements(parent, list.Content), string.Empty);
				}
				else throw new NotImplementedException();
			}
			else if (element is Title title)
			{
				return CreateDivContainer("Title", id, string.Empty, CreateHollowHTMLContentTag("h1", title.Text), CreateHollowHTMLContentTag("h2", title.SubtitleText));
			}
			else if (element is YouTubeVideo ytvid)
			{
				//TODO Encapsulate in to a function.
				return "<iframe id=\"" + id + "\" src=\"https://www.youtube.com/embed/" + ytvid.Id + "\" frameborder=\"0\" allow=\"autoplay; encrypted-media\" allowfullscreen></iframe>";
			}
			else throw new NotImplementedException();
		}

		private static string CreateDivContainer(string @class, string id, string otherValues, params string[] contents)
		{
			return "<div class=\"" + @class + "\" id=\"" + id + "\" " + otherValues + ">\n" + string.Join("\n", contents) + "\n</div>";
		}

		private static string CreateHollowHTMLContentTag(string tag, string content)
		{
			return "<" + tag + ">" + content + "</" + tag + ">";
		}

		private static string CreateHTMLNoContentTag(string tag, string @class, string id, string source, string otherValues)
		{
			return "<" + tag + " class=\"" + @class + "\" id=\"" + id + "\" " + (string.IsNullOrWhiteSpace(source) ? string.Empty : ("src=\"" + source + "\" ")) + otherValues + "\\>";
		}

		private static string CreateHTMLContentTag(string tag, string @class, string id, string content, string otherValues)
		{
			return "<" + tag + " class=\"" + @class + "\" id=\"" + id + "\" " + otherValues + ">\n" + content + "\n</" + tag + ">";
		}

		private static string ToHTMLListElements(Slide parent, ElementCollection content)
		{
			StringBuilder result = new StringBuilder();
			foreach (var listItem in content.Children)
			{
				result.AppendLine(CreateHollowHTMLContentTag("li", ToHTML(parent, listItem))); //TODO
			}
			return result.ToString();
		}

		private static bool HasAnyCSSValue(Element element, Style std)
		{

			return true;
		}

		private static string ToCSS(Slide slide, Style std)
		{
			if (!HasAnyCSSValue(slide, std))
				return string.Empty;

			StringBuilder result = new StringBuilder("#" + slide.Name + " {\n");
			if (slide.Background != null)
			{
				if (slide.Background.Color != null)
					result.AppendLine("background-color: " + ToCSSValue(slide.Background.Color));
				if (slide.Background.Image != null)
					result.AppendLine("background-image: " + ToCSSValue(slide.Background.Image));
			}
			result.AppendLine("}");
			result.AppendLine();
			return result.ToString();
		}

		private static string ToCSS(Element element, Slide parent, Style std)
		{
			if (!HasAnyCSSValue(element, std))
				return string.Empty;
			string id = parent.Name + "-" + element.Name;
			StringBuilder result = new StringBuilder("#" + id + " {\n");
			if (element.Background != null)
			{
				if (element.Background.Color != null)
					result.AppendLine("background-color: " + ToCSSValue(element.Background.Color) + ";");
				if (element.Background.Image != null)
					result.AppendLine("background-image: " + ToCSSValue(element.Background.Image) + ";");
			}
			result.AppendLine(ToCSSLine(element.Margin, element.HorizontalAlignment, element.VerticalAlignment));
			if(element is Image img)
			{
				int width = 100 - (int)(img.Margin.Left.Value + img.Margin.Right.Value);
				int height = 100 - (int)(img.Margin.Top.Value + img.Margin.Bottom.Value);
				System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(img.Source.Path);
				int actualWidth = (int)(bitmap.Width * (width / 100f));
				int actualHeight = (int)(bitmap.Height * (height / 100f));
				if(actualWidth < actualHeight)
					result.AppendLine("width: " + width + "vw;");
				else
					result.AppendLine("height: " + height + "vh;");
			}
			result.AppendLine("}");
			result.AppendLine();
			return result.ToString();
		}

		private static string ToCSS(Style style)
		{
			StringBuilder generalInstructions = new StringBuilder();
			Dictionary<string, StringBuilder> specificInstructions = new Dictionary<string, StringBuilder>();

			foreach (var runnable in style.IterateRunnables())
			{
				if (runnable is Instruction instruction)
				{
					if (instruction.IsSpecific)
					{
						string type = instruction.GetChildType();
						if (type == "label")
						{
							generalInstructions.Append(ToCSSLine(instruction.GetProperty(), instruction.Value));
							continue;
						}
						if (!specificInstructions.ContainsKey(type))
							specificInstructions.Add(instruction.GetChildType(), new StringBuilder());
						specificInstructions[type].AppendLine(ToCSSLine(instruction.GetProperty(), instruction.Value));
					}
					else
					{
						generalInstructions.AppendLine(ToCSSLine(instruction.GetProperty(), instruction.Value));
					}
				}
				else if (runnable is Command command)
				{
					throw new NotImplementedException();
				}
			}
			string result = CreateCSSBlock("slide", generalInstructions);
			foreach (var pair in specificInstructions)
			{
				result += CreateCSSBlock(pair.Key, pair.Value);
			}

			result += "\n";
			return result;
		}

		private static string CreateCSSBlock(string type, StringBuilder content)
		{
			string name = "";
			switch (type)
			{
				case "slide":
				case "label":
					name = ".slide";
					break;
				case "children":
					name = "div > *";
					break;
				default:
					throw new NotImplementedException();
			}
			return name + "{\n" + content.ToString() + "\n}";
		}
		
		private static string ToCSSLine(Thickness margin, Horizontal horizontal, Vertical vertical)
		{
			string result = "margin: " + 
				string.Join(" ", 
				ToCSSValue(margin.Top, true),
				ToCSSValue(margin.Right, false),
				ToCSSValue(margin.Bottom, true),
				ToCSSValue(margin.Left, false))
				+ ";";
			return result;
		}

		private static string ToCSSLine(string property, StyleValue value)
		{
			switch (property)
			{
				case "foreground":
					return ToCSSLine("color", value);
				case "fontsize":
					return ToCSSLine("font-size", value);

				default:
					return property + ": " + ToCSSValue(value) + ";";

			}
		}

		private static string ToCSSValue(Color color)
		{
			return "rgb(" + color.R + ", " + color.G + ", " + color.B + ")";
		}

		private static string ToCSSValue(ImageSource source)
		{
			return "url(" + source.Path.Replace('\\', '/') + ")";
		}

		private static string ToCSSValue(StyleValue value, bool? isVertical = null)
		{
			if (value is StyleUnit unit)
			{
				string type = "";
				switch (unit.Unit)
				{
					case Unit.Pixel:
						type = "px";
						break;
					case Unit.Percent:
						if (isVertical == null)
							throw new ArgumentException();
						else if (isVertical == true)
							type = "vh";
						else
							type = "vw";
						break;
					default:
						throw new NotImplementedException();
				}
				return unit.Value + type;
			}
			else if (value is StyleValueFunc func)
			{
				return func.Name + "(" + string.Join(", ", func.Parameters) + ")";
			}
			else if (value is StyleValueConstant styleConst)
			{
				return styleConst.Name;
			}
			else if (value is StyleCommandValue commandValue)
			{
				return commandValue.Compute().ToString();
			}
			else throw new NotImplementedException();
		}

		private static string ToHeaderReference(GoogleFont gfont)
		{
			//TODO
			throw new NotImplementedException();
		}
	}
}
