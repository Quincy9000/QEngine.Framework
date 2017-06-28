using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public class QAddContent
	{
		readonly QContentManager cm;

		public void Texture(string t)
		{
			cm.AddTexture(t);
		}

		public void Texture(string t, string path)
		{
			cm.AddTexture(t, path);
		}

		public void Font(string f)
		{
			cm.AddFont(f);
		}

		public void Font(string f, string path)
		{
			cm.AddFont(f, path);
		}

		public void Sound(string s)
		{
			cm.AddSound(s);
		}

		public void Sound(string s, string path)
		{
			cm.AddSound(s, path);
		}

		public void Music(string m)
		{
			cm.AddMusic(m);
		}

		public void Music(string m, string path)
		{
			cm.AddMusic(m, path);
		}

		public void Rectangle(string n, int w, int h, QColor color)
		{
			var t = new Texture2D(cm.Engine.GraphicsDevice, w, h, false, SurfaceFormat.Color, w * h);
			var c = new QColor[w * h];
			for(var i = 0; i < c.Length; i++)
				c[i] = color;
			t.SetData(c);
			if(!cm.AddCustomTexture(n, t))
			{
				t.Dispose();
			}
		}

		public void Circle(string n, int diameter, QColor color)
		{
//			Texture2D CreateCircleTexture(GraphicsDevice gd, int r, QColor c)
//			{
//				var texture = new Texture2D(gd, diameter, diameter);
//				var colorData = new QColor[diameter * diameter];
//
//				var diam = diameter / 2f;
//				var diamsq = diam * diam;
//
//				for(var x = 0; x < diameter; x++)
//				for(var y = 0; y < diameter; y++)
//				{
//					var index = x * diameter + y;
//					var pos = new QVec(x - diam, y - diam);
//					if(pos.LengthSquared() <= diamsq)
//					{
//						colorData[index] = color;
//					}
//					else
//					{
//						colorData[index] = QColor.Transparent;
//					}
//				}
//
//				texture.SetData(colorData);
//				return texture;
//			}

			Texture2D createCircleText(GraphicsDevice g, int r, QColor c)
			{
				Texture2D texture = new Texture2D(g, r, r);
				QColor[] colorData = new QColor[r * r];

				float diam = r / 2f;
				float diamsq = diam * diam;

				for(int x = 0; x < r; x++)
				{
					for(int y = 0; y < r; y++)
					{
						int index = x * r + y;
						QVec pos = new QVec(x - diam, y - diam);
						if(pos.LengthSquared() <= diamsq)
						{
							colorData[index] = c;
						}
						else
						{
							colorData[index] = QColor.Transparent;
						}
					}
				}

				texture.SetData(colorData);
				return texture;
			}

			var t = createCircleText(cm.Engine.GraphicsDevice, diameter, color);
			cm.AddCustomTexture(n, t);
		}

		internal QAddContent(QContentManager cm)
		{
			this.cm = cm;
		}
	}
}