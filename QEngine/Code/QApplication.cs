using QEngine.Demos;
using QEngine.Demos.CircleSpawnDemo;

namespace QEngine
{
	public class QApplication
	{
		QEngine Engine { get; }

		public QApplication(QAppConfig conf) => Engine = new QEngine(conf);

		public QApplication() => Engine = new QEngine();

		public void Run(params QScene[] scenes)
		{
			if(scenes.Length > 0)
			{
				foreach (var qScene in scenes)
				{
					qScene.Engine = Engine;
					Engine.Manager.AddScene(qScene);
				}
			}
			else
			{
				var s = new QDemoScene();
				s.Engine = Engine;
				Engine.Manager.AddScene(s);
			}
			Engine.Run();
			Engine.Dispose();
		}
	}
}