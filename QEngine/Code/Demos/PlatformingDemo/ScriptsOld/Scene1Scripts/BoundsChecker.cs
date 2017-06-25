using Microsoft.Xna.Framework;
using QEngine;
using QEngine.Interfaces;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class BoundsChecker : QBehavior, IQStart, IQUpdate
	{
		Player p;

		Vector2 PlayerStart;
		
		public void OnStart(QGetContent content)
		{
			p = GetComponent<Player>("Player");
			PlayerStart = p.Transform.Position;
		}

		public void OnUpdate(QTime time)
		{
			if(p.Transform.Position.Y > Transform.Position.Y)
			{
				p.Transform.Position = PlayerStart;
			}
		}

		public BoundsChecker() : base("BoundsChecker") { }
	}
}