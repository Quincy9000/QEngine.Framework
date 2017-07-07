using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo
{
	/// <summary>
	/// Platform for the player to stand on
	/// </summary>
	public class BiomeWall : QCharacterController
	{
		QSprite sprite;

		QRigiBody body;

		QVec toSize;

		public override void OnStart(QGetContent get)
		{
			var floorTiles = get.TextureSource("cavern_biome").Split(16, 16);
			sprite = new QSprite(this, floorTiles[1]);
			Transform.Scale = QVec.One * 4.01f;
			body = World.CreateRectangle(this, sprite.Width, sprite.Height, 1, QBodyType.Static);
			body.Friction = 0.2f;
			body.IsCCD = false;
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			if(QVec.Distance(Position, Camera.Position) < 1000)
				spriteRenderer.Draw(sprite, Transform);
		}
	}
}