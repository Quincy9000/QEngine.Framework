using System.Collections.Generic;
using QEngine;
using QEngine.Demos.PlatformingDemo.Scripts;
using QEngine.Prefabs;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class CavernFloorTile : QCharacterController
	{
		public CavernFloorTile() : base("CavernFloorTile") { }

		List<QRect> Frames;

		QRect Mushroom;

		static readonly QVec mushPos = new QVec(0, -48);

		QRect Tile;

		QRigiBody body;

		public override void OnLoad(QAddContent content)
		{
			//Texture("cavern_biome", Assets.Bryan + "cavern_biome");
			content.Texture(Assets.Bryan + "cavern_biome");
		}

		public override void OnStart(QGetContent content)
		{
			Frames = Scene.MegaTexture["cavern_biome"].Split(16, 16);
			Tile = Frames[0];

			Sprite = new QSprite(this, Tile);
			Sprite.Color = QColor.DarkKhaki;
			Sprite.Layer = 100f;
			Transform.Scale = new QVec(4);

			int random = QRandom.Range(1, 10);
			if(random == 1)
			{
				Mushroom = Frames[2];
			}
			else if(random == 2)
			{
				Mushroom = Frames[3];
			}
			else if(random == 3)
			{
				Mushroom = Frames[4];
			}
			else
			{
				Mushroom = QRect.Empty;
			}

			body = World.CreateRectangle(this, 14 * 4, 14 * 4, 1f, Transform.Position, 0, QBodyType.Static);
			body.Friction = 6f;
		}

		public override void OnDrawSprite(QSpriteRenderer renderer)
		{
			if(QVec.Distance(Transform.Position, Camera.Position) < Scene.Window.Width)
			{
				//TODO
				Sprite.Source = Mushroom;
				//DrawSource(renderer, Position + mushPos); //FIX DIS
				renderer.Draw(Sprite, Transform, Transform.Position + mushPos);
				Sprite.Source = Tile;
				//DrawSource(renderer);
				renderer.Draw(Sprite, Transform);
			}
		}
	}
}