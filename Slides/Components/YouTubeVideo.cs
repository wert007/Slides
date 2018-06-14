using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Components
{
	public class YouTubeVideo : Element
	{
		public string Url { get; private set; }
		public string Id { get; private set; }

		public YouTubeVideo(string id) : base("")
		{
			Id = id;
			Url = "http://www.youtube.com/watch?v=" + id + "";
		}
	}
}
