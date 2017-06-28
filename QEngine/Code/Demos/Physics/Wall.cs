using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
	class Wall : QCharacterController
	{
		QSprite sprite;

		QVec toPos;

		public override void OnLoad(QAddContent add)
		{
			add.Rectangle(Name, 40, Window.Height, QColor.White);
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
			World.CreateRectangle(this, sprite.Width, sprite.Height, 1, QBodyType.Static);
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