using QEngine.Prefabs;

namespace QEngine.Demos.CircleSpawnDemo
{
	public class QDemoCircle : QCharacterController
	{
		QSprite sprite;

		public override void OnLoad(QAddContent add)
		{
			add.Circle("circle", 60, QColor.White);
		}

		public override void OnStart(QGetContent get)
		{
			int Color() => QRandom.Number(1, 255);
			sprite = new QSprite(this, "circle");
			sprite.Color = new QColor(Color(), Color(), Color());
			Transform.Position = new QVec(
				QRandom.Number(Camera.Position.X - Window.Width/2f, Camera.Position.X  + Window.Width/2f), 
				QRandom.Number(Camera.Position.Y - Window.Height/2f, Camera.Position.Y + Window.Height/2f));
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