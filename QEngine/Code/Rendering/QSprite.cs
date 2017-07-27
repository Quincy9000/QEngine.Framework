namespace QEngine
{
	public class QSprite : IQRenderable
	{
		public QVec Offset { get; set; } = QVec.Zero;

		public QVec Origin { get; set; } = QVec.Zero;

		public QRect Source { get; set; } = QRect.Empty;

		public QColor Color { get; set; } = QColor.White;

		public QVec Scale { get; set; } = QVec.One;

		public float Layer { get; set; } = 0.5f;

		public bool Visible { get; set; } = true;

		public QRenderEffects Effect { get; set; } = QRenderEffects.None;

		public float Width => Source.Width * Scale.X;

		public float Height => Source.Height * Scale.Y;

		internal QAtlas Texture => Script.Scene.Atlas;

		public QBehavior Script { get; }

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