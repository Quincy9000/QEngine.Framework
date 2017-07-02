using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo
{
	/// <summary>
	/// Platform for the player to stand on
	/// </summary>
	public class Platform : QCharacterController
	{
		QSprite sprite;

		QRigiBody body;

		QVec toSize;

		public static bool CheckDistance = true;

		public override void OnLoad(QAddContent add)
		{
			add.Rectangle(Name, (int)toSize.X, (int)toSize.Y, QColor.White);
		}

		public override void OnStart(QGetContent get)
		{
			int R() => QRandom.Number(1, 255);

			sprite = new QSprite(this, Name);
			sprite.Color = new QColor(R(), R(), R());
			body = World.CreateRectangle(this, sprite.Width, sprite.Height, 1, QBodyType.Static);
			body.Friction = 0.2f;
			body.IsCCD = false;
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			if(CheckDistance || QVec.Distance(Position, Camera.Position) < 1000)
				spriteRenderer.Draw(sprite, Transform);
		}

		public Platform(QVec s)
		{
			toSize = s;
		}
	}
}