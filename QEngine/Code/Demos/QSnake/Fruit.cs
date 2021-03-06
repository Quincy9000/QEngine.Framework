﻿using QEngine.Prefabs;

namespace QEngine.Demos.QSnake
{
	public class Fruit : QCharacterController
	{
		QSprite sprite;

		public QRect Collision { get; private set; }

		public override void OnLoad(QAddContent add)
		{
			add.Rectangle("fruit", 40, 40, QColor.Red);
		}

		public override void OnStart(QGetContent get)
		{
			sprite = new QSprite(this, "fruit");
		}

		public void NewLocation()
		{
			Transform.Position = new QVec(
				QRandom.Number(Camera.Position.X - Window.Width / 2f, Camera.Position.X + Window.Width / 2f),
				QRandom.Number(Camera.Position.Y - Window.Height / 2f, Camera.Position.Y + Window.Height / 2f));
		}

		public override void OnFixedUpdate(float time)
		{
			Collision = new QRect(Transform.Position - new QVec(sprite.Width / 2f), new QVec(40));
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);
		}

		public Fruit() : base("Fruit") { }
	}
}