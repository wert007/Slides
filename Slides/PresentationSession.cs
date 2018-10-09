using Slides.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides
{
	public class PresentationSession
	{
		Presentation presentation;
		public int Index { get; private set; }
		Slide[] slides;
		SlideSession currentSlide;

		public PresentationSession(Presentation presentation, Slide[] slides)
		{
			this.presentation = presentation;
			Index = 0;
			this.slides = slides;
			currentSlide = null;
		}

		public IEnumerable<Slide> IterateSlides()
		{
			return slides;
		}

		public Step RequestNext()
		{
			if (Index >= presentation.SlidesCount)
				return null;
			if(currentSlide == null)
			{
				currentSlide = slides[Index].CreateSession();
				slides[Index].Show();
			}
			var step = currentSlide.RequestNext();
			if (step == null)
			{
				Index++;
				currentSlide = null;
				return RequestNext();
			}
			step.Show();
			return step;
		}
	}

	public class SlideSession
	{
		Slide slide;
		public int Index { get; private set; }
		Step[] steps;

		public SlideSession(Slide slide, Step[] steps)
		{
			this.slide = slide;
			Index = 0;
			this.steps = steps;
		}

		public Step RequestNext()
		{
			if (Index >= slide.StepsCount)
				return null;
				Index++;			
			return steps[Index - 1];
		}
	}
}
