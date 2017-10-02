namespace QEngine
{
	public class QSprite : IQRenderable
	{
		public QVector2 Offset { get; set; } = QVector2.Zero;

		public QVector2 Origin { get; set; } = QVector2.Zero;

		public QRectangle Source { get; set; } = QRectangle.Empty;

		public QColor Color { get; set; } = QColor.White;

		public QVector2 Scale { get; set; } = QVector2.One;

		public float Layer { get; set; } = 0.5f;

		public bool Visible { get; set; } = true;

		public QRenderEffects Effect { get; set; } = QRenderEffects.None;

		public float Width => Source.Width * Scale.X;

		public float Height => Source.Height * Scale.Y;

		internal QTextureAtlas Texture => Script.World.TextureAtlas;

		public QBehavior Script { get; }

		public QSprite(QBehavior s, string textureName) : this(s)
		{
			Source = Texture[textureName];
			Origin = Source.Center;
		}

		public QSprite(QBehavior s, QRectangle source) : this(s)
		{
			Source = source;
			Origin = Source.Center;
		}

		public QSprite(QBehavior script)
		{
			Script = script;
			Source = QRectangle.Empty;
			Origin = QVector2.Zero;
		}
	}
}