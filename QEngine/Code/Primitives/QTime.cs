using Microsoft.Xna.Framework;

namespace QEngine
{
	public struct QTime
	{
		float delta { get; }

		public float Delta
		{
			get
			{
				if(delta > 1 / 10f)
					return 1 / 10f;
				return delta;
			}
		}

		public float Fps => 1 / delta;

		public bool Under60Fps { get; }

		public static implicit operator QTime(GameTime gt)
		{
			return new QTime(gt);
		}

		public QTime(GameTime gt)
		{
			delta = (float)gt.ElapsedGameTime.TotalSeconds;
			Under60Fps = delta > 1 / 60f;
		}
	}
}