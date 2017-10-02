using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	/// <summary>
	/// Texture that acts as an atlas for many sprites, used for performance reasons
	/// </summary>
	public class QTextureAtlas
	{
		/*
			settings for the widths of the textureAtlas
		*/

		const int HighQualityWidth = 4096;
		const int HightQualityHeight = 4096;
		const int LowQualityWidth = 2048;
		const int LowQualityHeight = 2048;

		public static implicit operator QTexture(QTextureAtlas m) => m.Texture;

		public static implicit operator Texture2D(QTextureAtlas m) => m.Texture;

		public QTexture Texture { get; private set; }

		public Dictionary<string, QRectangle> Rectangles { get; private set; }

		public QRectangle this[string index] => Rectangles[index];

		void CreateAtlas(QEngine engine, QContentManager manager)
		{
			//if were on high def we can use 4096 as the max texture size
			if(QEngine.IsHighQuality)
			{
				CreateAtlas(engine, manager, HighQualityWidth, HightQualityHeight);
			}
			else
			{
				CreateAtlas(engine, manager, LowQualityWidth, LowQualityHeight);
			}
		}

		void CreateAtlas(QEngine engine, QContentManager manager, int maxWidth, int maxHeight)
		{
			Dictionary<string, QTexture> textures = manager.Textures;
			int totalWidth = 0;
			int biggestHeight = 0;
			//find totalwidth and biggest height
			foreach(var t in textures.Values)
			{
				//+1 because we want a pixel between textures so that theres no overlap
				totalWidth += t.Width + 1;
				if(t.Height > biggestHeight)
					biggestHeight = t.Height;
			}
			//if the width of all textures is less than maxwidth we only need one line
			if(totalWidth < maxWidth)
			{
				if(totalWidth == 0 || biggestHeight == 0)
					return;
				var target = new RenderTarget2D(engine.GraphicsDevice, totalWidth, biggestHeight);
				RenderAtlas(engine, manager, target, biggestHeight, maxWidth);
			}
			//else we need to use more than one row 
			else
			{
				//should give us the number of times we need to "grow the texture downwards"
				int rowsNeeded = (int)Math.Round((double)totalWidth / maxHeight);
				//take biggest textureHeight and mult by rows needed
				var target = new RenderTarget2D(engine.GraphicsDevice, maxWidth, rowsNeeded * biggestHeight);
				RenderAtlas(engine, manager, target, biggestHeight, maxWidth);
			}
		}

		/// <summary>
		/// Takes the created target and draws to it using the space that it needed for all textures
		/// </summary>
		/// <param name="target"></param>
		/// <param name="biggetsHeight"></param>
		/// <param name="maxWidth"></param>
		void RenderAtlas(QEngine engine, QContentManager manager, RenderTarget2D target, int biggetsHeight, int maxWidth)
		{
			//slows down code but useful for debugging
			const bool takePictureOfAtlas = false;
			target.GraphicsDevice.SetRenderTargets(target);
			var render = new QSpriteRenderer(engine);
			Rectangles = new Dictionary<string, QRectangle>();
			var pos = QVector2.Zero;
			render.ClearColor = QColor.Transparent;
			render.Begin();
			var textures = manager.Textures.ToList();
			for(int i = 0; i < textures.Count; i++)
			{
				var t = textures[i];
				if(pos.X + t.Value.Width > maxWidth)
				{
					pos.Y += biggetsHeight;
					pos.X = 0;
				}
				render.Draw(t.Value, pos, QColor.White);
				Rectangles.Add(t.Key, new QRectangle(pos, t.Value.Bounds.Size));
				pos.X += t.Value.Width + 1;
			}
			render.End();
			render.gd.SetRenderTarget(null);
			if(Texture != null)
				((Texture2D)Texture).Dispose();
			Texture = new QTexture(manager, target);
			if(takePictureOfAtlas)
				Texture.SaveAsPng("here.png");
		}

		internal QTextureAtlas(QEngine engine, QContentManager manager)
		{
			CreateAtlas(engine, manager);
		}
	}
}