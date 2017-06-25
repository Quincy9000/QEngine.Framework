using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	/// <summary>
	/// Loaded from a spritefont
	/// </summary>
	public class QFont
	{
		SpriteFont font;

		public float Spacing => font.Spacing;

		public QVec MeasureString(string s) => font.MeasureString(s);

		public QVec MeasureString(StringBuilder s) => font.MeasureString(s);

		public int LineSpacing => font.LineSpacing;

		public static implicit operator SpriteFont(QFont f) => f.font;

		public static implicit operator QFont(SpriteFont f) => new QFont(f);

		public QFont(SpriteFont f)
		{
			font = f;
		}
	}
}
