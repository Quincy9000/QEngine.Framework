using QEngine;

namespace QEngine.Demos.PlatformingDemo.Scripts
{
	/// <summary>
	/// Platform for the player to stand on
	/// </summary>
	public class Platform : QCharacterController
	{
		QSprite sprite;

		QRigiBody body;

		QVec toPos;

		QVec toSize;

		public override void OnLoad(QAddContent add)
		{
			add.Rectangle("Floor", (int)toSize.X, (int)toSize.Y, QColor.White);
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

		public Platform(QVec p, QVec s) : base("Floor")
		{
			toPos = p;
			toSize = s;
		}
	}
}