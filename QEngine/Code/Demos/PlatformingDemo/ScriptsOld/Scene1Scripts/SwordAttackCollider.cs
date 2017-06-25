using QEngine;
using QEngine.Interfaces;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class SwordAttackCollider : QBehavior, IQLoad, IQStart, IQUpdate, IQDrawSprite
	{
		public SwordAttackCollider() : base("SwordAttackCollider") { }

		public QSprite Sprite { get; set; }

		QRigiBody body;

		Player p;

		int Radius = 40;

		int distanceFromPlayer = 30;

		public void OnLoad(QAddContent content)
		{
			//CreateCircleTexture("circleAttackCollider", Radius, Color.Black);
			content.Circle("circleAttackCollider", Radius, QColor.Black);
		}

		public void OnStart(QGetContent content)
		{
			Sprite = new QSprite(this, Scene.MegaTexture["circleAttackCollider"]);
			Sprite.Origin = Sprite.Source.Center;
			Sprite.Color = new QColor(255, 255, 255, 200);

			p = GetComponent<Player>("Player");

			body = World.CreateCircle(this, Radius, 1, Transform.Position, QBodyType.Static);//QBody.Circle(Radius, 1f, userData: this);
			//body.OnCollision += OnCollision;
			body.AllowSleep = false;
			//body.IgnoreCollisionWith(p.body); //dont seem to have this anymore yet

			Transform.Position = p.Transform.Position + new QVec(distanceFromPlayer, 0);
		}

		public void OnUpdate(QTime time)
		{
			if(p.CurrentDirection)
			{
				Transform.Position = p.Transform.Position + new QVec(distanceFromPlayer, 0);
			}
			else
			{
				Transform.Position = p.Transform.Position + new QVec(-distanceFromPlayer, 0);
			}
		}

//		bool OnCollision(Fixture a, Fixture b, Contact c)
//		{
//			switch(b.Body.UserData)
//			{
//				case Bat bat:
//				{
//					if(p.Attacking && bat.CanTakeDamage)
//					{
//						var force = 500;
//						bat.Health--;
//						bat.CanTakeDamage = false;
//						if(Position.X < bat.Position.X)
//							bat.Rigibody.ApplyForce(new Vector2(force, -force));
//						if(Position.X > bat.Position.X)
//							bat.Rigibody.ApplyForce(new Vector2(-force, -force));
//						return true;
//					}
//					return false;
//				}
//			}
//			return false;
//		}

		public void OnDrawSprite(QSpriteRenderer renderer)
		{
			renderer.Draw(Sprite, Transform);
		}
	}
}