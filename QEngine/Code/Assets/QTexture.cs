using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public class QTexture : IDisposable
	{
		readonly Texture2D _texture;

		internal QContentManager Manager { get; }

		public string Name => _texture.Name;

		public QRectangle Bounds => _texture.Bounds;

		public int Width => _texture.Width;

		public int Height => _texture.Height;

		public QColor[] GetPixels()
		{
			var c = new QColor[Width * Height];
			_texture.GetData(c);
			return c;
		}

		public QTexture GetPartialTexture(QRectangle source)
		{
			Texture2D t = new Texture2D(_texture.GraphicsDevice, source.Width, source.Height);
			QColor[] colors = new QColor[t.Width * t.Height];
			_texture.GetData(0, source, colors, 0, colors.Length);
			t.SetData(0, source, colors, 0, colors.Length);
			return new QTexture(Manager, t);
		}

		public void SaveAsPng(string path)
		{
			using(var file = new FileStream(path, FileMode.OpenOrCreate))
			{
				_texture.SaveAsPng(file, _texture.Width, _texture.Height);
			}
		}

		public void SaveAsJpeg(string path)
		{
			using(var file = new FileStream(path, FileMode.OpenOrCreate))
			{
				_texture.SaveAsJpeg(file, _texture.Width, _texture.Height);
			}
		}

		public static implicit operator Texture2D(QTexture t) => t._texture;

		public void Dispose()
		{
			_texture.Dispose();
		}

		internal QTexture(QContentManager manager, Texture2D t)
		{
			_texture = t;
			Manager = manager;
			Manager.LoadCustomTexture(Name, t);
		}
	}
}
