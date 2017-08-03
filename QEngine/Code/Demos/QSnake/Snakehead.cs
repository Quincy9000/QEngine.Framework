using System.Collections;
using System.Collections.Generic;
using QEngine.Prefabs;

namespace QEngine.Demos.QSnake
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

			fruit = GetBehavior<Fruit>("Fruit");
			Scene.SpriteRenderer.ClearColor = QColor.Black;
		}

		public override void OnFixedUpdate(float time)
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
					Scene.Destroy(this);
				//Bodies[i + 1].PreviousPosition = Bodies[i].PreviousPosition;
			}
			if(QInput.Held(QKeyStates.W))
				CurrentDir = SnakeDirection.Up;
			if(QInput.Held(QKeyStates.S))
				CurrentDir = SnakeDirection.Down;
			if(QInput.Held(QKeyStates.A))
				CurrentDir = SnakeDirection.Left;
			if(QInput.Pressed(QKeyStates.D))
				CurrentDir = SnakeDirection.Right;

			if(!Camera.IsInCameraView(Transform.Position))
				Scene.Destroy(this);
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