namespace QEngine.Demos
{
	class Floor : QCharacterController
	{
		QSprite sprite;

		QRigiBody body;

		QVec toPos;

		public override void OnLoad(QAddContent add)
		{
			add.Rectangle("Floor", Window.Width, 40, QColor.White);
			Transform.Position = toPos;
		}

		public override void OnStart(QGetContent getContent)
		{
			int r()
			{
				return QRandom.Range(1, 255);
			}
			sprite = new QSprite(this, "Floor");
			sprite.Color = new QColor(r(), r(), r());
			body = World.CreateRectangle(this, sprite.Width, sprite.Height, 1, Transform.Position, Transform.Rotation, QBodyType.Static);
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);
		}

		public Floor(QVec p) : base("Floor")
		{
			toPos = p;
		}
	}
}