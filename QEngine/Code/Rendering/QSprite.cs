namespace QEngine
{
	public class QSprite
	{
		public QVec Offset { get; set; } = QVec.Zero;

		public QVec Origin { get; set; } = QVec.Zero;

		public QRect Source { get; set; }

		public QColor Color { get; set; } = QColor.White;

		public float Layer { get; set; } = 0.5f;

		public QSpriteEffects Effect { get; set; } = QSpriteEffects.None;

		public bool IsVisible { get; set; } = true;

		/// <summary>
		/// returns the Source width * scale
		/// </summary>
		public float Width => Source.Width * script.Transform.Scale.X;

		/// <summary>
		/// Returns the source height * scale
		/// </summary>
		public float Height => Source.Height * script.Transform.Scale.Y;

		internal QMegaTexture Texture => script.Scene.MegaTexture;

		internal QBehavior script{ get; }

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
			this.script = script;
			Source = QRect.Empty;
			Origin = QVec.Zero;
		}
	}
}