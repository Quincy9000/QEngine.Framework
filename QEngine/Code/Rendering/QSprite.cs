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

		/// <summary>
		/// returns the Source width * scaleX
		/// </summary>
		public float Width => Source.Width * script.Transform.Scale.X;

		/// <summary>
		/// Returns the source height * scaley
		/// </summary>
		public float Height => Source.Height * script.Transform.Scale.Y;

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