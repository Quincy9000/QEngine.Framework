using QEngine.Prefabs;

namespace QEngine.Demos.CircleSpawnDemo
{
	public class QDemoObject2 : QCharacterController
	{
		public override void OnFixedUpdate(float time)
		{
			if(QInput.Pressed(QKeyStates.R))
				Scene.SpriteRenderer.ClearColor = QColor.Red;
			else if(QInput.Pressed(QKeyStates.B))
				Scene.SpriteRenderer.ClearColor = QColor.Blue;
			else if(QInput.Pressed(QKeyStates.G))
				Scene.SpriteRenderer.ClearColor = QColor.Green;
			else if(QInput.Pressed(QKeyStates.Escape))
				Window.Exit();
		}

		public QDemoObject2() : base("QDemoObject2") { }
	}
}