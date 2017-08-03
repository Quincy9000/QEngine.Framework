using System.Collections;
using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
	class BlockCreator : QCharacterController
	{
		public override void OnUpdate(QTime delta)
		{
////			if(QInput.IsKeyHeld(QKeys.Space))
////				World.Gravity = new QVec(0, -10);
////			else
////				World.Gravity = new QVec(0, QWorldManager.DefaultGravity);
//			if(QInput.Held(QMouseStates.Left) && Accumulator.CheckAccum("Spawner", 0.03f))
////			{
////				var size = 40;
////				if(QRandom.Number(0, 1) == 0)
////					Instantiate(new Block(size, size), Camera.ScreenToWorld(QInput.MousePosition()));
////				else
////					Instantiate(new Ball(size), Camera.ScreenToWorld(QInput.MousePosition()));
////			}
//				if(QInput.IsMouseScrolledUp())
//					Camera.Zoom += Camera.Zoom * 0.1f;
//			if(QInput.IsMouseScrolledDown())
//				Camera.Zoom -= Camera.Zoom * 0.1f;
//			if(QInput.IsKeyPressed(QKeyStates.Escape))
//				Scene.ResetScene();
		}
	}
}