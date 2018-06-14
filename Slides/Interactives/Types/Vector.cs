using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slides.Interactives.Types
{
	public class Vector
	{
		float x, y, z, w;

		public string X
		{
			get { return x.ToString(); }
			set { x = float.Parse(value); }
		}
		public string Y
		{
			get { return y.ToString(); }
			set { y = float.Parse(value); }
		}
		public string Z
		{
			get { return z.ToString(); }
			set { z = float.Parse(value); }
		}
		public string W
		{
			get { return w.ToString(); }
			set { w = float.Parse(value); }
		}

		public Vector() { }

		public Vector(string x, string y)
		{
			this.x = float.Parse(x);
			this.y = float.Parse(y);
		}

		public Vector(string x, string y, string z)
		{
			this.x = float.Parse(x);
			this.y = float.Parse(y);
			this.z = float.Parse(z);
		}

		public Vector(string x, string y, string z, string w)
		{
			this.x = float.Parse(x);
			this.y = float.Parse(y);
			this.z = float.Parse(z);
			this.w = float.Parse(w);
		}

		public Vector(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public float Length()
		{
			return (float)Math.Sqrt(x * x + y * y + z * z + w * w);
		}

		public void Add(Vector vector)
		{
			this.x += vector.x;
			this.y += vector.y;
			this.z += vector.z;
			this.w += vector.w;
		}

		public void Sub(Vector vector)
		{
			this.x -= vector.x;
			this.y -= vector.y;
			this.z -= vector.z;
			this.w -= vector.w;
		}

		public override string ToString()
		{
			return "X: " + x + " Y: " + y + " Z: " + z + " W: " + w;
		}

		public static Vector Add(Vector a, Vector b)
		{
			return new Vector(
				a.x + b.x,
				a.y + b.y,
				a.z + b.z,
				a.w + b.w);
		}

		public static Vector Sub(Vector a, Vector b)
		{
			return new Vector(
				a.x - b.x,
				a.y - b.y,
				a.z - b.z,
				a.w - b.w);
		}

		public static Vector Mul(Vector a, Vector b)
		{
			return new Vector(
				a.x * b.x,
				a.y * b.y,
				a.z * b.z,
				a.w * b.w);
		}
	}
}
