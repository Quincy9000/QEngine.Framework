using QEngine.Prefabs;

namespace QEngine.Demos.QSnake
{
	class GameController : QCharacterController
	{
		public GameController() : base("GameController") { }

		public override void OnFixedUpdate(float time)
		{
			if(QInput.IsKeyPressed(QKeyStates.Escape))
				ExitGame();
			if(QInput.IsKeyPressed(QKeyStates.R))
				Scene.ResetScene();
		}
	}
}
