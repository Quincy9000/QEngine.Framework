namespace QEngine
{
	public class QSprite
	{
		public QVec Offset { get; set; } = QVec.Zero;

		public QVec Origin { get; set; } = QVec.Zero;

		public QRect Source { get; set; }

		public QColor Color { get; set; } = QColor.White;

		public QLayer Layer { get; set; } = 0.5f;

		public QSpriteEffects Effect { get; set; } = QSpriteEffects.None;

		public bool IsVisible { get; set; } = true;

		public float Width => Source.Width;

		public float Height => Source.Height;

		internal QMegaTexture Texture => script.Scene.MegaTexture;

		internal QBehavior script{ get; }

		public QSprite(QBehavior s, string textureName)
		{
			script = s;
			Source = Texture[textureName];
			Origin = Source.Center;
		}

		public QSprite(QBehavior s, QRect source)
		{
			script = s;
			Source = source;
			Origin = Source.Center;
		}
	}
}