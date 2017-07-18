namespace QEngine.Demos.PlatformingDemo
{
	public class DroppableItem : QBehavior, IQLoad, IQStart, IQDrawSprite
	{
		QSprite Sprite;

		QRigiBody Body;

		float PotionRadius = 11;

		public void OnLoad(QAddContent add)
		{
			add.Texture(Assets.Bryan + "BryanStuff1");
		}

		public void OnStart(QGetContent get)
		{
			Sprite = new QSprite(this, get.TextureSource("BryanStuff1").Split(32, 32)[7]);
			Sprite.Offset += new QVec(2, 0);
			Sprite.Scale = QVec.One * 2;
			Body = World.CreateCircle(this, PotionRadius, 5);
		}

		public void OnDrawSprite(QSpriteRenderer renderer)
		{
			renderer.Draw(Sprite, Transform);
		}
	}
}