using System;
using System.Diagnostics;
using QEngine.Demos.Physics;
using QEngine.Demos.PlatformingDemo;

namespace QEngine.Demos
{
	public class Demo
	{
		[STAThread]
		static void Main(string[] args)
		{
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
			new QApplication(new QAppConfig()
			{
				AssetDirectory = "Assets",
				Width = 1280,
				Height = 720,
				Fullscreen = false,
				Vsync = false,
				MouseVisible = true,
			}).Run(new PhysicsScene());
		}
	}
}