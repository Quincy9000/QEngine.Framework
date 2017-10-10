namespace QEngine
{
	public class QGetContent
	{
		QContentManager cm;

		public QTexture Texture(string t) => cm.GetTexture(t);

		public QFont Font(string f) => cm.GetFont(f);

		public QSound Sound(string s) => cm.GetSound(s);

		public QMusic Music(string m) => cm.GetMusic(m);

		/// <summary>
		/// returns the source area from a texture that is used on a textureatlas
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public QRectangle Source(string s)
		{
			foreach(var a in cm.Atlases)
			{
				foreach(var r in a.Rectangles)
				{
					if(r.Key == s)
						return r.Value;
				}
			}
			return QRectangle.Empty;
		}

		internal QGetContent(QContentManager cm)
		{
			this.cm = cm;
		}
	}
}
