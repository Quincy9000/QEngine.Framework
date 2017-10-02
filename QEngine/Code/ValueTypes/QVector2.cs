using System;
using Microsoft.Xna.Framework;
using QEngine.Physics.Utilities;

namespace QEngine
{
	/// <summary>
	/// QEngine vector class that adds extentions methods, its basically just a Monogame Vector2 but with more functions
	/// </summary>
	public struct QVector2
	{
		/*Privates*/

		Vector2 pos;

		/*Private Methods*/

		public bool Equals(QVector2 other)
		{
			return pos.Equals(other.pos);
		}

		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			return obj is QVector2 && Equals((QVector2)obj);
		}

		public override int GetHashCode()
		{
			return pos.GetHashCode();
		}

		public override string ToString()
		{
			return $"{{X:{X}, Y:{Y}}}";
		}

		/*Properties*/

		public float X
		{
			get => pos.X;
			set => pos.X = value;
		}

		public float Y
		{
			get => pos.Y;
			set => pos.Y = value;
		}

		public QVector2 Normalize()
		{
			return Normalize(pos);
		}

		public float LengthSquared()
		{
			return pos.LengthSquared();
		}

		/*Converts*/

		internal QVector2 ToSim()
		{
			return ConvertUnits.ToSimUnits(pos);
		}

		internal QVector2 ToDis()
		{
			return ConvertUnits.ToDisplayUnits(pos);
		}

		/*Operators*/

		public static QVector2 operator +(QVector2 left, QVector2 right)
		{
			return new QVector2(left.pos + right.pos);
		}

		public static QVector2 operator -(QVector2 left, QVector2 right)
		{
			return new QVector2(left.pos - right.pos);
		}

		public static QVector2 operator *(QVector2 left, QVector2 right)
		{
			return new QVector2(left.pos * right.pos);
		}

		public static QVector2 operator *(QVector2 left, float right)
		{
			return left.pos * right;
		}

		public static QVector2 operator /(QVector2 left, QVector2 right)
		{
			return new QVector2(left.pos / right.pos);
		}

		public static QVector2 operator /(QVector2 left, float right)
		{
			return new QVector2(left.pos / right);
		}

		public static bool operator ==(QVector2 left, QVector2 right)
		{
			return left.pos == right.pos;
		}

		public static bool operator !=(QVector2 left, QVector2 right)
		{
			return left.pos != right.pos;
		}

		/*Implicits*/

		public static implicit operator Vector2(QVector2 v)
		{
			return v.pos;
		}

		public static implicit operator QVector2(Vector2 v)
		{
			return new QVector2(v);
		}

		public static implicit operator Point(QVector2 v)
		{
			return v.pos.ToPoint();
		}

		public static implicit operator QVector2(Point p)
		{
			return p.ToVector2();
		}

		/*Statics*/

		public static QVector2 Zero { get; }

		public static QVector2 One { get; }

		public static QVector2 Up { get; }

		public static QVector2 Down { get; }

		public static QVector2 Left { get; }

		public static QVector2 Right { get; }

		public static QVector2 Max(QVector2 left, QVector2 right)
		{
			return new QVector2(Vector2.Max(left.pos, right.pos));
		}

		public static QVector2 Min(QVector2 left, QVector2 right)
		{
			return new QVector2(Vector2.Min(left.pos, right.pos));
		}

		public static QVector2 Clamp(QVector2 value, QVector2 min, QVector2 max)
		{
			return new QVector2(Vector2.Clamp(value.pos, min.pos, max.pos));
		}

		public static QVector2 Lerp(QVector2 from, QVector2 towards, float amount)
		{
			return new QVector2(Vector2.Lerp(from.pos, towards.pos, amount));
		}

		public static QVector2 Normalize(QVector2 v)
		{
			return new QVector2(Vector2.Normalize(v.pos));
		}

		public static float Distance(QVector2 first, QVector2 second)
		{
			return Vector2.Distance(first.pos, second.pos);
		}

		public static float DistanceSquared(QVector2 first, QVector2 second)
		{
			return Vector2.DistanceSquared(first.pos, second.pos);
		}

		public static QVector2 Transform(QVector2 v, QMatrix m)
		{
			return Vector2.Transform(v, m);
		}

		public static QVector2 Middle(QVector2 vec1, QVector2 vector2)
		{
			return new QVector2((vec1.X + vector2.X) / 2f, (vec1.Y + vector2.Y) / 2f);
		}

		/// <summary>
		/// Moves the camera based on a pixel grid for pixel games!
		/// </summary>
		public static QVector2 PixelMove(QVector2 target, int pixelsPerUnit = 16)
		{
			var x = (float)(Math.Round(target.X * pixelsPerUnit) / pixelsPerUnit) - target.X;
			var y = (float)(Math.Round(target.Y * pixelsPerUnit) / pixelsPerUnit) - target.Y;
			return new QVector2(x, y);
		}

		/// <summary>
		/// Returns the angle at which to turn to look at the target
		/// </summary>
		/// <param name="position"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static float LookAt(QVector2 position, QVector2 target)
		{
			var angle = (float)Math.Atan2(target.Y - position.Y, target.X - position.X);
			return MathHelper.ToRadians(angle * (180 / (float)Math.PI));
		}

		/// <summary>
		/// Returns direction of vector that points towards another vector
		/// </summary>
		/// <param name="vector2"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static QVector2 MoveTowards(QVector2 vector2, QVector2 target)
		{
			Vector2 direction = target - vector2;
			direction.Normalize();
			return direction;
		}

		/*Ctors*/

		public QVector2(float x, float y)
		{
			pos.X = x;
			pos.Y = y;
		}

		internal QVector2(Vector2 vec)
		{
			pos = vec;
		}

		/// <summary>
		/// Sets both x and y
		/// </summary>
		/// <param name="xy"></param>
		public QVector2(float xy)
		{
			pos = new Vector2(xy);
		}

		static QVector2()
		{
			/*Monogame reverses up and down axis*/
			Zero = new QVector2(0, 0);
			One = new QVector2(1, 1);
			Up = new QVector2(0, -1);
			Down = new QVector2(0, 1);
			Left = new QVector2(-1, 0);
			Right = new QVector2(1, 0);
		}
	}
}