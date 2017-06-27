using QEngine.Prefabs;

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
			add.Rectangle(Name, (int)toSize.X, (int)toSize.Y, QColor.White);
			Transform.Position = toPos;
		}

		public override void OnStart(QGetContent get)
		{
			int r()
			{
				return QRandom.Range(1, 255);
			}

			sprite = new QSprite(this, Name);
			sprite.Color = new QColor(r(), r(), r());
			body = World.CreateRectangle(this, sprite.Width, sprite.Height, 1, QBodyType.Static);
			body.Friction = 0.2f;
			body.IsIgnoreCcd = false;
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