using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	class QEngine : Game
	{
		internal GraphicsDeviceManager MonoGraphicsDeviceManager { get; }

		internal SpriteBatch MonoSpriteBatch { get; private set; }

		internal QWorldManager Manager { get; }

		internal QAppConfig Configuration { get; }

		public const bool IsHighQuality = true;

		internal QEngine(QAppConfig conf = null)
		{
			Configuration = conf;
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
			MonoGraphicsDeviceManager = new GraphicsDeviceManager(this);
			Manager = new QWorldManager();
			MonoGraphicsDeviceManager.GraphicsProfile = IsHighQuality ? GraphicsProfile.HiDef : GraphicsProfile.Reach;
		}

		protected override void LoadContent()
		{
			if(Configuration != null)
			{
				var conf = Configuration;
				MonoGraphicsDeviceManager.PreferMultiSampling = conf.Multisampling;
				MonoGraphicsDeviceManager.PreferredBackBufferHeight = conf.Height;
				MonoGraphicsDeviceManager.PreferredBackBufferWidth = conf.Width;
				MonoGraphicsDeviceManager.IsFullScreen = conf.Fullscreen;
				MonoGraphicsDeviceManager.SynchronizeWithVerticalRetrace = conf.Vsync;
				IsMouseVisible = conf.MouseVisible;
				Window.Title = conf.Title;
				Window.IsBorderless = conf.Borderless;
				Content.RootDirectory = conf.AssetDirectory;
				IsFixedTimeStep = conf.FixedTimeStep;
				TargetElapsedTime = TimeSpan.FromSeconds(conf.TimeStep);
				MonoGraphicsDeviceManager.ApplyChanges();
			}
			MonoSpriteBatch = new SpriteBatch(GraphicsDevice);
			Manager.Init();
		}

		protected override void Update(GameTime gameTime)
		{
			QTime.Update(gameTime);
			QInput.Update();
			Manager.Update();
		}

		protected override void Draw(GameTime gameTime)
		{
			Manager.Draw();
			Manager.Gui();
		}

		protected override void UnloadContent()
		{
			Manager.Unload();
		}
	}
}