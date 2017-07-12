namespace QEngine.Demos.PlatformingDemo
{
	public class BiomeLevel : QBehavior, IQLoad, IQStart
	{
		public void OnLoad(QAddContent add)
		{
			add.Texture(Assets.Bryan + "BiomeMap/biomeMapFront");
			add.Texture(Assets.Bryan + "cavern_biome");
		}

		public void OnStart(QGetContent get)
		{
			Scene.SpriteRenderer.ClearColor = QColor.Black;

			Instantiate(new BackgroundRendering());

			QMapTools.SpawnObjects(get, "biomeMapFront", QVec.Zero, new QVec(64, 64), (c, v) =>
			{
				if(c == new QColor(0, 150, 50)) //player
				{
					Instantiate(new Player(), v);
				}
				else if(c == new QColor(75, 60, 45)) //ground
				{
					Instantiate(new BiomeFloor(), v);
				}
				else if(c == new QColor(35, 30, 20)) //walls
				{
					Instantiate(new BiomeWall(), v);
				}
				else if(c == new QColor(255, 0, 0))//bats
				{
					Instantiate(new BiomeBat(), v);
				}
			});
			
			Instantiate(new PlayerCamera());
		}
	}
}