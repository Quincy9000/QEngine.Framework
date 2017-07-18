namespace QEngine
{
	public class QSprite
	{
		public QVec Offset { get; set; } = QVec.Zero;

		public QVec Origin { get; set; } = QVec.Zero;

		public QRect Source { get; set; }

		public QColor Color { get; set; } = QColor.White;

		public QVec Scale { get; set; } = QVec.One;

		public float Layer { get; set; } = 0.5f;

		public QSpriteEffects Effect { get; set; } = QSpriteEffects.None;

		public bool IsVisible { get; set; } = true;

		/// <summary>
		/// returns the Source width * scale
		/// </summary>
		public float Width => Source.Width * Scale.X;

		/// <summary>
		/// Returns the source height * scale
		/// </summary>
		public float Height => Source.Height * Scale.Y;

		internal QMegaTexture Texture => Script.Scene.MegaTexture;

		internal QBehavior Script { get; }

		public QSprite(QBehavior s, string textureName) : this(s)
		{
			Source = Texture[textureName];
			Origin = Source.Center;
		}

		public QSprite(QBehavior s, QRect source) : this(s)
		{
			Source = source;
			Origin = Source.Center;
		}

		public QSprite(QBehavior script)
		{
			Script = script;
			Source = QRect.Empty;
			Origin = QVec.Zero;
		}
	}
}