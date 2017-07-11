using QPhysics.Utilities;

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
		
		public static int ToSim(this int f)
		{
			return (int)ConvertUnits.ToSimUnits(f);
		}

		public static int ToDis(this int f)
		{
			return (int)ConvertUnits.ToDisplayUnits(f);
		}
	}
}
