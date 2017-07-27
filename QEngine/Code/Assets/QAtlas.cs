using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	/// <summary>
	/// Texture Atlas
	/// </summary>
	public class QAtlas
	{
		internal QAtlas(QTexture texture, Dictionary<string, QRect> rects)
		{
			Texture = texture;
			Rectangles = rects;
		}

		public static implicit operator QTexture(QAtlas m) => m.Texture;

		public static implicit operator Texture2D(QAtlas m) => m.Texture;

		internal QTexture Texture { get; set; }

		internal Dictionary<string, QRect> Rectangles { get; set; }

		public QRect this[string index] => Rectangles[index];
	}
}