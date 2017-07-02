using System.Collections.Generic;
using QEngine.Demos.Physics;
using QEngine.Demos.PlatformingDemo;

namespace QEngine.Demos.Scenes
{
	class PhysicsScene : QScene
	{
		protected override void BehaviorScriptLoader(List<IQLoad> scripts)
		{
			scripts.Add(new Block(40, 40));
		}

		protected override void Load()
		{
			Instantiate(new BlockCreator());
			Instantiate(new Platform(new QVec(Window.Width, 40)), new QVec(0, Window.Height / 2f - 20));
			Instantiate(new Platform(new QVec(Window.Width, 40)), new QVec(0, -Window.Height / 2f + 20));
			Instantiate(new Platform(new QVec(40, Window.Height)), new QVec(-Window.Width / 2f + 20, 0));
			Instantiate(new Platform(new QVec(40, Window.Height)), new QVec(Window.Width / 2f - 20, 0));
			Instantiate(new Player());
		}

		public PhysicsScene() : base("PhysicsScene") { }
	}
}