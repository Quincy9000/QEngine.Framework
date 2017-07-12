using System;
using System.Diagnostics;
using System.Reflection;
using QEngine.Demos.Physics;
using QEngine.Demos.Scenes;

namespace QEngine.Demos
{
	public class Demo
	{
		[STAThread]
		static void Main(string[] args)
		{
			new QApplication(new QAppConfig(Assembly.GetCallingAssembly())
			{
				AssetDirectory = "Assets",
				Width = 1920,
				Height = 1080,
				Fullscreen = true,
				Vsync = false,
				MouseVisible = true,
			}).Run(new Platformer());
		}
	}
}