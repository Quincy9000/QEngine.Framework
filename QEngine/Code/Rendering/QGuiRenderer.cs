using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public class QGuiRenderer : QRenderer
	{
		public QSortOrders Order
		{
			get
			{
				switch(sortMode)
				{
					case SpriteSortMode.Deferred:
						return QSortOrders.DontCare;
					case SpriteSortMode.FrontToBack:
						return QSortOrders.StartAtOne;
					case SpriteSortMode.BackToFront:
						return QSortOrders.StartAtZero;
					default:
						return QSortOrders.DontCare;
				}
			}
			set
			{
				switch(value)
				{
					case QSortOrders.DontCare:
						sortMode = SpriteSortMode.Deferred;
						break;
					case QSortOrders.StartAtOne:
						sortMode = SpriteSortMode.FrontToBack;
						break;
					case QSortOrders.StartAtZero:
						sortMode = SpriteSortMode.BackToFront;
						break;
					default:
						sortMode = SpriteSortMode.Deferred;
						break;
				}
			}
		}

		public QFilterStates Filter
		{
			get => filterState;
			set
			{
				switch(value)
				{
					case QFilterStates.Ansiotrophic:
					{
						samplerState = SamplerState.AnisotropicClamp;
						break;
					}
					case QFilterStates.Linear:
					{
						samplerState = SamplerState.LinearClamp;
						break;
					}
					case QFilterStates.Point:
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

		protected QFilterStates filterState;

		protected SpriteSortMode sortMode;

		protected SamplerState samplerState = SamplerState.AnisotropicClamp;

		public QEffect Effect { get; set; } = null;

		public QMatrix Matrix { get; set; } = QMatrix.Identity;

		internal override void Begin()
		{
			if(Effect != null)
				sb.Begin(sortMode, null, samplerState, null, null, Effect, Matrix);
			else
				sb.Begin(sortMode, null, samplerState, null, null, null, Matrix);
		}

		public void DrawImage(QImage i, QTransform t)
		{
			sb.Draw(i.Texture, t.Position + i.Offset, i.Source, i.Color, t.Rotation, i.Origin, i.Scale, SpriteEffects.None, i.Layer);
		}

		public void DrawImage(QImage i, QTransform t, QVector2 pos)
		{
			sb.Draw(i.Texture, pos + i.Offset, i.Source, i.Color, t.Rotation, i.Origin, i.Scale, SpriteEffects.None, i.Layer);
		}

		public void DrawString(QLabel label)
		{
			if(label.Visible)
				sb.DrawString(label.Font, label.Text, label.Position, label.Color, label.Rotation, QVector2.Zero, label.Scale, SpriteEffects.None, label.Layer);
		}

		public void DrawString(QLabel label, QTransform pos)
		{
			if(label.Visible)
				sb.DrawString(label.Font, label.Text, pos.Position, label.Color, pos.Rotation, QVector2.Zero, label.Scale, SpriteEffects.None, label.Layer);
		}

		public void DrawString(QLabel label, QVector2 pos, QTransform t)
		{
			if(label.Visible)
				sb.DrawString(label.Font, label.Text, pos, label.Color, t.Rotation, QVector2.Zero, label.Scale, SpriteEffects.None, label.Layer);
		}

		public void DrawString(QLabel label, QVector2 pos, QTransform t, float fade)
		{
			if(label.Visible)
				sb.DrawString(label.Font, label.Text, pos, label.Color * fade, t.Rotation, QVector2.Zero, label.Scale, SpriteEffects.None, label.Layer);
		}

		internal QGuiRenderer(QEngine e) : base(e)
		{
			Filter = QFilterStates.Point;
			Order = QSortOrders.StartAtZero;
		}
	}
}