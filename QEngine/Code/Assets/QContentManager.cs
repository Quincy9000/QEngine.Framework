using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using QEngine.Exceptions;

namespace QEngine
{
	/// <summary>
	/// QScene Level Content manager that keeps track of all the resources for a scene
	/// </summary>
	public class QContentManager
	{
		ContentManager MonoContentManager { get; }

		public static QTexture UnknownTexture { get; private set; }
		
		internal GraphicsDevice MonoGraphicsDevice { get; }

		internal Dictionary<string, QTexture> Textures { get; }

		internal Dictionary<string, QSound> Sounds { get; }

		internal Dictionary<string, QMusic> Music { get; }

		internal Dictionary<string, QFont> Fonts { get; }
		
		internal List<QTextureAtlas> Atlases { get; set; }

		T Load<T>(string path)
		{
			try
			{
				return MonoContentManager.Load<T>(path);
			}
			catch(Exception e)
			{
				throw new QMissingContentException($"{path} could not find Asset!\n" + e);
			}
		}

		internal bool LoadTexture(string name, string path)
		{
			if(!Textures.ContainsKey(name))
			{
				QTexture t = new QTexture(this, Load<Texture2D>(path));
				Textures.Add(name, t);
				return true;
			}
			return false;
		}

		internal bool LoadTexture(string path)
		{
			string name = path.Split('/').Last();
			return LoadTexture(name, path);
		}

		internal bool LoadCustomTexture(string name, Texture2D t)
		{
			if(!Textures.ContainsKey(name))
			{
				Textures.Add(name, new QTexture(this, t));
				return true;
			}
			return false;
		}

		internal bool LoadMusic(string name, string path)
		{
			if(!Music.ContainsKey(name))
			{
				Music.Add(name, Load<Song>(path));
				return true;
			}
			return false;
		}

		internal bool LoadMusic(string path)
		{
			string name = path.Split('/').Last();
			return LoadMusic(name, path);
		}

		internal bool LoadFont(string name, string path)
		{
			if(!Fonts.ContainsKey(name))
			{
				Fonts.Add(name, Load<SpriteFont>(path));
				return true;
			}
			return false;
		}

		internal bool LoadFont(string path)
		{
			string name = path.Split('/').Last();
			return LoadFont(name, path);
		}

		internal bool LoadSound(string name, string path)
		{
			if(!Sounds.ContainsKey(name))
			{
				Sounds.Add(name, Load<SoundEffect>(path));
				return true;
			}
			return false;
		}

		internal bool LoadSound(string path)
		{
			string name = path.Split('/').Last();
			return LoadSound(name, path);
		}

		internal QTexture GetTexture(string name)
		{
			if(!Textures.TryGetValue(name, out QTexture t))
				throw new Exception($"Cannot find texture {name}");
			return t;
		}

		internal QFont GetFont(string name)
		{
			if(!Fonts.TryGetValue(name, out QFont f))
				throw new Exception($"Cannot find font {name}");
			return f;
		}

		internal QMusic GetMusic(string name)
		{
			if(!Music.TryGetValue(name, out QMusic m))
				throw new Exception($"Cannot find Music {name}");
			return m;
		}

		internal QSound GetSound(string name)
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
			MonoContentManager.Unload();
		}

		internal void Unload()
		{
			Clear();
		}

		internal QContentManager(Game engine)
		{
			MonoGraphicsDevice = engine.GraphicsDevice;
			MonoContentManager = engine.Content;
			Atlases = new List<QTextureAtlas>();
			Textures = new Dictionary<string, QTexture>();
			Fonts = new Dictionary<string, QFont>();
			Sounds = new Dictionary<string, QSound>();
			Music = new Dictionary<string, QMusic>();
			//setting up the unknown texture once
			if (UnknownTexture == null)
			{
				var texture = new Texture2D(MonoGraphicsDevice, 2, 2);
				var c = new QColor[]
				{
					QColor.White, QColor.Purple, QColor.White, QColor.Purple
				};
				texture.SetData(c);
				UnknownTexture = new QTexture(this, texture);
			}
		}
	}
}
