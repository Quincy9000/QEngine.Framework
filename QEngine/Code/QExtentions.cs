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
	}
}
