using QEngine;
using QEngine.Demos.PlatformingDemo.Scripts;
using QEngine.Interfaces;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class Slime : QBehavior, IQLoad, IQStart, IQUpdate, IQDrawSprite
	{
		public Slime() : base("Slime") { }

		QAnimation SlimeIdle;

		QRigiBody body;

		public QSprite Sprite { get; set; }
		
		public void OnLoad(QAddContent content)
		{
			//Texture("slime", Assets.Bryan + "slime");
			content.Texture(Assets.Bryan + "slime");
		}

		public void OnStart(QGetContent content)
		{
			var frame = Scene.MegaTexture["slime"].Split(32,32);
			SlimeIdle = new QAnimation(frame, 0.5f, 0, 1);
			
			Sprite.Origin = new QVec(16);

			body = World.CreateRectangle(this, 32 * 4, 32 * 4, 1f, Transform.Position, 0);
			body.AllowSleep = false;
		}

		public void OnUpdate(QTime time)
		{
			Sprite.Source = SlimeIdle.Play(time.Delta);
		}

		public void OnDrawSprite(QSpriteRenderer renderer)
		{
			//DrawSource(renderer);
			renderer.Draw(Sprite, Transform);
		}
	}
}