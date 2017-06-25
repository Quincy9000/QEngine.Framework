using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public class QGuiRenderer : QRenderer
	{
		internal override void Begin()
		{
			base.Begin();
		}

		public void DrawImage(QImage i, QTransform t)
		{
			sb.Draw(i.Texture, t.Position + i.Offset, i.Source, i.Color, t.Rotation, i.Origin, t.Scale, SpriteEffects.None, i.Layer);
		}

		public void DrawImage(QImage i, QTransform t, QVec pos)
		{
			sb.Draw(i.Texture, pos + i.Offset, i.Source, i.Color, t.Rotation, i.Origin, t.Scale, SpriteEffects.None, i.Layer);
		}

		public void DrawString(QLabel label, QTransform pos)
		{
			sb.DrawString(label.Font, label.Text, pos.Position, label.Color, pos.Rotation, QVec.Zero, pos.Scale, SpriteEffects.None, label.Layer);
		}

		public void DrawString(QLabel label, QVec pos, QTransform t)
		{
			sb.DrawString(label.Font, label.Text, pos, label.Color, t.Rotation, QVec.Zero, t.Scale, SpriteEffects.None, label.Layer);
		}

		public void DrawString(QLabel label, QVec pos, QTransform t, float fade)
		{
			sb.DrawString(label.Font, label.Text, pos, label.Color * fade, t.Rotation, QVec.Zero, t.Scale, SpriteEffects.None, label.Layer);
		}

		internal override void End()
		{
			base.End();
		}

		internal QGuiRenderer(QEngine e) : base(e) { }
	}
}