using System;
using System.Collections.Generic;
using QEngine.Physics.Utilities;

namespace QEngine
{
	public static class QExtentions
	{
		/*Floats*/

		public static float ToSim(this float f)
		{
			return ConvertUnits.ToSimUnits(f);
		}

		public static float ToDis(this float f)
		{
			return ConvertUnits.ToDisplayUnits(f);
		}
		
		/*Ints*/
		
		public static float ToSim(this int f)
		{
			return ConvertUnits.ToSimUnits(f);
		}

		public static float ToDis(this int f)
		{
			return ConvertUnits.ToDisplayUnits(f);
		}

		/// <summary>
		/// Easier for loop method, nothing special, less typing and uses for-loop internally
		/// </summary>
		/// <param name="list"></param>
		/// <param name="action"></param>
		/// <typeparam name="T"></typeparam>
		public static void For<T>(this IList<T> list, Action<T> action)
		{
			for (int i = 0; i < list.Count; i++)
			{
				action(list[i]);
			}
		}

		/// <summary>
		/// Easier for loop method, nothing special, less typing and uses for-loop internally
		/// </summary>
		/// <param name="array"></param>
		/// <param name="action"></param>
		/// <typeparam name="T"></typeparam>
		public static void For<T>(this T[] array, Action<T> action)
		{
			for (int i = 0; i < array.Length; i++)
			{
				action(array[i]);
			}
		}
	}
}
