namespace QEngine.Demos.CircleSpawnDemo
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