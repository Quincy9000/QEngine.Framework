using System;
using System.Collections.Generic;
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

		QMegaTexture megaTexture;

		internal QMegaTexture MegaTexture
		{
			get
			{
				if(IsMegaDirty)
				{
					CreateMega();
					IsMegaDirty = false;
				}
				return megaTexture;
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
				return contentManager.Load<T>(path);
			}
			catch(Exception)
			{
				throw new Exception("Could not find Asset!");
			}
		}

		internal void CreateMega()
		{
			var width = 0;
			var height = 0;
			foreach(var t in Textures.Values)
			{
				width += t.Width + 1;
				if(t.Height > height)
					height = t.Height;
			}
			if(width == 0 || height == 0)
				return;
			var render = new QSpriteRenderer(Engine);
			var pos = QVec.Zero;
			var target = new RenderTarget2D(render.gd, width, height);
			var rects = new Dictionary<string, QRect>();
			render.gd.SetRenderTarget(target);
			render.ClearColor = QColor.Transparent;
			render.Begin();
			foreach(var t in Textures)
			{
				render.Draw(t.Value, pos, QColor.White);
				rects.Add(t.Key, new QRect(pos, t.Value.Bounds.Size));
				pos.X += t.Value.Width + 1;
			}
			render.End();
			render.gd.SetRenderTarget(null);
			if(megaTexture != null)
				((Texture2D)megaTexture.Texture).Dispose();
			megaTexture = new QMegaTexture(target, rects);
			megaTexture.Texture.SaveAsPng("here.png");
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