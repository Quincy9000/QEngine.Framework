using QEngine.Prefabs;

namespace QEngine.Demos.QSnake
{
	class SnakeBody : QCharacterController
	{
		QSprite sprite;

		public QRect Collision = new QRect();

		public override void OnLoad(QAddContent add)
		{
			add.Rectangle("body", 40, 40, QColor.Gray);
		}

		public override void OnStart(QGetContent get)
		{
			sprite = new QSprite(this, "body");
		}

		public override void OnFixedUpdate(float time)
		{
			Collision = new QRect(Transform.Position - new QVec(sprite.Width / 2f - QVec.One.X * 4), new QVec(39));
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);
		}

		public SnakeBody() : base("SnakeBody") { }
	}
}
