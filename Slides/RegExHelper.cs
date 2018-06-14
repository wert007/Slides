using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides
{
	public static class RegExHelper
	{
		public static string Variable => @"[a-zA-Z_][a-zA-Z_0-9]*";
		public static string File => @"'[a-zA-Z_0-9\\:. ]+'";
		public static string Function => Variable + @"\((" + Variable + "(, " + Variable + @")*)?\)";
		public static string FunctionParameters => @"(" + Variable + "(, " + Variable + @")*)?";
		public static string StyleFunction => @"(" + StyleAPPLYFunction +
			"|" + StyleCSSFunction +
			"|" + StyleHSVFunction +
			"|" + StyleRGBFunction +
			"|" + StyleIMAGEFunction + ")";
		public static string StyleRGBFunction => @"rgb\(" + Integer + ", " + Integer + ", " + Integer + @"\)";
		public static string StyleHSVFunction => @"hsv\(" + Integer + ", " + Integer + ", " + Integer + @"\)";
		public static string StyleCSSFunction => @"css\(" + File + @"\)";
		public static string StyleAPPLYFunction => @"apply\(" + Variable + @"\)";
		public static string StyleIMAGEFunction => @"image\(" + File + @"\)";
		public static string StyleConstants => @"(red|green|blue|yellow|brown|orange|pink|black|white|gray)";
		public static string StyleValue => Number + "(px|em|%)";
		public static string Number => @"(" + Integer + "|" + Float + ")";
		public static string Integer => @"\d+";
		public static string Float => @"(\d*\.\d+|\d+f)";
		public static string FunctionHeader => Variable + @"\((" + Variable + "(, " + Variable + @")*)?\)";
		public static string FunctionCallHeader => Variable + @"\((" + ValueOrConstructor + "(, " + ValueOrConstructor + @")*)?\)";
		public static string FunctionCall => Variable + "\\." + FunctionCallHeader;
		public static string ConstructorCall => "new " + Variable + @"\((" + Value + "(, " + Value + @")*)?\)";
		public static string Anything => @".*";
		public static string Line => @"[^\n]+";
		public static string String => @"@?'.*'";
		public static string Value => @"(" + String + "|" + Number + "|" + Variable + "|" + EnumOrProperty + "|" + StyleValue + ")";
		public static string ValueOrConstructor => "(" + Value + "|" + ConstructorCall + ")";
		public static string EnumOrProperty => Variable + "\\." + Variable;
		public static string Time => "(s|min|h|d)";
	}

}
