﻿using System;

namespace QEngine
{
	/// <summary>
	/// Random number generator class
	/// </summary>
	public static class QRandom
	{
		/// <summary>
		/// Overriding the Next so its inclusive min and max
		/// </summary>
		class Rng : Random
		{
			public override int Next(int minValue, int maxValue)
			{
				return base.Next(minValue, maxValue + 1);
			}
		}

		static readonly Rng R = new Rng();

		/// <summary>
		/// rolls # of dice with n sides
		/// </summary>
		/// <param name="dice"></param>
		/// <param name="sides"></param>
		/// <returns></returns>
		public static int Dice(int dice, int sides)
		{
			int total = 0;
			for(int i = 0; i < dice; i++)
			{
				total += Number(1, sides);
			}
			return total;
		}

		public static QColor Color()
		{
			return new QColor(Number(1,255), Number(1, 255), Number(1,255));
		}

		/// <summary>
		/// Inclusive Random Number
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int Number(int min, int max) => R.Next(min, max);

		//		/// <summary>
		//		/// Modify the min range for random floats
		//		/// </summary>
		//		public static int minExponential = -64;
		//
		//		/// <summary>
		//		/// Modify the max range for random floats
		//		/// </summary>
		//		public static int maxExponential = 64;

		/// <summary>
		/// Inclusive QTime Random Number
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static float Number(float min, float max)
		{
			return (float)(R.NextDouble() * (max - min)) + min;
		}

		//		public static QTime Number(QTime min, QTime max) => (QTime)((R.NextDouble() * max - min) * (Math.Pow(2.0, Number(minExponential, maxExponential))));

		/// <summary>
		/// Returns a double between 0 and 1
		/// </summary>
		/// <returns></returns>
		public static double Percent() => R.NextDouble();
	}
}