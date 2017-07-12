using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo
{
	/// <summary>
	/// Platform for the player to stand on
	/// </summary>
	public class BiomeFloor : QBehavior, IQStart, IQDrawSprite
	{
		QSprite sprite;

		QSprite mushroomSprite;

		QRigiBody body;

		QVec toSize;

		public void OnStart(QGetContent get)
		{
			var floorTiles = get.TextureSource("cavern_biome").Split(16, 16);
			sprite = new QSprite(this, floorTiles[0]);
			//Randomly decide to put mushroom on floor tile
			if(QRandom.Number(1, 10) > 6)
				mushroomSprite = new QSprite(this, floorTiles[QRandom.Number(2,4)]);
			Transform.Scale = QVec.One * 4.01f;
			body = World.CreateRectangle(this, sprite.Width, sprite.Height, 1, QBodyType.Static);
			body.Friction = 0.2f;
			body.IsCCD = true;
		}

		public void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			if(QVec.Distance(Position, Camera.Position) < Scene.Window.Width)
			{
				spriteRenderer.Draw(sprite, Transform);
				if(mushroomSprite != null)
				{
					spriteRenderer.Draw(mushroomSprite, Transform, Position + new QVec(0, -48));
				}
			}
		}
	}
}