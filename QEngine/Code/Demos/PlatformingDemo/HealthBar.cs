namespace QEngine.Demos.PlatformingDemo
{
	public class HealthBar : QBehavior, IQLoad,  IQStart, IQDrawGui
	{
		Player p;

		QImage Image { get; set; }

		QRect Heart;

		QRect EmptyHeart;

		public void OnLoad(QAddContent add)
		{
			add.Texture(Assets.Bryan + "BryanStuff1");
		}

		public void OnStart(QGetContent get)
		{
			Heart = get.TextureSource("BryanStuff1").Split(32, 32)[21];
			EmptyHeart = get.TextureSource("BryanStuff1").Split(32, 32)[22];
			p = GetComponent<Player>("Player");
			Image = new QImage(this, Heart);
			Transform.Scale = new QVec(4);
		}

		public void OnDrawGui(QGuiRenderer renderer)
		{
			QVec pos = new QVec(50, 50);
			Image.Source = Heart;
			for(int i = 0; i < p.Health; i++)
			{
				renderer.DrawImage(Image, Transform, pos);
				pos += new QVec(50, 0);
			}
			pos = new QVec(50 * p.HealthMax, 50);
			Image.Source = EmptyHeart;
			for(int i = p.Health; i < p.HealthMax; ++i)
			{
				renderer.DrawImage(Image, Transform, pos);
				pos -= new QVec(50, 0);
			}
		}
	}
}
