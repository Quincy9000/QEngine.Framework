namespace QEngine
{
	public class QApplication
	{
		QEngine Engine { get; }

		public QApplication(QAppConfig conf) => Engine = new QEngine(conf);

		public QApplication() => Engine = new QEngine(new QAppConfig());

		public void Run(params QWorld[] worlds)
		{
			if(worlds.Length > 0)
			{
				foreach(var qScene in worlds)
				{
					qScene.Engine = Engine;
					Engine.Manager.AddScene(qScene);
				}
			}
			else
			{
				var s = new QWorld();
				s.Engine = Engine;
				Engine.Manager.AddScene(s);
			}
			using(Engine)
				Engine.Run();
		}
	}
}