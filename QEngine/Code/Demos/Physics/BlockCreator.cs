﻿using System.Collections;
using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
	class BlockCreator : QCharacterController
	{
		public override void OnStart(QGetContent get)
		{
			//Coroutine.Start(ShowFPS());
		}

		IEnumerator ShowFPS()
		{
			Console.WriteLine($"FPS: {Debug.Fps}");
			yield return QCoroutine.WaitForSeconds(0.3f);
			Coroutine.Start(ShowFPS());
		}

		public override void OnFixedUpdate(float time)
		{
			//if(Input.IsKeyDown(QKeys.Space))
				//World.Gravity = new QVec(0, -10);
			//else
				//World.Gravity = new QVec(0, QWorldManager.DefaultGravity);
			if(Input.IsLeftMouseButtonHeld() && Accumulator.CheckAccum("Spawner", 0.03f))
				Instantiate(new Block(), Camera.ScreenToWorld(Input.MousePosition()));
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
