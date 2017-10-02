namespace QEngine
{
	public class QTransform
	{
		public QVector2 Position { get; set; }

		public float Rotation { get; set; }

		public void Reset()
		{
			Position = QVector2.Zero;
			Rotation = 0;
		}

		internal QTransform()
		{
			Reset();
		}
	}
}