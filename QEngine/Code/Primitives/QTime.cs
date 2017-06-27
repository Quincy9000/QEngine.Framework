using Microsoft.Xna.Framework;

namespace QEngine
{
	public struct QTime
	{
		float delta { get; }

		public float Delta => delta > 1 / 4f ? 1 / 4f : delta;

		public float Fps => 1 / Delta;

		public bool IsLagging { get; }

		public static implicit operator QTime(GameTime gt)
		{
			return new QTime(gt);
		}

		public QTime(GameTime gt)
		{
			delta = (float)gt.ElapsedGameTime.TotalSeconds;
			IsLagging = gt.IsRunningSlowly;
		}
	}
}