﻿using System.Collections;
using System.Collections.Generic;

namespace QEngine.Demos
{
	public class Snakehead : QCharacterController
	{
		QSprite sprite;

		QRect Collision;

		enum SnakeDirection
		{
			Up,
			Down,
			Left,
			Right
		}

		int Points;

		SnakeDirection CurrentDir;

		const int Movement = 40;

		Fruit fruit;

		float snakeMoveSpeed = 0.4f;

		List<SnakeBody> Bodies;

		QVec LastPosition;

		public override void OnLoad(QAddContent add)
		{
			Bodies = new List<SnakeBody>();
			Points = 0;
			add.Rectangle("snakeHead", 40, 40, QColor.White);
		}

		public override void OnStart(QGetContent get)
		{
			sprite = new QSprite(this, "snakeHead");
			CurrentDir = SnakeDirection.Down;
			Coroutine.Start(SnakeMove());

			fruit = GetComponent<Fruit>("Fruit");
			Scene.SpriteRenderer.ClearColor = QColor.Black;
		}

		public override void OnUpdate(QTime time)
		{
			Collision = new QRect(Transform.Position - new QVec(sprite.Width / 2f - 5), new QVec(Movement - 5));
			if(Collision.Intersects(fruit.Collision))
			{
				var body = new SnakeBody();
				Bodies.Insert(0, body);
				Instantiate(body, LastPosition);
				fruit.NewLocation();
				//snakeMoveSpeed -= snakeMoveSpeed * 0.05f;
				Points++;
			}
			if(Bodies.Count > 0)
				Bodies[0].Transform.Position = LastPosition;
			for(int i = 0; i < Bodies.Count; i++)
			{
				if(Collision.Intersects(Bodies[i].Collision))
					Scene.AddToDestroyList(this);
				//Bodies[i + 1].LastPosition = Bodies[i].LastPosition;
			}
			if(Input.IsKeyDown(QKeys.W))
				CurrentDir = SnakeDirection.Up;
			if(Input.IsKeyDown(QKeys.S))
				CurrentDir = SnakeDirection.Down;
			if(Input.IsKeyDown(QKeys.A))
				CurrentDir = SnakeDirection.Left;
			if(Input.IsKeyPressed(QKeys.D))
				CurrentDir = SnakeDirection.Right;

			if(!Camera.IsInCameraView(Transform.Position))
				Scene.AddToDestroyList(this);
		}

		IEnumerator SnakeMove()
		{
			LastPosition = Transform.Position;
			switch(CurrentDir)
			{
				case SnakeDirection.Up:
					Transform.Position += QVec.Up * Movement;
					break;
				case SnakeDirection.Down:
					Transform.Position += QVec.Down * Movement;
					break;
				case SnakeDirection.Left:
					Transform.Position += QVec.Left * Movement;
					break;
				case SnakeDirection.Right:
					Transform.Position += QVec.Right * Movement;
					break;
				default:
					break;
			}
			yield return QCoroutine.WaitForSeconds(snakeMoveSpeed);
			Coroutine.Start(SnakeMove());
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);
		}

		public override void OnDestroy()
		{
			Console.WriteLine("SNEK IS DED!!! Press R to restart! Escape to quit!");
		}

		public Snakehead() : base("SnakeHead") { }
	}
}