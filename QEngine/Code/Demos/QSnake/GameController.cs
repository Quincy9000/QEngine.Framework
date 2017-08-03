using QEngine.Prefabs;

namespace QEngine.Demos.QSnake
{
	class GameController : QCharacterController
	{
		public GameController() : base("GameController") { }

		public override void OnFixedUpdate(float time)
		{
			if(QInput.Pressed(QKeyStates.Escape))
				Window.Exit();
			if(QInput.Pressed(QKeyStates.R))
				Window.Reset();
		}
	}
}
