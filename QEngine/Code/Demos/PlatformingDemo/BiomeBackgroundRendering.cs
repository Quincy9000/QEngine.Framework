using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace QEngine.Demos.PlatformingDemo
{
	public class BackgroundRendering : QBehavior, IQStart, IQLoad, IQUpdate, IQDrawSprite
	{
		List<QTilePos> BackgroundTiles;

		List<QTilePos> LevelTiles;

		QSprite Sprite;

		QQuad quad;

		List<QTilePos> tiles;

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

			BackgroundTiles = QMapTools.CreateSpriteLayer(content, "biomeMapBack", QVec.Zero, QVec.One * 64, c =>
			{
				if(c == new QColor(60, 45, 30))
				{
					return cavernBiomes[1];
				}
				return QRect.Empty;
			});

			LevelTiles = QMapTools.CreateSpriteLayer(content, "biomeMapBack", QVec.Zero, QVec.One * 64, c =>
			{
				if(c == new QColor(134, 97, 61))
				{
					return cavernBiomes[1];
				}
				return QRect.Empty;
			});

//			tiles = QMapTools.CreateSpriteLayer(content, "biomeMapBack", QVec.Zero, QVec.One * 64, c =>
//			{
//				if(c == new QColor(60, 45, 30))
//				{
//					return cavernBiomes[1];
//				}
//				if(c == new QColor(134, 97, 61))
//				{
//					return cavernBiomes[1];
//				}
//				return QRect.Empty;
//			});
//
//			var b = content.Texture("biomeMapBack").Bounds;
//			//quad = new QQuad(new QRect(0,0, b.Width * 64, b.Height * 64));
//			quad = new QQuad(new QRect(0,0, b.Width * 64, b.Height * 64));
//
//			for(int i = 0; i < tiles.Count; i++)
//			{
//				quad.Insert(new QRect(tiles[i].Position, tiles[i].Source.Size));
//			}
		}

		public void OnUpdate(float delta)
		{
//			quad = new QQuad(Camera.Bounds);
//
//			for(int i = 0; i < tiles.Count; i++)
//			{
//				quad.Insert(new QRect(tiles[i].Position, tiles[i].Source.Size));
//			}
		}

		public void OnDrawSprite(QSpriteRenderer renderer)
		{
			Sprite.Color = QColor.White;
			var cpos = Camera.Position;
			for(int i = 0; i < BackgroundTiles.Count; ++i)
			{
				if(QVec.Distance(BackgroundTiles[i].Position, cpos) < 1000)
					renderer.Draw(BackgroundTiles[i].Source, Sprite, Transform, BackgroundTiles[i].Position);
			}
			Sprite.Color *= 0.3f;
			for(int i = 0; i < LevelTiles.Count; ++i)
			{
				if(QVec.Distance(LevelTiles[i].Position, cpos) < 1000)
					renderer.Draw(LevelTiles[i].Source, Sprite, Transform, LevelTiles[i].Position);
			}
			//quad tree rendering
//
//			
//			for(int i = 0; i < tiles.Count; i++)
//			{
//				List<QRect> t = new List<QRect>();
//				quad.Retrieve(t, new QRect(tiles[i].Position, new QVec(64, 64)));
//				//quad.Retrieve(t, tiles[i].Source);
//
//				for(int j = 0; j < t.Count; j++)
//				{
//					renderer.Draw(tiles[i].Source, Sprite, Transform, tiles[i].Position);
//				}
//			}
		}
	}
}