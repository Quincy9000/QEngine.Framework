namespace QEngine
{
	public class QGetContent
	{
		QContentManager cm;

		public QTexture Texture(string t) => cm.GetTexture(t);

		public QFont Font(string f) => cm.GetFont(f);

		public QSound Sound(string s) => cm.GetSound(s);

		public QMusic Music(string m) => cm.GetMusic(m);

		public QRect TextureSource(string n) => cm.MegaTexture[n];

		internal QGetContent(QContentManager cm)
		{
			this.cm = cm;
		}
	}
}
