using System.Diagnostics;
using QEngine.Demos.PlatformingDemo.Scripts.Enemies;

namespace QEngine.Demos.PlatformingDemo.Scripts
{
	public class BiomeLevel : QBehavior, IQLoad, IQStart
	{
		public void OnLoad(QAddContent add)
		{
			add.Texture(Assets.Bryan + "BiomeMap/biomeMapFront");
		}

		public void OnStart(QGetContent get)
		{
			QMapTools.SpawnObjects(get, "biomeMapFront", QVec.Zero, new QVec(64, 64), (c, v) =>
			{
				if(c == new QColor(0, 150, 50)) //player
				{
					Instantiate(new Player(), v);
				}
				else if(c == new QColor(75, 60, 45)) //ground
				{
					Instantiate(new Platform(v, new QVec(64, 64)));
				}
				else if(c == new QColor(35, 30, 20)) //walls
				{
					Instantiate(new Platform(v, new QVec(64, 64)));
				}
				else if(c == new QColor(255, 0, 0))//bats
				{
					Instantiate(new Bat(v), v);
				}
			});
		}
	}
}