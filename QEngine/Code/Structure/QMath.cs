using System;
using Microsoft.Xna.Framework;

namespace QEngine
{
	/// <summary>
	/// Static math class 
	/// </summary>
	public static class QMath
	{
		public static float E => MathHelper.E;

		public static float Pi => MathHelper.Pi;

		public static float PiOver2 => MathHelper.PiOver2;

		public static float PiOver4 => MathHelper.PiOver4;

		/// <summary>
		/// Tau or TwoPi
		/// </summary>
		public static float Tau => MathHelper.TwoPi;

		public static float Min(float one, float two) => MathHelper.Min(one, two);

		public static int Min(int one, int two) => MathHelper.Min(one, two);

		public static float Max(float one, float two) => MathHelper.Max(one, two);

		public static int Max(int one, int two) => MathHelper.Max(one, two);

		public static float ToDegrees(float radians) => MathHelper.ToDegrees(radians);

		public static float ToRadians(float degrees) => MathHelper.ToRadians(degrees);

		public static float Clamp(float value, float min, float max) => MathHelper.Clamp(value, min, max);

		public static float Clamp(int value, int min, int max) => MathHelper.Clamp(value, min, max);

		public static QVector2 Clamp(QVector2 value, QVector2 min, QVector2 max) => QVector2.Clamp(value, min, max);

		public static bool IsPowerOfTwo(int num) => MathHelper.IsPowerOfTwo(num);

		public static float Distance(float a, float b) => MathHelper.Distance(a, b);

		public static float SmoothStep(float value1, float value2, float amount) => MathHelper.SmoothStep(value1, value2, amount);

		public static float Lerp(float value1, float value2, float amount) => MathHelper.Lerp(value1, value2, amount);

		public static float LerpPrecise(float value1, float value2, float amount) => MathHelper.LerpPrecise(value1, value2, amount);

		public static float WrapAngle(float angle) => MathHelper.WrapAngle(angle);
	}
}