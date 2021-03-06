﻿using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo
{
	/// <summary>
	/// Platform for the player to stand on
	/// </summary>
	public class BiomeWall : QBehavior, IQStart, IQDrawSprite
	{
		QSprite sprite;

		QRigidBody body;

		QVec toSize;

		public void OnStart(QGetContent get)
		{
			var floorTiles = get.TextureSource("cavern_biome").Split(16, 16);
			sprite = new QSprite(this, floorTiles[1]);
			sprite.Scale = QVec.One * 4.01f;
			body = Physics.CreateRectangle(this, sprite.Width, sprite.Height, 1, QBodyType.Static);
			body.Friction = 0.2f;
			body.IsCCD = false;
		}

		public void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			if(QVec.Distance(Position, Camera.Position) < Scene.Window.Width)
				spriteRenderer.Draw(sprite, Transform);
		}
	}
}