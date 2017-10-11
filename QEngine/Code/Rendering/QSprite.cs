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

		public QTexture Texture { get; }

		public QBehavior Script { get; }

		public QSprite(QBehavior script)
		{
			Script = script;
			Source = QRectangle.Empty;
			Origin = QVector2.Zero;
			Texture = null;
		}

		public QSprite(QBehavior script, string textureName) : this(script)
		{
			foreach (var atlas in Script.World.Content.Atlases)
			{
				if (atlas.Rectangles.TryGetValue(textureName, out QRectangle rect))
				{
					Texture = atlas;
					Source = rect;
				}
			}
		}

		public QSprite(QBehavior script, QRectangle source) : this(script)
		{
			foreach (var atlas in Script.World.Content.Atlases)
			{
				foreach (var rects in atlas.Rectangles)
				{
					foreach (var qRectangle in rects.Value.Split(source.Width, source.Height))
					{
						if (source == qRectangle)
						{
							Source = source;
							Texture = atlas;
							Origin = Source.Center;
						}
					}
				}
			}
		}
	}
}