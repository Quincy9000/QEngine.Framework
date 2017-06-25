namespace QEngine.Demos
{
	public class QDemoCircle : QCharacterController
	{
		QSprite sprite;

		public override void OnLoad(QAddContent add)
		{
			add.Circle("circle", 60, QColor.White);
		}

		public override void OnStart(QGetContent getContent)
		{
			int Color() => QRandom.Range(1, 255);
			sprite = new QSprite(this, "circle");
			sprite.Color = new QColor(Color(), Color(), Color());
			Transform.Position = new QVec(
				QRandom.Range(Camera.Position.X - Window.Width/2f, Camera.Position.X  + Window.Width/2f), 
				QRandom.Range(Camera.Position.Y - Window.Height/2f, Camera.Position.Y + Window.Height/2f));
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);
		}

		public QDemoCircle() : base("QDemoCircle")
		{

		}
	}
}