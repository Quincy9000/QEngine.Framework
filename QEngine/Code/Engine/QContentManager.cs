using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace QEngine
{
	/// <summary>
	/// QScene Level Content manager that keeps track of all the resources for a scene
	/// </summary>
	class QContentManager
	{
		internal QEngine Engine { get; }

		QAtlas _atlas;

		internal QAtlas Atlas
		{
			get
			{
				if(IsMegaDirty)
				{
					CreateMega();
					IsMegaDirty = false;
				}
				return _atlas;
			}
		}

		bool IsMegaDirty;

		ContentManager contentManager => Engine.Content;

		Dictionary<string, QTexture> Textures { get; }

		Dictionary<string, QSound> Sounds { get; }

		Dictionary<string, QMusic> Music { get; }

		Dictionary<string, QFont> Fonts { get; }

		T Load<T>(string path)
		{
			try
			{
				IsMegaDirty = true;
				var s = contentManager.RootDirectory;
				return contentManager.Load<T>(path);
			}
			catch(Exception)
			{
				throw new Exception("Could not find Asset!");
			}
		}

		internal void CreateMega()
		{
			//settings for the widths of the textureAtlas
			const int HighQualityWidth = 4096;
			const int HightQualityHeight = 4096;
			const int LowQualityWidth = 2048;
			const int LowQualityHeight = 2048;

			//if were on high def we can use 4096 as the max texture size
			if(QEngine.IsHighQuality)
			{
				CreateMega(HighQualityWidth, HightQualityHeight);
//				CreateMega(512, 512); //used to test when there is little space for an atlas WORKS!
			}
			else
			{
				CreateMega(LowQualityWidth, LowQualityHeight);
			}
		}

		void CreateMega(int MaxWidth, int MaxHeight)
		{
			int totalWidth = 0;
			int biggetsHeight = 0;
			//find totalwidth and biggets height
			foreach(var t in Textures.Values)
			{
				totalWidth += t.Width + 1;
				if(t.Height > biggetsHeight)
					biggetsHeight = t.Height;
			}
			//if the width of all textures is less than maxwidth we only need one line
			if(totalWidth < MaxWidth)
			{
				if(totalWidth == 0 || biggetsHeight == 0)
					return;
				var target = new RenderTarget2D(Engine.GraphicsDevice, totalWidth, biggetsHeight);
				RenderAtlas(target, biggetsHeight, MaxWidth);
			}
			//else we need to use more than one row 
			else
			{
				//should give us the number of times we need to "grow the texture downwards"
				int rowsNeeded = (int)Math.Round((double)totalWidth / MaxHeight);
				var target = new RenderTarget2D(Engine.GraphicsDevice, MaxWidth, rowsNeeded * biggetsHeight);
				RenderAtlas(target, biggetsHeight, MaxWidth);
			}
		}

		/// <summary>
		/// Takes the created target and draws to it using the space that it needed for all textures
		/// </summary>
		/// <param name="target"></param>
		/// <param name="biggetsHeight"></param>
		/// <param name="MaxWidth"></param>
		void RenderAtlas(RenderTarget2D target, int biggetsHeight, int MaxWidth)
		{
			const bool TakePictureOfAtlas = false;
			Engine.GraphicsDevice.SetRenderTargets(target);
			var render = new QSpriteRenderer(Engine);
			var rects = new Dictionary<string, QRect>();
			var pos = QVec.Zero;
			render.ClearColor = QColor.Transparent;
			render.Begin();
			var textures = Textures.ToList();
			for(int i = 0; i < textures.Count; i++)
			{
				var t = textures[i];
				if(pos.X + t.Value.Width > MaxWidth)
				{
					pos.Y += biggetsHeight;
					pos.X = 0;
				}
				render.Draw(t.Value, pos, QColor.White);
				rects.Add(t.Key, new QRect(pos, t.Value.Bounds.Size));
				pos.X += t.Value.Width + 1;
			}
			render.End();
			render.gd.SetRenderTarget(null);
			if(_atlas != null)
				((Texture2D)_atlas.Texture).Dispose();
			_atlas = new QAtlas(target, rects);
			if(TakePictureOfAtlas)
				_atlas.Texture.SaveAsPng("here.jpg");
		}

		public bool AddTexture(string name, string path)
		{
			if(!Textures.ContainsKey(name))
			{
				Textures.Add(name, Load<Texture2D>(path));
				return true;
			}
			return false;
		}

		public bool AddTexture(string path)
		{
			string name = path.Split('/').Last();
			return AddTexture(name, path);
		}

		public bool AddCustomTexture(string name, Texture2D t)
		{
			if(!Textures.ContainsKey(name))
			{
				IsMegaDirty = true;
				Textures.Add(name, t);
				return true;
			}
			return false;
		}

		public bool AddMusic(string name, string path)
		{
			if(!Music.ContainsKey(name))
			{
				Music.Add(name, Load<Song>(path));
				return true;
			}
			return false;
		}

		public bool AddMusic(string path)
		{
			string name = path.Split('/').Last();
			return AddMusic(name, path);
		}

		public bool AddFont(string name, string path)
		{
			if(!Fonts.ContainsKey(name))
			{
				Fonts.Add(name, Load<SpriteFont>(path));
				return true;
			}
			return false;
		}

		public bool AddFont(string path)
		{
			string name = path.Split('/').Last();
			return AddFont(name, path);
		}

		public bool AddSound(string name, string path)
		{
			if(!Sounds.ContainsKey(name))
			{
				Sounds.Add(name, Load<SoundEffect>(path));
				return true;
			}
			return false;
		}

		public bool AddSound(string path)
		{
			string name = path.Split('/').Last();
			return AddSound(name, path);
		}

		public QTexture GetTexture(string name)
		{
			if(!Textures.TryGetValue(name, out QTexture t))
				throw new Exception($"Cannot find texture {name}");
			return t;
		}

		public QFont GetFont(string name)
		{
			if(!Fonts.TryGetValue(name, out QFont f))
				throw new Exception($"Cannot find font {name}");
			return f;
		}

		public QMusic GetMusic(string name)
		{
			if(!Music.TryGetValue(name, out QMusic m))
				throw new Exception($"Cannot find Music {name}");
			return m;
		}

		public QSound GetSound(string name)
		{
			if(!Sounds.TryGetValue(name, out QSound s))
				throw new Exception($"Cannot find sound {name}");
			return s;
		}

		void Clear()
		{
			foreach(var texturesValue in Textures.Values)
			{
				((Texture2D)texturesValue).Dispose();
			}
			Textures.Clear();
			Sounds.Clear();
			Fonts.Clear();
			Music.Clear();
			contentManager.Unload();
		}

		public void Unload()
		{
			Clear();
		}

		internal QContentManager(QEngine engine)
		{
			Engine = engine;
			Textures = new Dictionary<string, QTexture>();
			Fonts = new Dictionary<string, QFont>();
			Sounds = new Dictionary<string, QSound>();
			Music = new Dictionary<string, QMusic>();
		}
	}
}

//			Engine.GraphicsDevice.SetRenderTargets(target);
//			var render = new QSpriteRenderer(Engine);
//			var rects = new Dictionary<string, QRect>();
//			var pos = QVec.Zero;
//			render.ClearColor = QColor.Transparent;
//			render.Begin();
//			var textures = Textures.ToList();
//			for(int i = 0; i < textures.Count; i++)
//			{
//				var t = textures[i];
//				render.Draw(t.Value, pos, QColor.White);
//				rects.Add(t.Key, new QRect(pos, t.Value.Bounds.Size));
//				pos.X += t.Value.Width + 1;
//			}
//			render.End();
//			render.gd.SetRenderTarget(null);
//			if(_atlas != null)
//				((Texture2D)_atlas.Texture).Dispose();
//			_atlas = new QAtlas(target, rects);
//			Thread.Sleep(100);
//			_atlas.Texture.SaveAsPng("here.jpg");

//			int MaxWidth = w;
//			int MaxHeight = h;
//			int TotalWidth = 0;
//			int TotalHeight = 0;
//			int maxTextureHeight = 0;
//			//this loop just finds the biggest height,
//			//so we know how far to go down in the next row for now overlap
//			foreach(var t in Textures.Values)
//			{
//				if(t.Height > maxTextureHeight)
//					maxTextureHeight = t.Height;
//				TotalWidth += t.Width + 1;
//				TotalHeight += t.Height + 1;
//			}
//			if(TotalWidth == 0 || TotalHeight == 0)
//				return;
//			int TimesSplitWidthSplit = TotalWidth / MaxWidth;
//			var render = new QSpriteRenderer(Engine);
//			var pos = QVec.Zero;
//			//if the current width is less than the max, use the current else use max
//			RenderTarget2D target;
//			if(TotalWidth < MaxWidth)
//			{
//				target = new RenderTarget2D(render.gd, TotalWidth, TotalHeight);
//			}
//			else
//			{
//				target = new RenderTarget2D(render.gd, MaxWidth, TimesSplitWidthSplit);
//			}
//			var rects = new Dictionary<string, QRect>();
//			render.gd.SetRenderTarget(target);
//			render.ClearColor = QColor.Transparent;
//			render.Begin();
//			var textures = Textures.ToList();
//			for(int i = 0; i < textures.Count; i++)
//			{
//				KeyValuePair<string, QTexture> t = textures[i];
//				//If the texture flows over the width limit
//				//we move down a row of the biggets texture height
//				if(pos.X + t.Value.Width > MaxWidth)
//				{
//					pos.Y += maxTextureHeight;
//					pos.X = 0;
//				}
//				//draw the image then move over to the right
//				render.Draw(t.Value, pos, QColor.White);
//				rects.Add(t.Key, new QRect(pos, t.Value.Bounds.Size));
//				pos.X += t.Value.Width + 1;
//			}
//			render.End();
//			render.gd.SetRenderTarget(null);
//			if(_atlas != null)
//				((Texture2D)_atlas.Texture).Dispose();
//			_atlas = new QAtlas(target, rects);
//			Thread.Sleep(100);
//			_atlas.Texture.SaveAsPng("here.jpg");