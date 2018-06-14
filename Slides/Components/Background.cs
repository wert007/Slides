using Slides.Interactives.Types;

namespace Slides
{
	public class Background
	{
		public Color Color { get; set; }
		public ImageSource Image { get; set; }

		public static implicit operator Background (Color color)
		{
			return new Background()
			{
				Color = color
			};
		}

		public static implicit operator Background (ImageSource image)
		{
			return new Background()
			{
				Image = image
			};
		}
	}
}