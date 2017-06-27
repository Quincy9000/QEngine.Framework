namespace QEngine.Demos.Physics
{
	class PhysicsScene : QScene
	{
		protected override void Load()
		{
			Instantiate(new BlockCreator());
			Instantiate(new Floor(new QVec(0, Window.Height / 2f - 20)));
			Instantiate(new Floor(new QVec(0, -Window.Height / 2f + 20)));
			Instantiate(new Wall(new QVec(-Window.Width / 2f + 20, 0)));
			Instantiate(new Wall(new QVec(Window.Width / 2f - 20, 0)));
		}

		public PhysicsScene() : base("PhysicsScene") { }
	}
}