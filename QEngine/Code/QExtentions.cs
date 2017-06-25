using System;
using VelcroPhysics.Utilities;

namespace QEngine
{
	public static class QExtentions
	{
		public static T ThrowIfNull<T>(this T obj)
		{
			if(obj == null)
				throw new NullReferenceException($"{nameof(obj)} is null!");
			return (T)obj;
		}

		public static float ToSim(this float f)
		{
			return ConvertUnits.ToSimUnits(f);
		}

		public static float ToPixel(this float f)
		{
			return ConvertUnits.ToDisplayUnits(f);
		}
	}
}
