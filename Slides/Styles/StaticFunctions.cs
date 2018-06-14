using Slides.Components;
using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Styles
{
	public static class StaticFunctions
	{
		public static void Center(object[] args)
			=> Center((Element)args[0]);
		public static void Center(Element element)
		{
			element.HorizontalAlignment = Horizontal.Center;
			element.VerticalAlignment = Vertical.Center;
		}

		public static void Margin(object[] args)
		{
			switch (args.Length)
			{
				case 1:
					throw new ArgumentException();
				case 2:
					Margin((Element)args[0], (StyleUnit)args[1]);
					break;
				case 3:
				case 4:
					Margin((Element)args[0], (StyleUnit)args[1], (StyleUnit)args[2]);
					break;
				default:
					Margin((Element)args[0], (StyleUnit)args[1], (StyleUnit)args[2], (StyleUnit)args[3], (StyleUnit)args[4]);
					break;
			}
		}

		public static void Padding(object[] args)
		{
			switch (args.Length)
			{
				case 1:
					throw new ArgumentException();
				case 2:
					Padding((Element)args[0], (StyleUnit)args[1]);
					break;
				case 3:
				case 4:
					Padding((Element)args[0], (StyleUnit)args[1], (StyleUnit)args[2]);
					break;
				default:
					Padding((Element)args[0], (StyleUnit)args[1], (StyleUnit)args[2], (StyleUnit)args[3], (StyleUnit)args[4]);
					break;
			}
		}

		public static void Margin(Element element, StyleUnit thickness)
			=> element.Margin += new Thickness(thickness);

		public static void Margin(Element element, StyleUnit horizontal, StyleUnit vertical)
			=> element.Margin += new Thickness(horizontal, vertical);

		public static void Margin(Element element, StyleUnit top, StyleUnit right, StyleUnit bottom, StyleUnit left)
			=> element.Margin += new Thickness(top, right, bottom, left);

		public static void Padding(Element element, StyleUnit thickness)
			=> element.Padding += new Thickness(thickness);

		public static void Padding(Element element, StyleUnit horizontal, StyleUnit vertical)
			=> element.Padding += new Thickness(horizontal, vertical);

		public static void Padding(Element element, StyleUnit top, StyleUnit right, StyleUnit bottom, StyleUnit left)
			=> element.Padding += new Thickness(top, right, bottom, left);

		public static void ResetMargin(object[] args)
			=> ResetMargin((Element)args[0]);
		public static void ResetMargin(Element element) 
			=> element.Margin = new Thickness();

		internal static ImageSource LoadImage(object[] args)
			=> LoadImage((string)args[0]);
		public static ImageSource LoadImage(string path) 
			=> new ImageSource(path);

		internal static object LoadYoutubeVideo(object[] args)
			=> LoadYoutubeVideo((string)args[0]);

		public static YouTubeVideo LoadYoutubeVideo(string url)
			=> new YouTubeVideo(url);
	}
}
