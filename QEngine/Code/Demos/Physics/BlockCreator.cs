using System.Collections;
using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
	class BlockCreator : QCharacterController
	{
		IEnumerator ShowFPS()
		{
			Console.WriteLine($"FPS: {Debug.Fps}");
			yield return QCoroutine.WaitForSeconds(0.3f);
			Coroutine.Start(ShowFPS());
		}

		public override void OnFixedUpdate(float time)
		{
			if(Input.IsKeyDown(QKeys.Space))
				World.Gravity = new QVec(0, -10);
			else
				World.Gravity = new QVec(0, QWorldManager.DefaultGravity);
			if(Input.IsLeftMouseButtonHeld() && Accumulator.CheckAccum("Spawner", 0.03f))
			{
				var tt = 40;
				int i = QRandom.Range(0, 1);
				if(i == 0)
					Instantiate(new Block(tt, tt), Camera.ScreenToWorld(Input.MousePosition()));
				else
				{
					Instantiate(new Ball(tt), Camera.ScreenToWorld(Input.MousePosition()));
				}
			}
			if(Input.IsMouseScrolledUp())
				Camera.Zoom += Camera.Zoom * 0.1f;
			if(Input.IsMouseScrolledDown())
				Camera.Zoom -= Camera.Zoom * 0.1f;
			if(Input.IsKeyPressed(QKeys.Escape))
				Scene.ResetScene();
		}

		public BlockCreator() : base("BlockCreator") { }
	}
}