using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public class QSpriteRenderer : QRenderer
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
			ClearScreen();
			if(Effect != null)
				sb.Begin(sortMode, null, samplerState, null, null, Effect, Matrix);
			else
				sb.Begin(sortMode, null, samplerState, null, null, null, Matrix);
		}

		public void Draw(QTexture t, QSprite s, QTransform tr)
		{
			if(!s.IsVisible) return;
			sb.Draw(t, tr.Position + s.Offset, s.Source, s.Color, tr.Rotation, s.Origin, tr.Scale, (SpriteEffects)s.Effect, s.Layer.condensedValue);
		}

		public void Draw(QSprite s, QTransform t)
		{
			if(!s.IsVisible) return;
			sb.Draw(s.Texture, t.Position + s.Offset, s.Source, s.Color, t.Rotation, s.Origin, t.Scale, (SpriteEffects)s.Effect, s.Layer);
		}

		/// <summary>
		/// Draw at a different location instead of the pos in transform, but uses everything else
		/// </summary>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <param name="vec"></param>
		public void Draw(QSprite s, QTransform t, QVec vec)
		{
			if(!s.IsVisible) return;
			sb.Draw(s.Texture, vec + s.Offset, s.Source, s.Color, t.Rotation, s.Origin, t.Scale, (SpriteEffects)s.Effect, s.Layer);
		}

		internal override void End()
		{
			sb.End();
		}

		internal QSpriteRenderer(QEngine e) : base(e)
		{
			ClearColor = Color.CornflowerBlue;
		}
	}
}