using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Slides.Styles
{
	public class StyleUnit : StyleValue
	{
		public Unit Unit { get; private set; }
		public float Value { get; private set; }

		public override string RegEx => RegExHelper.Number + "(%|px)";

		public StyleUnit(float value, Unit unit)
		{
			this.Unit = unit;
			this.Value = value;
		}

		public override object Compute()
		{
			return this;
		}

		public static Unit FromString(string unit)
		{
			switch (unit)
			{
				case "px":
					return Unit.Pixel;
				case "%":
					return Unit.Percent;
				default:
					throw new ArgumentException("Couldn't parse " + unit + " into a Unit.");
			}
		}

		public static StyleUnit Parse(string s)
		{
			string value = Regex.Match(s, RegExHelper.Number).Value;
			string unit = s.Substring(s.IndexOf(value) + value.Length);
			return new StyleUnit(float.Parse(value), FromString(unit));
		}

		public static implicit operator StyleUnit (float value) => new StyleUnit(value, Unit.Percent);
		public static StyleUnit operator +(StyleUnit a, StyleUnit b)
		{
			if(a.Unit == b.Unit)
			{
				return new StyleUnit(a.Value + b.Value, a.Unit);
			}
			else
			{
				if (a > b)
					return a;
				return b;
			}
		}

		public static bool operator > (StyleUnit a, StyleUnit b)
		{
			if (a.Unit == b.Unit)
				return a.Value > b.Value;
			if(a.Unit == Unit.Percent)
				return true;
			return false;
		}

		public static bool operator < (StyleUnit a, StyleUnit b)
		{
			if (a.Unit == b.Unit)
				return a.Value < b.Value;
			if (a.Unit == Unit.Percent)
				return false;
			return true;
		}
	}
	public enum Unit
	{
		Pixel,
		Percent,
	}
}
