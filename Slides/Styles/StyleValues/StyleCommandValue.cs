using Slides.Interactives.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides.Styles
{
	public class StyleCommandValue : StyleValue
	{
		object value;

		public StyleCommandValue(object value)
		{
			this.value = value;
			if (value is String str)
			{
				if (Regex.IsMatch(str, RegExHelper.String))
				{
					if (str.StartsWith("@"))
						str = str.Replace("\\", "\\\\").Substring(1);
					this.value = str.Trim('\'');
				}
				else if (int.TryParse(str, out int i))
					this.value = i;
				else if (float.TryParse(str, out float f))
					this.value = f;
			}
		}

		public override string RegEx => RegExHelper.Anything;

		public override object Compute()
		{
			return value;
		}
	}
}
