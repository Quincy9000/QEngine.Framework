using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace QEngine.Demos.PlatformingDemo
{
	public class BackgroundRendering : QBehavior, IQStart, IQLoad, IQDrawSprite
	{
		List<QTilePos> BackgroundTiles;

		List<QTilePos> LevelTiles;

		List<QTilePos> TestTiles;

		QSprite Sprite;

		public void OnLoad(QAddContent add)
		{
			BackgroundTiles = new List<QTilePos>(1000);
			LevelTiles = new List<QTilePos>(1000);
			add.Texture(Assets.Bryan + "BiomeMap/biomeMapBack");
			add.Texture(Assets.Bryan + "BiomeMap/biomeMapFront");
		}

		public void OnStart(QGetContent content)
		{
			var cavernBiomes = content.TextureSource("cavern_biome").Split(16, 16);

			Sprite = new QSprite(this, cavernBiomes[0]);
			Transform.Scale = QVec.One * 4;

//			BackgroundTiles = QMapTools.CreateSpriteLayer(content, "biomeMapBack", QVec.Zero, QVec.One * 64, c =>
//			{
//				if(c == new QColor(60, 45, 30))
//				{
//					return cavernBiomes[1];
//				}
//				return QRect.Empty;
//			});
//
//			LevelTiles = QMapTools.CreateSpriteLayer(content, "biomeMapBack", QVec.Zero, QVec.One * 64, c =>
//			{
//				if(c == new QColor(134, 97, 61))
//				{
//					return cavernBiomes[1];
//				}
//				return QRect.Empty;
//			});

			TestTiles = QMapTools.CreateSpriteLayer(content, "biomeMapBack", QVec.Zero, QVec.One * 64, c =>
			{
				if(c == new QColor(60, 45, 30))
				{
					return cavernBiomes[1];
				}
				if(c == new QColor(134, 97, 61))
				{
					return cavernBiomes[1];
				}
				return QRect.Empty;
			});
		}

		public void OnDrawSprite(QSpriteRenderer renderer)
		{
//			Sprite.Color = QColor.White;
//			var cpos = Camera.Position;
//			for(int i = 0; i < BackgroundTiles.Count; ++i)
//			{
//				if(QVec.Distance(BackgroundTiles[i].Position, cpos) < 1000)
//					renderer.Draw(BackgroundTiles[i].Source, Sprite, Transform, BackgroundTiles[i].Position);
//			}
//			Sprite.Color = QColor.BlanchedAlmond;
//			for(int i = 0; i < LevelTiles.Count; ++i)
//			{
//				if(QVec.Distance(LevelTiles[i].Position, cpos) < 1000)
//					renderer.Draw(LevelTiles[i].Source, Sprite, Transform, LevelTiles[i].Position);
//			}
		}
	}
}