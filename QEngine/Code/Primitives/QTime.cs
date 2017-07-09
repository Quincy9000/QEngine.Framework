using Microsoft.Xna.Framework;

namespace QEngine
{
	public struct QTime
	{
		float delta { get; }

		//if the delta is greater than 0.25 (4fps) then return 0.25 instead of delta
		public float Delta => delta < 0.25 ? delta : 0.25f;
//		{
//			get
//			{
//				if(delta > 0.25)
//					return 0.25f;
//				return delta;
//			}
//		}

		public float Fps => 1 / delta;

		public bool Under60Fps { get; }

		public static implicit operator QTime(GameTime gt)
		{
			return new QTime(gt);
		}

		internal QTime(GameTime gt)
		{
			delta = (float)gt.ElapsedGameTime.TotalSeconds;
			Under60Fps = delta > 1 / 60f;
		}

		internal QTime(float fixedTime)
		{
			delta = fixedTime;
			Under60Fps = delta > 1 / 60f;
		}
	}
}