using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Slides
{
	public static class ReflectionHelper
	{
		public static object Cast(this Type Type, object data)
		{
			var DataParam = Expression.Parameter(typeof(object), "data");
			var Body = Expression.Block(Expression.Convert(Expression.Convert(DataParam, data.GetType()), Type));

			var Run = Expression.Lambda(Body, DataParam).Compile();
			var ret = Run.DynamicInvoke(data);
			return ret;
		}
	}
}
