namespace QEngine
{
	/// <summary>
	/// Visible object that can be rendered on the screen
	/// </summary>
	public class QImage : IQRenderable
	{
		public QBehavior Script { get; }
		public QVector2 Offset { get; set; } = QVector2.Zero;
		public QVector2 Origin { get; set; } = QVector2.Zero;
		public QRectangle Source { get; set; } = QRectangle.Empty;
		public QColor Color { get; set; } = QColor.White;
		public QVector2 Scale { get; set; } = QVector2.One;
		public float Layer { get; set; } = 0;
		public bool Visible { get; set; } = true;
		public QRenderEffects Effect { get; set; } = QRenderEffects.None;
		public QTexture Texture { get; }

		public QImage(QBehavior s, string textureName) : this(s)
		{
			Source = s.World.Content.Atlases[textureName].
			Origin = Source.Center;
		}

		public QImage(QBehavior s, QRectangle source) : this(s)
		{
			Source = source;
			Origin = Source.Center;
		}

		public QImage(QBehavior script)
		{
			Script = script;
			Texture = null;
		}
	}
}