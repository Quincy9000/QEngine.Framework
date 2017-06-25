﻿namespace QEngine.Demos
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