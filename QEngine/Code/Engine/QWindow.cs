using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QWindow
	{
		readonly GameWindow _window;

		public QRect Bounds => _window.ClientBounds;

		public int Width => Bounds.Width;

		public int Height => Bounds.Height;

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

		public static implicit operator QWindow(GameWindow w) => new QWindow(w);
		public static implicit operator GameWindow(QWindow w) => w._window;
		internal QWindow(GameWindow g) => _window = g;
	}
}