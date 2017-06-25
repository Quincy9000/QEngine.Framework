﻿namespace QEngine.Demos
{
	public class QDemoObject2 : QCharacterController
	{
		public override void OnUpdate(QTime time)
		{
			if(Input.IsKeyPressed(QKeys.R))
				Scene.SpriteRenderer.ClearColor = QColor.Red;
			else if(Input.IsKeyPressed(QKeys.B))
				Scene.SpriteRenderer.ClearColor = QColor.Blue;
			else if(Input.IsKeyPressed(QKeys.G))
				Scene.SpriteRenderer.ClearColor = QColor.Green;
			else if(Input.IsKeyPressed(QKeys.Escape))
				ExitGame();
		}

		public QDemoObject2() : base("QDemoObject2") { }
	}
}