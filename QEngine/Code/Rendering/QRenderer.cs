using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	/// <summary>
	/// Base renderer class
	/// </summary>
	public abstract class QRenderer
	{
		internal SpriteBatch sb { get; }

		internal GraphicsDevice gd => sb.GraphicsDevice;

		internal Effect basicEffect;

		internal QEngine Engine;

		internal float State;

		public QColor ClearColor { get; set; } = QColor.White;

		internal virtual void Begin()
		{
			sb.Begin();
		}

//		/// <summary>
//		/// Draws filled in circle
//		/// </summary>
//		/// <param name="t"></param>
//		/// <param name="radius"></param>
//		/// <param name="c"></param>
//		public void DrawCircle(QTransform t, float radius, QColor c)
//		{
//			sb.DrawCircle(t.Position, radius, 20, c, radius);
//		}
//
//		/// <summary>
//		/// Assumes that the rectangle origin is in the middle
//		/// </summary>
//		/// <param name="t"></param>
//		/// <param name="width"></param>
//		/// <param name="height"></param>
//		/// <param name="color"></param>
//		public void DrawRect(QTransform t, float width, float height, QColor color)
//		{
//			var v = t.Position;
//			//sb.DrawRectangle(v - new QVec(width, height)/2f, new QVec(width, height), color);
//			sb.FillRectangle(v - new QVec(width, height)/2f, new QVec(width, height), color);
//			//sb.FillRectangle(t.Position.X, t.Position.Y, width, height, color, t.Rotation);
//		}

		internal virtual void Draw(QTexture t, QVector2 pos, QColor c)
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
			sb = e.MonoSpriteBatch;
			Engine = e;
			basicEffect = new BasicEffect(sb.GraphicsDevice);
		}
	}
}