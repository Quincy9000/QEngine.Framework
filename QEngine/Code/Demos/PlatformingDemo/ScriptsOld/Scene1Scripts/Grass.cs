using QEngine;
using QEngine.Demos.PlatformingDemo.Scripts;
using QEngine.Interfaces;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class Grass : QBehavior, IQLoad, IQStart, IQDrawSprite
	{
		public Grass(QVec pos) : base("Grass")
		{
			topos = pos;
		}

		public Grass() : base("Grass") { }

		public QSprite Sprite { get; set; }

		public QRigiBody body;

		public void OnLoad(QAddContent content)
		{
			//Texture("Grass", Assets.Sprites + "Bryan/Grass");
			content.Texture(Assets.Sprites + "Bryan/Grass");
		}

		QVec topos = QVec.Zero;

		public void OnStart(QGetContent content)
		{
			Sprite.Source = Scene.MegaTexture["Grass"].Split(16, 16)[0];
			Sprite.Layer = 1f;
			Sprite.Origin = new QVec(8);
			Transform.Scale = new QVec(4);

			//Position = topos * Sprite.Scale * new Vector2(16);
			body = World.CreateRectangle(this, Transform.Scale.X * 16, Transform.Scale.X * 16, 1f, Transform.Position, 0, QBodyType.Static);
		}

		public void OnDrawSprite(QSpriteRenderer renderer)
		{
			//if(QVec.Distance(this.Position, Camera.Position) < 2000)
				renderer.Draw(Sprite, Transform);
		}
	}
}