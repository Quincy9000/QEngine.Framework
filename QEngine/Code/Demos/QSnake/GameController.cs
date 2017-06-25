namespace QEngine.Demos
{
	class GameController : QCharacterController
	{
		public GameController() : base("GameController") { }

		public override void OnUpdate(QTime time)
		{
			if(Input.IsKeyPressed(QKeys.Escape))
				ExitGame();
			if(Input.IsKeyPressed(QKeys.R))
				Scene.ResetScene();
		}
	}
}
