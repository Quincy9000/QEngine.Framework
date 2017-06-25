namespace QEngine.Demos
{
	class Block : QCharacterController
	{
		QSprite sprite;

		QRigiBody body;

		public override void OnLoad(QAddContent add)
		{
			add.Circle("Block", 22, QColor.White);
			//add.Rectangle("Block", 22, 22, QColor.YellowGreen);
		}

		public override void OnStart(QGetContent get)
		{
			int r()
			{
				return QRandom.Range(1, 255);
			}
			sprite = new QSprite(this, "Block");
			sprite.Color = new QColor(r(), r(), r());
			body = World.CreateCircle(this, 10, 100, Transform.Position);
		}

		public override void OnUpdate(QTime time)
		{
			//if(QVec.Distance(Camera.Position, Transform.Position) > 500)
				//Scene.AddToDestroyList(this);
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);	
		}

		public override void OnDestroy()
		{
			//Console.WriteLine($"Block destroyed! {Id}");
		}

		public Block() : base("Block") { }
	}
}