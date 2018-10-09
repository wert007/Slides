using Slides.Interactives.Commands;
using Slides.Interactives.Types;
using Slides.Patterns;
using Slides.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Slides.Components
{
	public class Slide : Step
	{
		List<Step> steps;
		public int CurrentStep { get; private set; }
		public int StepsCount => steps.Count;
		public new int LineLength => steps.Sum(s => s.LineLength) + 2;

		public IEnumerable<Step> IterateSteps()
		{
			return steps;
		}

		public new Presentation Parent { get; private set; }

		private void SetBackground(object value)
		{
			if (value is Color c)
				Background = c;
			else if (value is ImageSource i)
				Background = i;
		}

		public Slide(string name, Presentation parent) : base(name, null)
		{
			steps = new List<Step>();
			CurrentStep = 0;
			Parent = parent;
		}

		public void AddStep(Step step)
		{
			steps.Add(step);
		}

		public new void ApplyStyles(List<Variable> presentationVariables)
		{
			List<Variable> variables = GetStandardVariables(presentationVariables);
			foreach (var step in steps)
			{
				step.ApplyStyles(variables);
			}
			ComputeStandardVariables(variables);
		}

		public new void CollectVariables(List<Variable> presentationVariables)
		{
			List<Variable> variables = GetStandardVariables(presentationVariables);
			foreach (var step in steps)
			{
				step.CollectVariables(variables);
			}
			ComputeStandardVariables(variables);
		}

		public new void Run(List<Variable> presentationVariables)
		{
			List<Variable> variables = GetStandardVariables(presentationVariables);
			foreach (var step in steps)
			{
				variables.AddRange(step.Variables);
				step.Run(variables);
			}
			ComputeStandardVariables(variables);
		}

		private List<Variable> GetStandardVariables(List<Variable> presentationVariables)
		{
			List<Variable> result = new List<Variable>
			{
				new Variable("background", Background),
				new Variable("name", Name),
				new Variable("time", Time),
			};
			result.AddRange(presentationVariables);
			return result;
		}

		private void ComputeStandardVariables(List<Variable> variables)
		{
			SetBackground(variables.FirstOrDefault(v => v.Name == "background").Value);
		}

		public List<Element> GetAllElements()
		{
			List<Element> result = new List<Element>();
			foreach (var step in steps)
			{
				result.AddRange(step.GetElements());
			}
			return result;
		}

		public List<Element> GetElementsUntil(Step step)
		{
			return GetElementsUntil(steps.IndexOf(step));
		}

		public List<Element> GetElementsUntil(int index)
		{
			List<Element> result = new List<Element>();
			for (int i = 0; i < index + 1; i++)
			{
				result.AddRange(steps[i].GetElements());
			}
			return result;
		}

		public List<Variable> GetVariables()
		{
			List<Variable> result = new List<Variable>();
			for (int i = 0; i < CurrentStep; i++)
			{
				result.AddRange(steps[i].Variables);
			}
			return result;
		}

		public List<LoopingCommand> GetLoops()
		{
			List<LoopingCommand> result = new List<LoopingCommand>();
			for (int i = 0; i < CurrentStep; i++)
			{
				result.AddRange(steps[i].Loops);
			}
			return result;
		}

		public static bool TryScan(CodeReader reader, Presentation parent, out Slide slide)
		{
			slide = null;
			string line = reader.NextLine();
			if (!Regex.IsMatch(line, RegEx))
				return false;
			string name = Regex.Match(line, RegExHelper.String).Value.Trim('@', '\'');
			slide = new Slide(name, parent);
			line = reader.NextLine();
			Step normalStep = null;

			while (!reader.Done && !reader.EndingKeyword)
			{
				if (Step.TryScan(reader.Copy(), slide, out Step step))
				{
					if (normalStep != null)
						slide.AddStep(normalStep);
					normalStep = null;
					reader.Skip(step.LineLength - 1);
					slide.AddStep(step);
				}
				else
				{
					if (normalStep == null)
						normalStep = new Step("normal", slide);
					if (Command.TryScan(reader.Copy(), out Command command))
					{
						reader.Skip(command.Lines - 1);
						normalStep.Add(command);
					}
					else if (StaticFunctionCallCommand.TryScan(line, out StaticFunctionCallCommand functionCallCommand))
					{
						reader.Skip(functionCallCommand.Lines - 1);
						normalStep.Add(functionCallCommand);
					}

					else
						throw new ArgumentException("Unknown Command in Line " + reader.CurrentLine + ".");
				}
				line = reader.NextLine();

			}
			if (normalStep != null)
				slide.AddStep(normalStep);
			return true;
		}

		public SlideSession CreateSession()
		{
			return new SlideSession(this, steps.ToArray());
		}

		public static Slide Empty => null;
		public new static string RegEx => RegExHelper.Variable + " " + RegExHelper.String + ":";
	}
}