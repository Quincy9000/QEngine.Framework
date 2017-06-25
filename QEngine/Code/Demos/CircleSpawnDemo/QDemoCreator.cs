namespace QEngine.Demos
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