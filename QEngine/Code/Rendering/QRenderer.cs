using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public abstract class QRenderer
	{
		internal SpriteBatch sb { get; }

		internal GraphicsDevice gd => sb.GraphicsDevice;

		public QColor ClearColor { get; set; } = QColor.White;

		internal virtual void Begin()
		{
			sb.Begin();
		}

		internal virtual void Draw(QTexture t, QVec pos, QColor c)
		{
			sb.Draw(t, pos, c);
		}

		internal virtual void End()
		{
			sb.End();
		}

		internal virtual void ClearScreen(QColor color)
		{
			gd.Clear(color);
		}

		internal virtual void ClearScreen()
		{
			gd.Clear(ClearColor);
		}

		internal QRenderer(QEngine e)
		{
			sb = e.SpriteBatch;
		}
	}
}