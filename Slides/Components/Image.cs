using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Components
{
	public class Image : Element
	{
		public ImageSource Source { get; set; }
		public Scale Scale { get; set; }

		public Image(ImageSource source) : base("")
		{
			Source = source;
			Scale = Scale.FillFit;
		}
	}

	public enum Scale
	{
		No,
		Fill,
		Fit,
		FillFit
	}
}
