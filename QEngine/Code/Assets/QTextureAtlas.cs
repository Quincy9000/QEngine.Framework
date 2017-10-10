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

		QEngine Engine { get; }

		QContentManager ContentManager { get; }

		public static implicit operator QTexture(QTextureAtlas m) => m.Texture;

		public static implicit operator Texture2D(QTextureAtlas m) => m.Texture;

		public QTexture Texture { get; private set; }

		public Dictionary<string, QRectangle> Rectangles { get; private set; }

		public QRectangle this[string index] => Rectangles[index];

		void CreateAtlas()
		{
			//if were on high def we can use 4096 as the max texture size
			if(QEngine.IsHighQuality)
			{
				CreateAtlas(HighQualityWidth, HightQualityHeight);
			}
			else
			{
				CreateAtlas(LowQualityWidth, LowQualityHeight);
			}
		}

		void CreateAtlas(int maxWidth, int maxHeight)
		{
			Dictionary<string, QTexture> textures = ContentManager.Textures;
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
				var target = new RenderTarget2D(Engine.GraphicsDevice, totalWidth, biggestHeight);
				RenderAtlas(target, biggestHeight, maxWidth);
			}
			//else we need to use more than one row 
			else
			{
				//should give us the number of times we need to "grow the texture downwards"
				int rowsNeeded = (int)Math.Round((double)totalWidth / maxHeight);
				//take biggest textureHeight and mult by rows needed
				var target = new RenderTarget2D(Engine.GraphicsDevice, maxWidth, rowsNeeded * biggestHeight);
				RenderAtlas(target, biggestHeight, maxWidth);
			}
		}

		/// <summary>
		/// Takes the created target and draws to it using the space that it needed for all textures
		/// </summary>
		/// <param name="target"></param>
		/// <param name="biggetsHeight"></param>
		/// <param name="maxWidth"></param>
		void RenderAtlas(RenderTarget2D target, int biggetsHeight, int maxWidth)
		{
			//slows down code but useful for debugging
			const bool takePictureOfAtlas = true;
			target.GraphicsDevice.SetRenderTargets(target);
			var render = new QSpriteRenderer(Engine);
			Rectangles = new Dictionary<string, QRectangle>();
			var pos = QVector2.Zero;
			render.ClearColor = QColor.Transparent;
			render.Begin();
			var textures = ContentManager.Textures.ToList();
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
			Texture = new QTexture(ContentManager, target);
			if(takePictureOfAtlas)
				Texture.SaveAsPng($"{DateTime.Now.Millisecond}.png");
		}
		
		/// <summary>
		/// Measures how many atlases are needed depending on the system
		/// </summary>
		/// <param name="manager"></param>
		/// <returns></returns>
		static int AtlasesNeeded(QContentManager manager)
		{
			//number of atlases
			int atlasCount = 0;
			//how big the texture can get before resettting
			int textureMaxWidth = 0;
			//how big the texture can get before resettting
			int textureMaxHeight = 0;
			//the height needed for the current atlas; last one will always be the shortest
			int biggestHeight = 0;
			int totalWidth = 0;

			//If the current system is high quality we can use bigger textures because of max limits on textures
			if(QEngine.IsHighQuality)
			{
				textureMaxWidth = HighQualityWidth;
				textureMaxHeight = HightQualityHeight;
			}
			else
			{
				textureMaxWidth = LowQualityWidth;
				textureMaxHeight = LowQualityHeight;
			}

			//finds the width all the textures take up side by side, and finds the biggest ones height
			foreach(var texture in manager.Textures.Values)
			{
				totalWidth += texture.Width;
				if(texture.Height > biggestHeight)
					biggestHeight = texture.Height;
			}

			int rowsNeeded = (int)Math.Round((double)totalWidth / textureMaxHeight);

			int totalHeight = rowsNeeded * biggestHeight;

			//gives us the sheets needed but without remainder
			atlasCount = totalHeight / textureMaxHeight;
			//then gives use the actual remainder
			int remainder = totalHeight % textureMaxHeight;

			//if the remainder exist then we need another texture
			if(remainder > 0)
			{
				atlasCount++;
			}

			return atlasCount;
		}

		/// <summary>
		/// Returns list that may contain one or more atlases depending on the texture sizes
		/// </summary>
		/// <param name="world">uses the current world to create the atlases</param>
		/// <returns></returns>
		public static List<QTextureAtlas> CreateAtlases(QWorld world)
		{
			var dict = new List<QTextureAtlas>();
			int atlasCount = AtlasesNeeded(world.Content);
			for(int i = 0; i < atlasCount; i++)
			{
				//TODO NEED TO add the atlases depending on the textures!
				dict.Add(new QTextureAtlas(world.Engine, world.Content));
			}
			return dict;
		}

		QTextureAtlas(QEngine engine, QContentManager contentManager)
		{
			Engine = engine;
			ContentManager = contentManager;
			CreateAtlas();
		}
	}
}