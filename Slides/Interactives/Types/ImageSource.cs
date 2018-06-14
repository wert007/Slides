using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Types
{
	public class ImageSource
	{
		public string Path { get; private set; }

		public ImageSource(string path)
		{
			Path = path;
		}
	}
}
