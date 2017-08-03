using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
	class Ball : QCharacterController
	{
		QRigidBody body;

		QVec DirectionOfMovement;

		QSprite Sprite;

		int Radius;

		public Ball(int diameter, QVec direction)
		{
			Radius = diameter / 2;
			DirectionOfMovement = direction;
		}

		public override void OnLoad(QAddContent add)
		{
			add.Circle(Name, Radius);
		}

		public override void OnStart(QGetContent get)
		{
			Sprite = new QSprite(this, get.TextureSource(Name));
			Sprite.Color = QRandom.Color();
			body = Physics.CreateCircle(this, Radius);
			body.ApplyForce(DirectionOfMovement);
			body.IsBullet = true;
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(Sprite, Transform);
		}
	}
}