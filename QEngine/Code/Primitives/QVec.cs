using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using QPhysics.Utilities;

namespace QEngine
{
    /// <summary>
    /// QEngine vector class that adds extentions methods, its basically just a Monogame Vector2 but with more functions
    /// </summary>
    ///   
    public struct QVec
    {
        /*Privates*/

        Vector2 pos;

        /*Private Methods*/

        public bool Equals(QVec other)
        {
            return pos.Equals(other.pos);
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj)) return false;
            return obj is QVec && Equals((QVec) obj);
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

        public QVec Normalize()
        {
            return Normalize(pos);
        }

        public float LengthSquared()
        {
            return pos.LengthSquared();
        }

        /*Converts*/

        internal QVec ToSim()
        {
            return ConvertUnits.ToSimUnits(pos);
        }

        internal QVec ToDis()
        {
            return ConvertUnits.ToDisplayUnits(pos);
        }

        /*Operators*/

        public static QVec operator +(QVec left, QVec right)
        {
            return new QVec(left.pos + right.pos);
        }

        public static QVec operator -(QVec left, QVec right)
        {
            return new QVec(left.pos - right.pos);
        }

        public static QVec operator *(QVec left, QVec right)
        {
            return new QVec(left.pos * right.pos);
        }

        public static QVec operator *(QVec left, float right)
        {
            return new QVec(left.pos * right);
        }

        public static QVec operator /(QVec left, QVec right)
        {
            return new QVec(left.pos / right.pos);
        }

        public static QVec operator /(QVec left, float right)
        {
            return new QVec(left.pos / right);
        }

        public static bool operator ==(QVec left, QVec right)
        {
            return left.pos == right.pos;
        }

        public static bool operator !=(QVec left, QVec right)
        {
            return left.pos != right.pos;
        }

        /*Implicits*/

        public static implicit operator Vector2(QVec v)
        {
            return v.pos;
        }

        public static implicit operator QVec(Vector2 v)
        {
            return new QVec(v);
        }

        public static implicit operator Point(QVec v)
        {
            return v.pos.ToPoint();
        }

        public static implicit operator QVec(Point p)
        {
            return p.ToVector2();
        }

        /*Statics*/

        public static QVec Zero { get; }

        public static QVec One { get; }

        public static QVec Up { get; }

        public static QVec Down { get; }

        public static QVec Left { get; }

        public static QVec Right { get; }

        public static QVec Max(QVec left, QVec right)
        {
            return new QVec(Vector2.Max(left.pos, right.pos));
        }

        public static QVec Min(QVec left, QVec right)
        {
            return new QVec(Vector2.Min(left.pos, right.pos));
        }

        public static QVec Clamp(QVec value, QVec min, QVec max)
        {
            return new QVec(Vector2.Clamp(value.pos, min.pos, max.pos));
        }

        public static QVec Lerp(QVec from, QVec towards, float amount)
        {
            return new QVec(Vector2.Lerp(from.pos, towards.pos, amount));
        }

        public static QVec Normalize(QVec v)
        {
            return new QVec(Vector2.Normalize(v.pos));
        }

        public static float Distance(QVec first, QVec second)
        {
            return Vector2.Distance(first.pos, second.pos);
        }

        public static float DistanceSquared(QVec first, QVec second)
        {
            return Vector2.DistanceSquared(first.pos, second.pos);
        }

        public static QVec Transform(QVec v, QMat m)
        {
            return Vector2.Transform(v, m);
        }

        public static QVec Middle(QVec vec1, QVec vec2)
        {
            return new QVec((vec1.X + vec2.X) / 2f, (vec1.Y + vec2.Y) / 2f);
        }

        /// <summary>
        /// Moves the camera based on a pixel grid for pixel games!
        /// </summary>
        public static QVec PixelMove(QVec target, int pixelsPerUnit = 16)
        {
            var x = (float) (Math.Round(target.X * pixelsPerUnit) / pixelsPerUnit) - target.X;
            var y = (float) (Math.Round(target.Y * pixelsPerUnit) / pixelsPerUnit) - target.Y;
            return new QVec(x, y);
        }

        /// <summary>
        /// Returns the angle at which to turn to look at the target
        /// </summary>
        /// <param name="position"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float LookAt(QVec position, QVec target)
        {
            var angle = (float) Math.Atan2(target.Y - position.Y, target.X - position.X);
            return MathHelper.ToRadians(angle * (180 / (float) Math.PI));
        }

        /// <summary>
        /// Returns direction of vector that points towards another vector
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static QVec MoveTowards(QVec vec, QVec target)
        {
            Vector2 direction = target - vec;
            direction.Normalize();
            return direction;
        }

        /*Ctors*/

        public QVec(float x, float y)
        {
            pos.X = x;
            pos.Y = y;
        }

        internal QVec(Vector2 vec)
        {
            pos = vec;
        }

        /// <summary>
        /// Sets both x and y
        /// </summary>
        /// <param name="xy"></param>
        public QVec(float xy)
        {
            pos = new Vector2(xy);
        }

        static QVec()
        {
            /*Monogame reverses up and down axis*/
            Zero = new QVec(0, 0);
            One = new QVec(1, 1);
            Up = new QVec(0, -1);
            Down = new QVec(0, 1);
            Left = new QVec(-1, 0);
            Right = new QVec(1, 0);
        }
    }
}