using System.ComponentModel;

namespace QEngine.Demos.PlatformingDemo
{
	public class PlayerAttackCollider : QBehavior, IQLoad, IQStart, IQUpdate, IQDrawSprite
	{
		QSprite Sprite;

		QRigiBody Body;

		Player p;

		public const float Radius = 1000f;

		public void OnLoad(QAddContent add)
		{
			add.Circle(Name, 20);
		}

		public void OnStart(QGetContent get)
		{
			Sprite = new QSprite(this, get.TextureSource(Name));
			Sprite.Color = QColor.Black;

			Body = World.CreateCircle(this, Radius, 1, QBodyType.Static);
			Body.IsSensor = true;
			Body.OnCollisionStay += OnCollision;

			p = GetComponent<Player>();
		}

		public void OnUpdate(QTime time)
		{
			if(p.PlayerDirection == Player.Directions.Left)
				Position = new QVec(-25, 15) + p.Position;
			else
				Position = new QVec(25, 15) + p.Position;
		}

		public void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			//spriteRenderer.Draw(Sprite, Transform);
		}

		void OnCollision(QRigiBody other)
		{
			if(other.Script is BiomeBat bat)
			{
				if(bat.CanTakeDamage && p.PlayerState == Player.PlayerStates.Attacking)
				{
					bat.CanTakeDamage = false;
					bat.Health--;
					if(p.PlayerDirection == Player.Directions.Left)
						bat.Flick(new QVec(-450, -150));
					else
						bat.Flick(new QVec(450, -150));
				}
			}
		}
	}
}