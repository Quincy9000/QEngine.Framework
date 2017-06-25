using QEngine;
using QEngine.Interfaces;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class Spikes : QBehavior, IQStart, IQDrawSprite
	{
		public Spikes() : base("Spikes") { }

		public QSprite Sprite { get; set; }

		QRigiBody body;

		public void OnStart(QGetContent content)
		{
			Sprite.Source = Scene.MegaTexture["cavern_biome"].Split(16, 16)[0];
			Sprite.Origin = new QVec(8);
			Transform.Scale = new QVec(4);

			body = World.CreateRectangle(this, 16 * 4, 16 * 4, 1f, Transform.Position,0, QBodyType.Static);
		}

		public void OnDrawSprite(QSpriteRenderer renderer)
		{
			//if(Vector2.Distance(Position, MainCamera.Position) < Scene.WindowWidth)
				//DrawSource(renderer);
				renderer.Draw(Sprite, Transform);
		}
	}
}