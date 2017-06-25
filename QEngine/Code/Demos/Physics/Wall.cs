namespace QEngine.Demos
{
	class Wall : QCharacterController
	{
		QSprite sprite;

		QVec toPos;

		public override void OnLoad(QAddContent add)
		{
			add.Rectangle("Wall", 40, Window.Height, QColor.White);
			Transform.Position = toPos;
		}

		public override void OnStart(QGetContent get)
		{
			int r()
			{
				return QRandom.Range(1, 255);
			}
			sprite = new QSprite(this, "Wall");
			sprite.Color = new QColor(r(), r(), r());
			World.CreateRectangle(this, sprite.Width, sprite.Height, 1, Transform.Position, Transform.Rotation, QBodyType.Static);
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);
		}

		public Wall(QVec q) : base("Wall")
		{
			toPos = q;
		}
	}
}