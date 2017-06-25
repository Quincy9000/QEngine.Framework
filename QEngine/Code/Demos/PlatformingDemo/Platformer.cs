using QEngine.Demos.PlatformingDemo.Scripts;

namespace QEngine.Demos
{
	public class Platformer : QScene
	{
		protected override void Load()
		{
//			Instantiate(new CavernBiome("map1_3", Assets.Bryan + "map1_2/map1_3"));
//			Instantiate(new BlockCreator());
//			Instantiate(new Player());
//			Instantiate(new Floor(new QVec(0, Window.Height / 2f - 20)));
//			Instantiate(new Floor(new QVec(0, -Window.Height / 2f + 20)));
//			Instantiate(new Wall(new QVec(-Window.Width / 2f + 20, 0)));
//			Instantiate(new Wall(new QVec(Window.Width / 2f - 20, 0)));
//			Instantiate(new Platform(new QVec(0, 500), new QVec(5000, 20)));
//			Instantiate(new Bat(new QVec(-200, -200)));
			Instantiate(new BiomeLevel());
		}

		public Platformer() : base("Platformer") { }
	}
}