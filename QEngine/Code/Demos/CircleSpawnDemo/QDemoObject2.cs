using QEngine.Prefabs;

namespace QEngine.Demos.CircleSpawnDemo
{
	public class QDemoObject2 : QCharacterController
	{
		public override void OnFixedUpdate(float time)
		{
			if(QInput.IsKeyPressed(QKeyStates.R))
				Scene.SpriteRenderer.ClearColor = QColor.Red;
			else if(QInput.IsKeyPressed(QKeyStates.B))
				Scene.SpriteRenderer.ClearColor = QColor.Blue;
			else if(QInput.IsKeyPressed(QKeyStates.G))
				Scene.SpriteRenderer.ClearColor = QColor.Green;
			else if(QInput.IsKeyPressed(QKeyStates.Escape))
				ExitGame();
		}

		public QDemoObject2() : base("QDemoObject2") { }
	}
}