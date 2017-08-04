using System.Reflection;

namespace QEngine
{
	public class QAppConfig
	{
		public Assembly Assembly = null;
		
		public int Width = 800;

		public int Height = 600;

		public bool Fullscreen = false;

		public bool Borderless = false;

		public bool Multisampling = false;

		public bool MouseVisible = true;

		public bool Vsync = false;

		public bool FixedTimeStep = false;

		public float TimeStep = 1 / 60f;

		public string Title = "QGame";

		/// <summary>
		/// Always use this as the default directory
		/// </summary>
		public readonly string AssetDirectory = "Assets";

		public QAppConfig(Assembly a = null)
		{
			Assembly = a;
		}
	}
}