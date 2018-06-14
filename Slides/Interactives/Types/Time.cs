using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Types
{
	public class CurrentTime
	{
		long milliseconds;
		long startTime;

		public CurrentTime()
		{
			milliseconds = 0;
			startTime = (long)TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;
		}

		public override bool Equals(object obj)
		{
			if (obj is CurrentTime ct)
				return ct.startTime == startTime;
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			var hashCode = -1519324383;
			hashCode = hashCode * -1521134295 + milliseconds.GetHashCode();
			hashCode = hashCode * -1521134295 + startTime.GetHashCode();
			return hashCode;
		}

		public override string ToString()
		{
			return milliseconds.ToString();
		}

		public void UpdateElapsed(int elapsedTime)
			=> milliseconds += elapsedTime;

		public void UpdateTime(long currentTime) 
			=> milliseconds = currentTime - startTime;

		public static bool operator ==(CurrentTime time1, CurrentTime time2)
		{
			return EqualityComparer<CurrentTime>.Default.Equals(time1, time2);
		}

		public static bool operator !=(CurrentTime time1, CurrentTime time2)
		{
			return !(time1 == time2);
		}

		public static implicit operator float(CurrentTime t) => t.milliseconds / 1000.0f;

		public static explicit operator CurrentTime(float f) => new CurrentTime();


	}
}
