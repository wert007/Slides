using Slides;
using Slides.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Slides.Interactives.Types;
using System.Windows.Threading;
using System.Reflection;
using System.Windows.Input;
using Slides.Styles;

namespace SlidesWPF
{
	public class PresentationWPF : Grid
	{
		PresentationSession session;
		Step current;
		List<DispatcherTimer> currentLoops;
		Grid display;

		public new float Width => (float)display.ActualWidth;
		public new float Height => (float)display.ActualHeight;


		public PresentationWPF(Presentation presentation)
		{
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;
			session = presentation.CreateSession();
			display = new Grid();
			Children.Add(display);
			current = session.RequestNext();
			Focus();
			currentLoops = new List<DispatcherTimer>();
		}

		public void Init()
		{
			ChangeSlide(current);
			//currentLoops = new List<DispatcherTimer>();
			//foreach (var loop in current.GetLoops())
			//{
			//	var timer = new DispatcherTimer();
			//	timer.Interval = TimeSpan.FromSeconds(1.0 / loop.FPS);
			//	timer.Tick += (s, e) =>
			//	{
			//		if (!loop.Run(current.GetVariables()))
			//			timer.Stop();
			//		else
			//			ChangeSlide(current);
			//	};
			//	currentLoops.Add(timer);
			//}
			//currentLoops.ForEach(t => t.Start());
		}

		public void ChangeSlide(Step step)
		{

			foreach (var child in display.Children)
			{
				if (child is WebBrowser browser)
				{
					browser.Dispose();
					browser = null;
				}
			}
			display.Children.Clear();

			display.Margin = Convert(step.Parent.Padding);
			Background = Convert(step.Parent.Background);
			display.Background = Brushes.Transparent;
			foreach (var child in step.Parent.GetElementsUntil(step)) 
			{
				display.Children.Add(ConvertChild(child));
			}
		}

		private FrameworkElement ConvertChild(Element child)
		{
			if (child is Title title)
			{
				StackPanel stackPanel = new StackPanel();
				stackPanel.Orientation = System.Windows.Controls.Orientation.Vertical;
				stackPanel = Positionate(title, stackPanel);
				stackPanel.Children.Add(ConvertChild(title.Main));
				stackPanel.Children.Add(ConvertChild(title.Subtitle));
				return stackPanel;
			}
			if (child is Slides.Components.Label lbl)
			{
				System.Windows.Controls.Label label = new System.Windows.Controls.Label();
				label.Padding = new System.Windows.Thickness();
				label = Positionate(lbl, label);
				label.Content = lbl.Content;
				label.FontSize = ConvertFontsize(lbl.Fontsize);
				label.Foreground = new SolidColorBrush(Convert(lbl.Foreground));
				if(lbl.Font != null)
					label.FontFamily = new FontFamily(lbl.Font.Name);
				return label;
			}
			if (child is Slides.Components.Image img)
			{
				System.Windows.Controls.Image image = new System.Windows.Controls.Image();
				image.Source = Convert(img.Source);
				image.HorizontalAlignment = Convert(img.HorizontalAlignment);
				image.VerticalAlignment = Convert(img.VerticalAlignment);
				image.Margin = Convert(img.Margin);
				image.Stretch = Convert(img.Scale);
				return image;
			}
			if (child is YouTubeVideo vid)
			{

				WebBrowser browser = new WebBrowser();

				browser = Positionate(vid, browser);
				browser.Navigate(new Uri(vid.Url));
				dynamic activeX = browser.GetType().InvokeMember("ActiveXInstance",
					  BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
					  null, browser, new object[] { });

				activeX.Silent = true;

				return browser;
			}
			if (child is Stack stack)
			{
				StackPanel stackPanel = new StackPanel();
				stackPanel.Orientation = stack.ContentOrientation == Slides.Components.Orientation.Vertical ?  System.Windows.Controls.Orientation.Vertical : System.Windows.Controls.Orientation.Horizontal;
				stackPanel = Positionate(stack, stackPanel);
				foreach (var stackChild in stack.Content.Children)
				{
					stackPanel.Children.Add(ConvertChild(stackChild));
				}
				return stackPanel;
			}
			else
				throw new NotImplementedException();
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if(e.Key == Key.Space)
			{
				SetCurrent(session.RequestNext());
				ChangeSlide(current);
			}
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			SetCurrent(session.RequestNext());
			if (current != null)
				ChangeSlide(current);
		}

		private void SetCurrent(Step cur)
		{
			//currentLoops.ForEach(t => t.Stop());
			current = cur;
		}

		public T Positionate<T>(Element e, T target) where T : FrameworkElement
		{
			target.HorizontalAlignment = Convert(e.HorizontalAlignment);
			target.VerticalAlignment = Convert(e.VerticalAlignment);
			target.Margin = Convert(e.Margin);
			
			if(target is Control)
			{
				Control control = target as Control;
				control.Padding = Convert(e.Padding);
				control.Background = Convert(e.Background);
			}
			return target;
		}

		public Stretch Convert(Scale scale)
		{
			switch (scale)
			{
				case Scale.No:
					return Stretch.None;
				case Scale.Fill:
					return Stretch.Fill;
				case Scale.Fit:
					return Stretch.Uniform;
				case Scale.FillFit:
					return Stretch.UniformToFill;
				default:
					throw new NotSupportedException();
			}
		}

		public Brush Convert(Background background)
		{
			if (background == null)
				return null;
			if (background.Image != null)
			{
				return new ImageBrush(Convert(background.Image));
			}
			else if (background.Color != null)
			{
				return new SolidColorBrush(Convert(background.Color));
			}
			else throw new NotSupportedException();
			//return new Brush
		}

		public System.Windows.Media.ImageSource Convert(Slides.Interactives.Types.ImageSource source)
		{
			ImageSourceConverter isc = new ImageSourceConverter();
			var r = isc.ConvertFromString(source.Path);
			return (System.Windows.Media.ImageSource)r;
		}

		public double ConvertFontsize(StyleUnit size)
		{
			double result = 0;
			switch (size.Unit)
			{
				case Unit.Pixel:
					result = size.Value;
					break;
				case Unit.Percent:
					result = size.Value * Height / 100.0;
					break;
				default:
					throw new NotImplementedException();
			}
			return result;
		}

		public System.Windows.Thickness Convert(Slides.Interactives.Types.Thickness margin)
		{
			float left = margin.Left.Value;
			float top = margin.Top.Value;
			float right = margin.Right.Value;
			float bottom = margin.Bottom.Value;
			if (margin.Left.Unit == Unit.Percent)
				left *= Width / 100f;
			if (margin.Top.Unit == Unit.Percent)
				top *= Height / 100f;
			if (margin.Right.Unit == Unit.Percent)
				right *= Width / 100f;
			if (margin.Bottom.Unit == Unit.Percent)
				bottom *= Height / 100f;
			return new System.Windows.Thickness(left, top, right, bottom);
		}

		public System.Windows.Media.Color Convert(Slides.Interactives.Types.Color color)
		{
			byte r = byte.Parse(color.R);
			byte g = byte.Parse(color.G);
			byte b = byte.Parse(color.B);
			return System.Windows.Media.Color.FromRgb(r, g, b);
		}

		public HorizontalAlignment Convert(Horizontal alignment)
		{
			switch (alignment)
			{
				case Horizontal.Left:
					return HorizontalAlignment.Left;
				case Horizontal.Center:
					return HorizontalAlignment.Center;
				case Horizontal.Right:
					return HorizontalAlignment.Right;
				default:
					throw new NotSupportedException();
			}
		}

		public VerticalAlignment Convert(Vertical alignment)
		{
			switch (alignment)
			{
				case Vertical.Top:
					return VerticalAlignment.Top;
				case Vertical.Center:
					return VerticalAlignment.Center;
				case Vertical.Bottom:
					return VerticalAlignment.Bottom;
				default:
					throw new NotSupportedException();
			}
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			Init();
		}
	}
}
