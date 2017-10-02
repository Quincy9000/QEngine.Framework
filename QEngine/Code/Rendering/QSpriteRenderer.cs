using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	public class QSpriteRenderer : QRenderer
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

		public QEffect Effect { get; set; } = new QEffect(null);

		public QMatrix Matrix { get; set; } = QMatrix.Identity;

		/// <summary>
		/// Clears the screen and begins a new draw batch
		/// </summary>
		internal override void Begin()
		{
			ClearScreen();
			sb.Begin(sortMode, null, samplerState, null, null, Effect ?? null, Matrix);
		}

		/// <summary>
		/// Use custom texture to draw to screen but doesnt use built in optimization
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="sprite"></param>
		/// <param name="tr"></param>
		public void Draw(QTexture texture, QSprite sprite, QTransform tr)
		{
			if(!sprite.Visible) return;
			sb.Draw(texture,
				(tr.Position + sprite.Offset),
				sprite.Source,
				sprite.Color,
				tr.Rotation,
				sprite.Origin,
				sprite.Scale,
				(SpriteEffects)sprite.Effect,
				sprite.Layer);
		}

		/// <summary>
		/// uses sprite to draw to screen using render optimization
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="transform"></param>
		public void Draw(QSprite sprite, QTransform transform)
		{
			if(!sprite.Visible) return;
			sb.Draw(sprite.Texture,
				(transform.Position + sprite.Offset),
				sprite.Source,
				sprite.Color,
				transform.Rotation,
				sprite.Origin,
				sprite.Scale,
				(SpriteEffects)sprite.Effect,
				sprite.Layer);
		}

		/// <summary>
		/// use sprite to draw but use different source than the sprites
		/// </summary>
		/// <param name="source"></param>
		/// <param name="sprite"></param>
		/// <param name="t"></param>
		/// <param name="pos"></param>
		public void Draw(QRectangle source, QSprite sprite, QTransform t, QVector2 pos)
		{
			if(!sprite.Visible) return;
			sb.Draw(sprite.Texture,
				pos,
				source,
				sprite.Color,
				t.Rotation,
				sprite.Origin,
				sprite.Scale,
				(SpriteEffects)sprite.Effect,
				sprite.Layer);
		}

		/// <summary>
		/// Custom draw method using built in megatexture, so you need to use a textureSource from QGetContent
		/// </summary>
		/// <param name="position"></param>
		/// <param name="source"></param>
		/// <param name="color"></param>
		/// <param name="rotation"></param>
		/// <param name="origin"></param>
		/// <param name="scale"></param>
		/// <param name="effect"></param>
		/// <param name="layer"></param>
		public void Draw(QVector2 position, QRectangle source, QColor color, float rotation, QVector2 origin, QVector2 scale,
			QRenderEffects effect, float layer)
		{
			sb.Draw(Engine.Manager.CurrentWorld.TextureAtlas,
				position,
				source,
				color,
				rotation,
				origin,
				scale,
				(SpriteEffects)effect,
				layer);
		}

		/// <summary>
		/// Draw at a different location instead of the pos in transform, but uses everything else
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="transform"></param>
		/// <param name="position"></param>
		public void Draw(QSprite sprite, QTransform transform, QVector2 position)
		{
			if(!sprite.Visible) return;
			sb.Draw(sprite.Texture,
				position + sprite.Offset,
				sprite.Source,
				sprite.Color,
				transform.Rotation,
				sprite.Origin,
				sprite.Scale,
				(SpriteEffects)sprite.Effect,
				sprite.Layer);
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