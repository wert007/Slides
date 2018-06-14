using System;
using System.Collections.Generic;

namespace Slides
{
	public class Variable
	{
		public string Name { get; set; }
		public object Value { get; set; }

		public Variable(string name, object value)
		{
			Name = name;
			Value = value;
		}

		public override bool Equals(object obj)
		{
			if (obj is Variable v)
				return Name == v.Name;
			return base.Equals(obj);
		}

		public bool ValueEquals(object obj)
		{
			if (obj is Variable v)
				return Value == v.Value;
			return false;
		}

		public override string ToString()
		{
			return Name + ": " + Value.ToString();
		}

		public override int GetHashCode()
		{
			var hashCode = -244751520;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
			hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
			return hashCode;
		}
	}
}