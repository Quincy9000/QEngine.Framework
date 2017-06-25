using QEngine;
using QEngine.Prefabs;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class CavernWalls : QCharacterController
	{
		public CavernWalls() : base("CavernWalls") { }

		QRigiBody body;

		public override void OnStart(QGetContent content)
		{
			Sprite = new QSprite(this, Scene.MegaTexture["cavern_biome"].Split(16, 16)[1]);
			Sprite.Color = QColor.DarkKhaki;
			Transform.Scale = new QVec(4);

			body = World.CreateRectangle(this, 15 * 4, 15 * 4, 1f, Transform.Position, 0, QBodyType.Static);
			body.Friction = 0;
		}

		public override void OnDrawSprite(QSpriteRenderer renderer)
		{
//			if(Vector2.Distance(Position, MainCamera.Position) < Scene.WindowWidth)
//				DrawSource(renderer);
			renderer.Draw(Sprite, Transform);
		}
	}
}