using QEngine;
using QEngine.Code;
using QEngine.Interfaces;
using System.Collections.Generic;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class CavernWallBackground : QBehavior, IQStart, IQDrawSprite
	{
		public CavernWallBackground() : base("CavernWallBackground") { }

		List<QMapTools.TilePos> OutsideTiles;

		List<QMapTools.TilePos> InsideTiles;

		public QSprite Sprite { get; set; }

		public void OnStart(QGetContent content)
		{
			Sprite = new QSprite(this, "cavern_wall");
			Sprite.Origin = new QVec(8);
			Sprite.Layer = 1f;
			Sprite.Color *= 0.4f;
			Transform.Scale = new QVec(4);

			QRect wall = Scene.MegaTexture["cavern_biome"].Split(16, 16)[1];

			using(var layer2 = content.Texture("map1_3").GetPartialTexture(new QRect(0, 0, 128, 128)))
			{
				OutsideTiles = QMapTools.CreateSpriteLayer(layer2, Transform.Position, new QVec(16, 16) * Transform.Scale, color =>
				{
					if(color == new QColor(60, 45, 30))
					{
						return wall;
					}
					return QRect.Empty;
				});

				InsideTiles = QMapTools.CreateSpriteLayer(layer2, Transform.Position, new QVec(16, 16) * Transform.Scale, color =>
				{
					if(color == new QColor(134, 97, 61))
					{
						return wall;
					}
					return QRect.Empty;
				});
			}
		}

		public void OnDrawSprite(QSpriteRenderer renderer)
		{
			QVec temp;
			Sprite.Color = QColor.DarkKhaki;
			for(int i = 0; i < OutsideTiles.Count; ++i)
			{
				temp = OutsideTiles[i].Position;
				if(QVec.Distance(Transform.Position, Camera.Position) < Scene.Window.Width)
				{
					Sprite.Source = OutsideTiles[i].Source;
					renderer.Draw(Sprite, Transform, temp);
				}
			}
			Sprite.Color = QColor.DarkSlateGray;
			for(int i = 0; i < InsideTiles.Count; i++)
			{
				temp = InsideTiles[i].Position;
				if(QVec.Distance(Transform.Position, Camera.Position) < Scene.Window.Width)
				{
					Sprite.Source = InsideTiles[i].Source;
					renderer.Draw(Sprite, Transform, temp);
				}
			}
		}
	}
}