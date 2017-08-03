using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public class QAddContent
	{
		readonly QContentManager cm;

		/// <summary>
		/// Adds texture from files and the file name becomes the name of the texture in game
		/// </summary>
		/// <param name="t"></param>
		public void Texture(string t)
		{
			cm.AddTexture(t);
		}

		/// <summary>
		/// Adds texture from files but the name is custom
		/// </summary>
		/// <param name="t"></param>
		public void Texture(string t, string path)
		{
			cm.AddTexture(t, path);
		}

		/// <summary>
		/// Adds font from files and the file name becomes the name of the font in game
		/// </summary>
		/// <param name="f"></param>
		public void Font(string f)
		{
			cm.AddFont(f);
		}

		/// <summary>
		/// Adds font from files but the name is custom
		/// </summary>
		/// <param name="f"></param>
		public void Font(string f, string path)
		{
			cm.AddFont(f, path);
		}

		/// <summary>
		/// Adds sound from the files and the file name becomes the name of the sound in game
		/// </summary>
		/// <param name="s"></param>
		public void Sound(string s)
		{
			cm.AddSound(s);
		}

		/// <summary>
		/// Adds sound from files but the name is custom
		/// </summary>
		/// <param name="s"></param>
		public void Sound(string s, string path)
		{
			cm.AddSound(s, path);
		}

		/// <summary>
		/// Adds the music from the files and the file name becomes the name of the music in game
		/// </summary>
		/// <param name="m"></param>
		public void Music(string m)
		{
			cm.AddMusic(m);
		}

		/// <summary>
		/// Adds music from files but the name is custom
		/// </summary>
		/// <param name="m"></param>
		public void Music(string m, string path)
		{
			cm.AddMusic(m, path);
		}

		/// <summary>
		/// Adds a custom rectanlge texture to the game
		/// </summary>
		/// <param name="n"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="color"></param>
		public void Rectangle(string n, int w, int h, QColor color)
		{
			var t = new Texture2D(cm.Engine.GraphicsDevice, w, h);
			var c = new QColor[w * h];
			for(var i = 0; i < c.Length; i++)
				c[i] = color;
			t.SetData(c);
			if(!cm.AddCustomTexture(n, t))
			{
				t.Dispose();
			}
		}

		/// <summary>
		/// Adds a custom circle texture to the game
		/// </summary>
		/// <param name="name"></param>
		/// <param name="radius"></param>
		/// <param name="color"></param>
		public void Circle(string name, int radius, QColor? color = null)
		{
			if(color == null)
				color = QColor.White;

			Texture2D CreateCircle(GraphicsDevice g, int r, QColor c)
			{
				int outerRadius = r * 2 + 2; // So circle doesn't go out of bounds
				Texture2D texture = new Texture2D(g, outerRadius, outerRadius);

				QColor[] data = new QColor[outerRadius * outerRadius];

				// Colour the entire texture transparent first.
				for(int i = 0; i < data.Length; i++)
					data[i] = QColor.Transparent;

				// Work out the minimum step necessary using trigonometry + sine approximation.
				double angleStep = 1f / r;

				for(double angle = 0; angle < Math.PI * 2; angle += angleStep)
				{
					// Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
					int x = (int)Math.Round(r + r * Math.Cos(angle));
					int y = (int)Math.Round(r + r * Math.Sin(angle));

					data[y * outerRadius + x + 1] = QColor.White;
				}

				bool finished = false;
				int firstSkip = 0;
				int lastSkip = 0;
				for(int i = 0; i <= data.Length - 1; i++)
				{
					if(finished == false)
					{
						//T = transparent W = White;
						//Find the First Batch of Colors TTTTWWWTTTT The top of the circle
						if((data[i] == QColor.White) && (firstSkip == 0))
						{
							while(data[i + 1] == QColor.White)
							{
								i++;
							}
							firstSkip = 1;
							i++;
						}
						//Now Start Filling                       TTTTTTTTWWTTTTTTTT
						//circle in Between                       TTTTTTW--->WTTTTTT
						//transaparent blancks                    TTTTTWW--->WWTTTTT
						//                                        TTTTTTW--->WTTTTTT
						//                                        TTTTTTTTWWTTTTTTTT
						if(firstSkip == 1)
						{
							if(data[i] == QColor.White && data[i + 1] != QColor.White)
							{
								i++;
								while(data[i] != QColor.White)
								{
									//Loop to check if its the last row of pixels
									//We need to check this because of the 
									//int outerRadius = radius * 2 + -->'2'<--;
									for(int j = 1; j <= outerRadius; j++)
									{
										if(data[i + j] != QColor.White)
										{
											lastSkip++;
										}
									}
									//If its the last line of pixels, end drawing
									if(lastSkip == outerRadius)
									{
										break;
										finished = true;
									}
									else
									{
										data[i] = QColor.White;
										i++;
										lastSkip = 0;
									}
								}
								while(data[i] == QColor.White)
								{
									i++;
								}
								i--;
							}
						}
					}
				}

				texture.SetData(data);
				return texture;
			}


			var t = CreateCircle(cm.Engine.GraphicsDevice, radius, color.Value);
			cm.AddCustomTexture(name, t);
		}

		internal QAddContent(QContentManager cm)
		{
			this.cm = cm;
		}
	}
}

// retarded code not mine
//			Texture2D createCircleText(GraphicsDevice g, int diameter, QColor c)
//			{
//				Texture2D texture = new Texture2D(g, r, r);
//				QColor[] colorData = new QColor[r * r];
//
//				float diam = r / 2f;
//				float diamsq = diam * diam;
//
//				for(int x = 0; x < r; x++)
//				{
//					for(int y = 0; y < r; y++)
//					{
//						int index = x * r + y;
//						QVec pos = new QVec(x - diam, y - diam);
//						if(pos.LengthSquared() <= diamsq)
//						{
//							colorData[index] = c;
//						}
//						else
//						{
//							colorData[index] = QColor.Transparent;
//						}
//					}
//				}