using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	class QEngine : Game
	{
		internal GraphicsDeviceManager DeviceManager { get; }

		internal QSceneManager Manager { get; }

		internal SpriteBatch SpriteBatch;
		
		internal QAppConfig Configuration { get; }

		internal QEngine(QAppConfig conf = null)
		{
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
			DeviceManager = new GraphicsDeviceManager(this);
			Manager = new QSceneManager();
			if(conf != null)
			{
				Configuration = conf;
				DeviceManager.PreferMultiSampling = conf.Multisampling;
				DeviceManager.PreferredBackBufferHeight = conf.Height;
				DeviceManager.PreferredBackBufferWidth = conf.Width;
				DeviceManager.IsFullScreen = conf.Fullscreen;
				DeviceManager.SynchronizeWithVerticalRetrace = conf.Vsync;
				IsMouseVisible = conf.MouseVisible;
				Window.Title = conf.Title;
				Content.RootDirectory = conf.AssetDirectory;
				IsFixedTimeStep = conf.FixedTimeStep;
				TargetElapsedTime = TimeSpan.FromSeconds(conf.TimeStep);
				DeviceManager.ApplyChanges();
			}
		}

		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			Content.Unload();
			Manager.Init();
		}

		protected override void Update(GameTime gameTime)
		{
			Manager.Update(gameTime);
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