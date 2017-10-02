using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QEngine
{
	/// <summary>
	/// Wrapper for the rectangle class in monogame
	/// </summary>
	public struct QRectangle
	{
		/*Privates*/

		Rectangle _rect;

		/*Properties*/

		public QVector2 Position
		{
			get => _rect.Location;
			set => _rect.Location = value;
		}

		public QVector2 Size => _rect.Size;

		public int X => _rect.X;

		public int Y => _rect.Y;

		public int Width => _rect.Width;

		public int Height => _rect.Height;

		public int Bottom => _rect.Bottom;

		public int Left => _rect.Left;

		public int Right => _rect.Right;

		public int Top => _rect.Top;

		public QVector2 Center => new Vector2(Width / 2f, Height / 2f);

		public bool IsEmpty => _rect.IsEmpty;

		public bool Contains(QVector2 v)
		{
			return _rect.Contains((Vector2)v);
		}

		public bool Intersects(QRectangle r)
		{
			return _rect.Intersects(r);
		}

		public void Inflate(QVector2 v)
		{
			_rect.Inflate(v.X, v.Y);
		}

		public void Offset(QVector2 v)
		{
			_rect.Offset((Vector2)v);
		}

		public QVector2 TopLeft()
		{
			return new QVector2(Left, Top);
		}

		public QVector2 TopRight()
		{
			return new QVector2(Right, Top);
		}

		public QVector2 BottomLeft()
		{
			return new QVector2(Left, Bottom);
		}

		public QVector2 BottomRight()
		{
			return new QVector2(Right, Bottom);
		}

		/// <summary>
		/// splits rectangle into an array of smaller rectangles, that are the dimention of the width and height
		/// Great for textures that are spritesheets, as long as you follow a convention
		/// if this rect is a sprite sheet
		/// </summary>
		/// <param name="rec"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <returns></returns>
		public List<QRectangle> Split(int w, int h)
		{
			var rects = new List<QRectangle>();

			for(var i = 0; i < Height / h; i++)
			{
				for(var j = 0; j < Width / w; j++)
				{
					rects.Add(new QRectangle(X + j * w, Y + i * h, w, h));
				}
			}
			return rects;
		}

		/// <summary>
		/// Get the textureSource from a spritesheet with uneven cells for sprites
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public QRectangle RelativeSource(int x, int y, int width, int height)
		{
			return new QRectangle(X + x, Y + y, width, height);
		}

		/*Implicits*/

		public static implicit operator Rectangle(QRectangle r)
		{
			return r._rect;
		}

		public static implicit operator QRectangle(Rectangle r)
		{
			return new QRectangle(r);
		}

		/*Operators*/

		public static bool operator ==(QRectangle l, QRectangle r)
		{
			return l._rect == r._rect;
		}

		public static bool operator !=(QRectangle l, QRectangle r)
		{
			return l._rect != r._rect;
		}

		public bool Equals(QRectangle other)
		{
			return _rect.Equals(other._rect);
		}

		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			return obj is QRectangle && Equals((QRectangle)obj);
		}

		public override int GetHashCode()
		{
			return _rect.GetHashCode();
		}

		public static QRectangle operator *(QRectangle r, int scaleSize)
		{
			return new QRectangle(r.X, r.Y, r.Width * scaleSize, r.Height * scaleSize);
		}

		/*Ctors*/

		internal QRectangle(Rectangle r)
		{
			_rect = r;
		}

		public QRectangle(QVector2 v, int w, int h) : this((int)v.X, (int)v.Y, w, h) { }

		public QRectangle(int x, int y, QVector2 size) : this(x, y, (int)size.X, (int)size.Y) { }

		public QRectangle(QVector2 v, QVector2 v2) : this((int)v.X, (int)v.Y, (int)v2.X, (int)v2.Y) { }

		public QRectangle(int x, int y, int w, int h)
		{
			_rect = new Rectangle(x, y, w, h);
		}

		public static QRectangle Empty { get; }

		static QRectangle()
		{
			Empty = new QRectangle(0, 0, 0, 0);
		}
	}
}