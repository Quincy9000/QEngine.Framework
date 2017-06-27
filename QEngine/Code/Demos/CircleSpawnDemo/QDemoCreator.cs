using QEngine.Prefabs;

namespace QEngine.Demos.CircleSpawnDemo
{
	class QDemoCreator : QCharacterController
	{
		public override void OnStart(QGetContent get)
		{
			Instantiate(new QDemoObject());
		}

		public QDemoCreator() : base("QDemoCreator") { }
	}
}