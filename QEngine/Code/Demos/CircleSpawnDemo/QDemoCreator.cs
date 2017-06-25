namespace QEngine.Demos
{
	class QDemoCreator : QCharacterController
	{
		public override void OnStart(QGetContent getContent)
		{
			Instantiate(new QDemoObject());
		}

		public QDemoCreator() : base("QDemoCreator") { }
	}
}