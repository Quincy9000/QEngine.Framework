namespace QEngine.Demos.PlatformingDemo
{
	public class PlayerAttackCollider : QBehavior, IQLoad, IQStart, IQUpdate, IQDrawSprite
	{
		QSprite Sprite;

		QRigidBody Body;

		Player p;

		public const float Radius = 45;

		public void OnLoad(QAddContent add)
		{
			add.Circle(Name, 20);
		}

		public void OnStart(QGetContent get)
		{
			Sprite = new QSprite(this, get.TextureSource(Name));
			Sprite.Color = QColor.Black;

			Body = Physics.CreateCircle(this, Radius, 1, QBodyType.Static);
			Body.IsSensor = true;
			Body.OnCollisionStay += OnCollisionStay;

			p = GetBehavior<Player>();
		}

		public void OnUpdate(QTime time)
		{
			if(p.DirectionState == PlayerDirections.Left)
				Position = new QVec(-20, 0) + p.Position;
			else
				Position = new QVec(20, 0) + p.Position;
		}

		public void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			//spriteRenderer.Draw(Sprite, Transform);
		}

		void OnCollisionStay(QRigidBody other)
		{
			if(other.Script is BiomeBat bat)
			{
				if(bat.CanTakeDamage && p.CombatState == PlayerCombatStates.Attacking)
				{
					bat.CanTakeDamage = false;
					bat.Health--;
					if(p.Position.X > bat.Position.X)
						bat.Flick(new QVec(-2000, -150));
					else
						bat.Flick(new QVec(2000, -150));
				}
			}
		}
	}
}