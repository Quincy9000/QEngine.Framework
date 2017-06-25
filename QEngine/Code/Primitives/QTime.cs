using Microsoft.Xna.Framework;

namespace QEngine
{
	public struct QTime
	{
		readonly GameTime time;

		readonly float delta;

		public float Delta => delta > 1/4f ? 1/4f : delta;

		public float Fps => 1 / Delta;

		public static implicit operator QTime(GameTime gt)
		{
			return new QTime(gt);
		}

		public QTime(GameTime gt)
		{
			delta = (float)gt.ElapsedGameTime.TotalSeconds;
			time = gt;
		}

		public QTime(float f)
		{
			delta = f;
			time = null;
		}
	}
}