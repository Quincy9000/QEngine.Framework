using QEngine.Prefabs;

namespace QEngine.Demos.QSnake
{
	class GameController : QCharacterController
	{
		public GameController() : base("GameController") { }

		public override void OnFixedUpdate(float time)
		{
			if(Input.IsKeyPressed(QKeys.Escape))
				ExitGame();
			if(Input.IsKeyPressed(QKeys.R))
				Scene.ResetScene();
		}
	}
}
