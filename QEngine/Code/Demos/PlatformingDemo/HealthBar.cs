namespace QEngine.Demos.PlatformingDemo
{
	public class HealthBar : QBehavior, IQLoad,  IQStart, IQDrawGui
	{
		Player p;

		QImage GuiHeart { get; set; }

		QImage GuiEmptyHeart { get; set; }

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
			p = GetBehavior<Player>();
			GuiHeart = new QImage(this, Heart);
			GuiEmptyHeart = new QImage(this, EmptyHeart);
			GuiHeart.Scale = new QVec(4);
			GuiEmptyHeart.Scale = new QVec(4);
		}

		public void OnDrawGui(QGuiRenderer renderer)
		{
			QVec pos = new QVec(50, 50);
			for(int i = 0; i < p.Health; i++)
			{
				renderer.DrawImage(GuiHeart, Transform, pos);
				pos += new QVec(50, 0);
			}
			pos = new QVec(50 * p.MaxHealth, 50);
			for(int i = p.Health; i < p.MaxHealth; ++i)
			{
				renderer.DrawImage(GuiEmptyHeart, Transform, pos);
				pos -= new QVec(50, 0);
			}
		}
	}
}
