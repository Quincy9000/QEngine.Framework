using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public class QTexture : IDisposable
	{
		Texture2D texture;

		public string Name => texture.Name;

		public QRect Bounds => texture.Bounds;

		public int Width => texture.Width;

		public int Height => texture.Height;

		public QColor[] GetPixels()
		{
			var c = new QColor[Width * Height];
			texture.GetData(c);
			return c;
		}

		public QTexture GetPartialTexture(QRect source)
		{
			Texture2D t = new Texture2D(texture.GraphicsDevice, source.Width, source.Height);
			QColor[] colors = new QColor[t.Width * t.Height];
			texture.GetData(0, source, colors, 0, colors.Length);
			t.SetData(0, source, colors, 0, colors.Length);
			return t;
		}

		public void SaveAsPng(string path)
		{
			using(var file = new FileStream(path, FileMode.OpenOrCreate))
			{
				texture.SaveAsPng(file, texture.Width, texture.Height);
			}
		}

		public void SaveAsJpeg(string path)
		{
			using(var file = new FileStream(path, FileMode.OpenOrCreate))
			{
				texture.SaveAsJpeg(file, texture.Width, texture.Height);
			}
		}

		public static implicit operator Texture2D(QTexture t) => t.texture;

		public static implicit operator QTexture(Texture2D t) => new QTexture(t);

		public void Dispose()
		{
			texture.Dispose();
		}

		internal QTexture(Texture2D t)
		{
			texture = t;
		}
	}
}
