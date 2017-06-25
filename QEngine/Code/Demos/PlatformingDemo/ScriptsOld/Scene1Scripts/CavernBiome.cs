using QEngine;
using QEngine.Code;
using QEngine.Demos.PlatformingDemo.Scripts;
using QEngine.Interfaces;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class CavernBiome : QBehavior, IQLoad, IQStart
	{
		public CavernBiome(string mapName, string mapPath) : base("CavernBiome")
		{
			map = mapName;
			this.mapPath = mapPath;
		}

		string map;

		string mapPath;

		public void OnLoad(QAddContent content)
		{
			content.Texture(map, mapPath);
			content.Texture(Assets.Bryan + "cavern_wall");
			content.Texture(Assets.Bryan + "cavern_biome");
		}

		public void OnStart(QGetContent content)
		{
			Instantiate(new CavernWallBackground());

			using(var layer1 = content.Texture("map1_3").GetPartialTexture(new QRect(0, 0, 128, 128)))
			{
				QMapTools.SpawnObjects(layer1, Transform.Position, new QVec(16) * 4, (color, pos) =>
				{
					if(color == new QColor(0, 150, 50))
					{
						Instantiate(new PlayerSpawner(), pos);
					}
					if(color == QColor.Black)
					{
						Instantiate(new Spikes(), pos);
					}
					if(color == new QColor(75, 60, 45))
					{
						Instantiate(new CavernFloorTile(), pos);
					}
					if(color == new QColor(35, 30, 20))
					{
						Instantiate(new CavernWalls(), pos);
					}
					if(color == new QColor(255, 0, 0))
					{
						Instantiate(new BatSpawner(), pos);
					}
				});
			}
		}
	}
}