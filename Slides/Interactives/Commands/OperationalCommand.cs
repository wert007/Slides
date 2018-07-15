using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Commands
{
	public class OperationalCommand : Command
	{
		Operation operation;
		Command left;
		Command right;

		public OperationalCommand(Operation operation, Command left, Command right) : base(left.ReturnType, 1)
		{
			this.operation = operation;
			this.left = left;
			this.right = right;
		}

		public static string Regex => RegExHelper.Command + "(\\+|-|\\*|\\/|^|%)" + RegExHelper.Command;
		public override bool EditsVariables => false;

		public override int LineLength => 1;

		public static Operation FromString(string type)
		{
			switch (type)
			{
				case "+": return Operation.Plus;
				case "-": return Operation.Minus;
				case "*": return Operation.Multiply;
				case "/": return Operation.Divide;
				case "^": return Operation.Pow;
				case "%": return Operation.Modulo;
				default:
					throw new ArgumentException("Couldn't translate " + type + " into an Operation");
			}
		}

		public static string ToString(Operation type)
		{
			switch (type)
			{
				case Operation.Plus: return "+";
				case Operation.Minus: return "-";
				case Operation.Multiply: return "*";
				case Operation.Divide: return "/";
				case Operation.Modulo: return "%";
				case Operation.Pow: return "^";
				default:
					throw new NotImplementedException();
			}
		}

		protected override object InnerRun(List<Variable> variables)
		{
			var valLeft = left.Run(variables);
			var valRight = right.Run(variables);
			var valLeftStr = valLeft.ToString();
			var valRightStr = valRight.ToString();

			if (int.TryParse(valLeftStr, out int intLeft) && int.TryParse(valRightStr, out int intRight))
				switch (operation)
				{
					case Operation.Plus:
						return (intLeft + intRight).ToString();
					case Operation.Minus:
						return (intLeft - intRight).ToString();
					case Operation.Multiply:
						return (intLeft * intRight).ToString();
					case Operation.Divide:
						return (intLeft / intRight).ToString();
					case Operation.Modulo:
						return (intLeft % intRight).ToString();
					case Operation.Pow:
						return ((int)Math.Pow(intLeft, intRight)).ToString();
					default:
						throw new NotImplementedException("Unknown Operation " + operation);
				}
			else if (float.TryParse(valLeftStr, out float floatLeft) && float.TryParse(valRightStr, out float floatRight))
				switch (operation)
				{
					case Operation.Plus:
						return (floatLeft + floatRight).ToString();
					case Operation.Minus:
						return (floatLeft - floatRight).ToString();
					case Operation.Multiply:
						return (floatLeft * floatRight).ToString();
					case Operation.Divide:
						return (floatLeft / floatRight).ToString();
					case Operation.Modulo:
						return (floatLeft % floatRight).ToString();
					case Operation.Pow:
						return ((int)Math.Pow(floatLeft, floatRight)).ToString();
					default:
						throw new NotImplementedException("Unknown Operation " + operation);
				}
			else if (valLeft.GetType() == valRight.GetType())
			{
				if (valLeft.GetType() == typeof(Vector))
				{
					switch (operation)
					{
						case Operation.Plus:
							return Vector.Add((Vector)valLeft, (Vector)valRight);
						case Operation.Minus:
							return Vector.Sub((Vector)valLeft, (Vector)valRight);
						case Operation.Multiply:
							return Vector.Mul((Vector)valLeft, (Vector)valRight);
						default:
							throw new ArgumentException("Vectors detected, but you can these only add, subtract and multiply!");
					}
				}
				else
					return Fallback(operation, valLeftStr, valRightStr);

			}
			else
				return Fallback(operation, valLeftStr, valRightStr);
		}

		private object Fallback(Operation operation, string left, string right)
		{

			if (operation == Operation.Plus)
				return left + right;
			else
				throw new ArgumentException("Strings detected, but you can these only add up!");

		}

		public override string ToString()
		{
			return left + " " + ToString(operation) + " " + right;
		}
	}

	public enum Operation
	{
		Plus,
		Minus,
		Multiply,
		Divide,
		Modulo,
		Pow
	}
}
