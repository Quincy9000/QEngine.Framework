using System;

namespace QEngine.Demos
{
	public class Demo
	{
		[STAThread]
		static void Main(string[] args)
		{
			System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
			new QApplication(new QAppConfig()
			{
				AssetDirectory = "Assets",
				Width = 1280,
				Height = 720,
				Fullscreen = false,
				Vsync = false,
				MouseVisible = true,
			}).Run(new Platformer());
		}
	}
}