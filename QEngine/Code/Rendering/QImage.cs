namespace QEngine
{
	public class QImage : QSprite
	{
		public QImage(QBehavior s, string textureName) : base(s, textureName) { }

		public QImage(QBehavior s, QRect source) : base(s, source) { }
	}
}