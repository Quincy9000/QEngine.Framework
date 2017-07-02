using QEngine.Demos.CircleSpawnDemo;

namespace QEngine.Demos.Scenes
{
	public class QDemoScene : QScene
	{
		public QDemoScene() : base("DemoScene") { }

		protected override void Load()
		{
			Instantiate(new QDemoCreator());
		}
	}
}