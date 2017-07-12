using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	/// <summary>
	/// Texture Atlas
	/// </summary>
	public class QMegaTexture
	{
		internal QMegaTexture(QTexture texture, Dictionary<string, QRect> rects)
		{
			Texture = texture;
			Rectangles = rects;
		}

		public static implicit operator QTexture(QMegaTexture m) => m.Texture;

		public static implicit operator Texture2D(QMegaTexture m) => m.Texture;

		internal QTexture Texture { get; set; }

		internal Dictionary<string, QRect> Rectangles { get; set; }

		public QRect this[string index] => Rectangles[index];
	}
}