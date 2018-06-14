using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Components
{
	public class ElementCollection : Element
	{
		private Element[] element;
		public IEnumerable<Element> Children => element;

		public ElementCollection(Element[] element) : base("")
		{
			this.element = element;
		}
	}
}
