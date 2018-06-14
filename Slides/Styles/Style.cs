using Slides.Components;
using Slides.Interactives.Commands;
using Slides.Interactives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Slides.Styles
{
	public class Style : IScanable
	{
		public string Name { get; private set; }
		List<IRunable> lines;

		public static List<Style> styles;

		public Style(string name)
		{
			Name = name;
			lines = new List<IRunable>();
			if (styles == null)
				styles = new List<Style>();
			styles.Add(this);
		}

		public void AddInstruction(Instruction instruction)
		{
			lines.Add(instruction);
		}

		public void AddCommand(Command command)
		{
			lines.Add(command);
		}

		public void Apply(Element element)
		{
			Type type = element.GetType();
			foreach (var line in lines)
			{
				if (line is Instruction instruction)
				{
					if(instruction.Property.Contains('.'))
					{
						string childType = instruction.Property.Split('.')[0];
						string childProp = instruction.Property.Split('.')[1];
						if(element is Slide slide)
						{
							switch (childType)
							{
								case "children":
									foreach (var child in slide.GetAllElements())
									{
										var childProperty = child.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == childProp.ToLower());
										if (childProperty != null)
											childProperty.SetValue(child, ReflectionHelper.Cast(childProperty.PropertyType, instruction.Run(child)));
									}
									break;
								case "label":
									foreach (var child in slide.GetAllElements().Where(l => l is Label))
									{
										var childProperty = child.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == childProp.ToLower());
										if (childProperty != null)
											childProperty.SetValue(child, ReflectionHelper.Cast(childProperty.PropertyType, instruction.Run(child)));
									}
									foreach (Stack stck in slide.GetAllElements().Where(s => s is Stack))
									{
										foreach (var child in stck.Content.Children.Where(l => l is Label))
										{
											var childProperty = child.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == childProp.ToLower());
											if (childProperty != null)
												childProperty.SetValue(child, ReflectionHelper.Cast(childProperty.PropertyType, instruction.Run(child)));
										}
									}
									break;
								default:
									throw new NotImplementedException("No Keyword named " + childType + " found. Instruction was: " + instruction.Property + ": " + instruction.Value);
							}
						}
					}
					var property = type.GetProperties().FirstOrDefault(p => p.Name.ToLower() == instruction.Property.ToLower());
					if(property != null)
						property.SetValue(element, ReflectionHelper.Cast(property.PropertyType, instruction.Run(element)));
				}
				else if(line is Command command)
				{
					command.Run(new List<Variable>()
					{
						new Variable("slide", element)
					});
				}
			}
		}

		public static Style GetByName(string name)
		{
			return styles.FirstOrDefault(p => p.Name == name);
		}

		public static bool TryScan(CodeReader reader, out Style scanned)
		{
			scanned = null;
			string line = reader.NextLine();
			if (!Regex.IsMatch(line, RegEx))
				return false;
			string name = Regex.Match(line, @"(std:|" + RegExHelper.Variable + @"\()").Value;
			name = name.Remove(name.Length - 1);
			scanned = new Style(name);
			line = reader.NextLine();
			while (!reader.Done && !reader.EndingKeyword)
			{
				if (Instruction.TryScan(reader.Copy(), out Instruction instruction))
				{
					scanned.AddInstruction(instruction);
					line = reader.NextLine();
				}
				else if (StaticFunctionCallCommand.TryScan(line, out StaticFunctionCallCommand command))
				{
					scanned.AddCommand(command);
					line = reader.NextLine();
				}
				else
					throw new Exception("Unkwon Command in Line " + reader.CurrentLine + ".");
			}
			return true;
		}

		public static Style Empty => null;

		public int LineLength => lines.Count + 2;

		public static string RegEx => @"style (std|" + RegExHelper.Function + "):";
	}
}