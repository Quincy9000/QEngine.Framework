using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QWindow
	{
		readonly GameWindow _window;

		readonly QEngine _engine;

		public QRect Bounds => _window.ClientBounds;

		public int Width => Bounds.Width;

		public int Height => Bounds.Height;

		/// <summary>
		/// Exit Game
		/// </summary>
		public void Exit() => _engine.Manager.CurrentScene.ExitGame();

		/// <summary>
		/// Change the current scene
		/// </summary>
		/// <param name="name"></param>
		public void Change(string name) => _engine.Manager.CurrentScene.ChangeScene(name);

		/// <summary>
		/// Reset the current Scene
		/// </summary>
		public void Reset() => _engine.Manager.CurrentScene.ResetScene();

		public QVec Position
		{
			get => _window.Position;
			set => _window.Position = value;
		}

		public bool AllowAltF4
		{
			get => _window.AllowAltF4;
			set => _window.AllowAltF4 = value;
		}

		public bool Borderless
		{
			get => _window.IsBorderless;
			set => _window.IsBorderless = value;
		}

		public QOrientation Orientation => (QOrientation)_window.CurrentOrientation;

		public string Title
		{
			get => _window.Title;
			set => _window.Title = value;
		}

		public bool AllowResize
		{
			get => _window.AllowUserResizing;
			set => _window.AllowUserResizing = value;
		}

		internal QWindow(QEngine e)
		{
			_window = e.Window;
			_engine = e;
		}
	}
}