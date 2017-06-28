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
				if(delta > 1 / 4f)
				{
					IsLagging = true;
					return 1 / 4f;
				}
				else
				{
					return delta;
				}
			}
		}

		public float Fps => 1 / Delta;

		public bool IsLagging { get; private set; }

		public static implicit operator QTime(GameTime gt)
		{
			return new QTime(gt);
		}

		public QTime(GameTime gt)
		{
			delta = (float)gt.ElapsedGameTime.TotalSeconds;
			IsLagging = false;
		}
	}
}