﻿using QEngine.Demos.QSnake;

namespace QEngine.Demos.Scenes
{
	public class QSnakeScene : QScene
	{
		public QSnakeScene() : base("Snake") { }

		protected override void Load()
		{
			Instantiate(new GameController());
			Instantiate(new Snakehead());
			Instantiate(new Fruit());
		}
	}
}
