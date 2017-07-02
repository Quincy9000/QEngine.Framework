using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
	class Ball : QCharacterController
	{
		QRigiBody body;

		QColor c;

		float radius;

		public Ball(float diameter)
		{
			radius = diameter / 2f;
		}

		public override void OnStart(QGetContent get)
		{
			int R() => QRandom.Number(100, 255);
			body = World.CreateCircle(this, radius, 20);
			c = new QColor(R(), R(), R());
			//body.Restitution = 1f;
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			//spriteRenderer.Draw(sprite, Transform);
			spriteRenderer.DrawCircle(Transform, radius, c);
		}
	}
}