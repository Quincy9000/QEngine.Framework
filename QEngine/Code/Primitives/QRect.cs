using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QEngine
{
	/// <summary>
	/// Wrapper for the rectangle class in monogame
	/// </summary>
	public struct QRect
	{
		/*Privates*/

		Rectangle rect;

		/*Properties*/

		public QVec Position
		{
			get => rect.Location;
			set => rect.Location = value;
		}

		public QVec Size => rect.Size;

		public int X => rect.X;

		public int Y => rect.Y;

		public int Width => rect.Width;

		public int Height => rect.Height;

		public int Bottom => rect.Bottom;

		public int Left => rect.Left;

		public int Right => rect.Right;

		public int Top => rect.Top;

		public QVec Center => new Vector2(Width / 2f, Height / 2f);

		public bool IsEmpty => rect.IsEmpty;

		public bool Contains(QVec v)
		{
			return rect.Contains((Vector2)v);
		}

		public bool Intersects(QRect r)
		{
			return rect.Intersects(r);
		}

		public void Inflate(QVec v)
		{
			rect.Inflate(v.X, v.Y);
		}

		public void Offset(QVec v)
		{
			rect.Offset((Vector2)v);
		}

		public QVec TopLeft()
		{
			return new QVec(Left, Top);
		}

		public QVec TopRight()
		{
			return new QVec(Right, Top);
		}

		public QVec BottomLeft()
		{
			return new QVec(Left, Bottom);
		}

		public QVec BottomRight()
		{
			return new QVec(Right, Bottom);
		}

		/// <summary>
		/// splits rectangle into an array of smaller rectangles, that are the dimention of the width and height
		/// Great for textures that are spritesheets, as long as you follow a convention
		/// </summary>
		/// <param name="rec"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <returns></returns>
		public List<QRect> Split(int w, int h)
		{
			var rects = new List<QRect>();

			for(var i = 0; i < Height / h; i++)
			for(var j = 0; j < Width / w; j++)
				rects.Add(new QRect(X + j * w, Y + i * h, w, h));

			return rects;
		}

		/*Implicits*/

		public static implicit operator Rectangle(QRect r)
		{
			return r.rect;
		}

		public static implicit operator QRect(Rectangle r)
		{
			return new QRect(r);
		}

		/*Operators*/

		public static bool operator ==(QRect l, QRect r)
		{
			return l.rect == r.rect;
		}

		public static bool operator !=(QRect l, QRect r)
		{
			return l.rect != r.rect;
		}
		
		public static QRect operator *(QRect r, int scaleSize)
		{
			return new QRect(r.X, r.Y, r.Width * scaleSize, r.Height * scaleSize);
		}

		/*Ctors*/

		internal QRect(Rectangle r)
		{
			rect = r;
		}

		public QRect(QVec v, int w, int h) : this((int)v.X, (int)v.Y, w, h)
		{
			
		}

		public QRect(int x, int y, QVec size) : this(x,y,(int)size.X, (int)size.Y)
		{
			
		}

		public QRect(QVec v, QVec v2) : this((int)v.X, (int)v.Y, (int)v2.X, (int)v2.Y)
		{
			
		}

		public QRect(int x, int y, int w, int h)
		{
			rect = new Rectangle(x, y, w, h);
		}

		public static QRect Empty { get; }

		static QRect()
		{
			Empty = new QRect(0, 0, 0, 0);
		}
	}
}