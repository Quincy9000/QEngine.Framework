﻿using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
	class Floor : QCharacterController
	{
		QSprite sprite;

		QRigiBody body;

		QVec toPos;

		public override void OnLoad(QAddContent add)
		{
			add.Rectangle(Name, Window.Width, 40, QColor.White);
			Transform.Position = toPos;
		}

		public override void OnStart(QGetContent get)
		{
			int r()
			{
				return QRandom.Range(1, 255);
			}
			sprite = new QSprite(this, Name);
			sprite.Color = new QColor(r(), r(), r());
			body = World.CreateRectangle(this, sprite.Width, sprite.Height, 1, QBodyType.Static);
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);
		}

		public Floor(QVec p) : base("Floor")
		{
			toPos = p;
		}
	}
}