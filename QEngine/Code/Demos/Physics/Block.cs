using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
	class Block : QCharacterController
	{
		QRigiBody body;

		public float Width { get; }

		public float Height { get; }

		QSprite sprite;

		QVec DirectionOfMovement;

		public Block(int w, int h, QVec direction)
		{
			Width = w;
			Height = h;
			DirectionOfMovement = direction;
		}

		public override void OnLoad(QAddContent add)
		{
			add.Rectangle(Name, (int)Width, (int)Height, QColor.White);
		}

		public override void OnStart(QGetContent get)
		{
			sprite = new QSprite(this, get.TextureSource(Name));
			sprite.Color = QRandom.Color();
			body = World.CreateRectangle(this, Width, Height);
			body.ApplyForce(DirectionOfMovement);
			body.IsBullet = true;
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);
		}
	}
}