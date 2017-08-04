using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
			const int HighQualityWidth = 4096;
			const int HightQualityHeight = 4096;
			const int LowQualityWidth = 2048;
			const int LowQualityHeight = 2048;

			//if were on high def we can use 4096 as the max texture size
			if(QEngine.IsHighQuality)
			{
				CreateMega(HighQualityWidth, HightQualityHeight);
			}
			else
			{
				CreateMega(LowQualityWidth, LowQualityHeight);
			}
		}

		void CreateMega(int w, int h)
		{
			int MaxWidth = w;
			int MaxHeight = h;
			var maxTextureHeight = 0;
			foreach(var t in Textures.Values)
			{
//				width += t.Width + 1;
				if(t.Height > maxTextureHeight)
					maxTextureHeight = t.Height;
			}
//			if(width == 0 || height == 0)
//				return;
			var render = new QSpriteRenderer(Engine);
			var pos = QVec.Zero;
			var target = new RenderTarget2D(render.gd, MaxWidth, MaxHeight);
			var rects = new Dictionary<string, QRect>();
			render.gd.SetRenderTarget(target);
			render.ClearColor = QColor.Transparent;
			render.Begin();
			var textures = Textures.ToList();
			for(int i = 0; i < textures.Count; i++)
			{
				KeyValuePair<string, QTexture> t = textures[i];
				//If the texture flows over the width limit
				//we move down a row of the biggets texture height
				if(pos.X + t.Value.Width > MaxWidth)
				{
					pos.Y += maxTextureHeight;
					pos.X = 0;
				}
				//draw the image then move over to the right
				render.Draw(t.Value, pos, QColor.White);
				rects.Add(t.Key, new QRect(pos, t.Value.Bounds.Size));
				pos.X += t.Value.Width + 1;
			}
			render.End();
			render.gd.SetRenderTarget(null);
			if(_atlas != null)
				((Texture2D)_atlas.Texture).Dispose();
			_atlas = new QAtlas(target, rects);
			_atlas.Texture.SaveAsJpeg("here.jpg");
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