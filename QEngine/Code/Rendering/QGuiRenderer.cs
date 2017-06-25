using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public class QGuiRenderer : QRenderer
	{
		public QSortOrder Order
		{
			get
			{
				switch(sortMode)
				{
					case SpriteSortMode.Deferred:
						return QSortOrder.DontCare;
					case SpriteSortMode.FrontToBack:
						return QSortOrder.StartAtOne;
					case SpriteSortMode.BackToFront:
						return QSortOrder.StartAtZero;
					default:
						return QSortOrder.DontCare;
				}
			}
			set
			{
				switch(value)
				{
					case QSortOrder.DontCare:
						sortMode = SpriteSortMode.Deferred;
						break;
					case QSortOrder.StartAtOne:
						sortMode = SpriteSortMode.FrontToBack;
						break;
					case QSortOrder.StartAtZero:
						sortMode = SpriteSortMode.BackToFront;
						break;
					default:
						sortMode = SpriteSortMode.Deferred;
						break;
				}
			}
		}

		public QFilteringState Filter
		{
			get => filterState;
			set
			{
				switch(value)
				{
					case QFilteringState.Ansiotrophic:
					{
						samplerState = SamplerState.AnisotropicClamp;
						break;
					}
					case QFilteringState.Linear:
					{
						samplerState = SamplerState.LinearClamp;
						break;
					}
					case QFilteringState.Point:
					{
						samplerState = SamplerState.PointClamp;
						break;
					}
					default:
						samplerState = SamplerState.AnisotropicClamp;
						break;
				}
				filterState = value;
			}
		}

		protected QFilteringState filterState;

		protected SpriteSortMode sortMode;

		protected SamplerState samplerState = SamplerState.AnisotropicClamp;

		public QEffect Effect { get; set; } = null;

		public QMat Matrix { get; set; } = QMat.Identity;

		internal override void Begin()
		{
			if(Effect != null)
				sb.Begin(sortMode, null, samplerState, null, null, Effect, Matrix);
			else
				sb.Begin(sortMode, null, samplerState, null, null, null, Matrix);
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

		internal QGuiRenderer(QEngine e) : base(e)
		{
			Filter = QFilteringState.Point;
			Order = QSortOrder.StartAtZero; 
		}
	}
}