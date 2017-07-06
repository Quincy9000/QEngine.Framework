using System.Collections;
using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
	class BlockCreator : QCharacterController
	{
		public override void OnUpdate(QTime delta)
		{
//			if(Input.IsKeyHeld(QKeys.Space))
//				World.Gravity = new QVec(0, -10);
//			else
//				World.Gravity = new QVec(0, QWorldManager.DefaultGravity);
			if(Input.IsLeftMouseButtonHeld() && Accumulator.CheckAccum("Spawner", 0.03f))
//			{
//				var size = 40;
//				if(QRandom.Number(0, 1) == 0)
//					Instantiate(new Block(size, size), Camera.ScreenToWorld(Input.MousePosition()));
//				else
//					Instantiate(new Ball(size), Camera.ScreenToWorld(Input.MousePosition()));
//			}
			if(Input.IsMouseScrolledUp())
				Camera.Zoom += Camera.Zoom * 0.1f;
			if(Input.IsMouseScrolledDown())
				Camera.Zoom -= Camera.Zoom * 0.1f;
			if(Input.IsKeyPressed(QKeys.Escape))
				Scene.ResetScene();
		}
	}
}