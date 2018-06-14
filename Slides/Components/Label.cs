using Slides.Interactives.Types;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Components
{
	public class Label : Element
	{
		public string Content { get; set; }
		public StyleUnit Fontsize { get; set; }
		public Font Font { get; set; }

		public Label(string content) : base("")
		{
			Content = content;
			Fontsize = 15;
			Foreground = Color.Black;
		}

		
	}
}
