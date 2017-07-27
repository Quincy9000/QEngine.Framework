namespace QEngine
{
	/// <summary>
	/// Visible object that can be rendered on the screen
	/// </summary>
	public class QImage : IQRenderable
	{
		public QBehavior Script { get; }
		public QVec Offset { get; set; } = QVec.Zero;
		public QVec Origin { get; set; } = QVec.Zero;
		public QRect Source { get; set; } = QRect.Empty;
		public QColor Color { get; set; } = QColor.White;
		public QVec Scale { get; set; } = QVec.One;
		public float Layer { get; set; } = 0;
		public bool Visible { get; set; } = true;
		public QRenderEffects Effect { get; set; } = QRenderEffects.None;

		internal QAtlas Texture => Script.Scene.Atlas;

		public QImage(QBehavior s, string textureName) : this(s)
		{
			Source = s.Scene.Atlas[textureName];
			Origin = Source.Center;
		}

		public QImage(QBehavior s, QRect source) : this(s)
		{
			Source = source;
			Origin = Source.Center;
		}

		public QImage(QBehavior script)
		{
			Script = script;
		}
	}
}