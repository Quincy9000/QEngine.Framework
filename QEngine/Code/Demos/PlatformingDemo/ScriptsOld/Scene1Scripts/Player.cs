using System.Collections.Generic;
using QEngine;
using QEngine.Demos.PlatformingDemo.Scripts;
using QEngine.Interfaces;
using QEngine.Rendering;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class PlayerHealth : QBehavior, IQStart, IQDrawGui
	{
		Player p;

		public QImage Image { get; set; }

		public PlayerHealth() : base("PlayerHealth") { }

		public void OnStart(QGetContent content)
		{
			p = GetComponent<Player>("Player");

			Image = new QImage(this, "BryanStuff1");
			Image.Origin = new QVec(16);
			Transform.Scale = new QVec(4);
		}

		public void OnDrawGui(QGuiRenderer renderer)
		{
			QVec pos = new QVec(50, 50);
			Image.Source = p.HearthSource;
			for(int i = 0; i < p.Health; i++)
			{
				//DrawSource(renderer, pos);
				renderer.DrawImage(Image, Transform, pos);
				pos += new QVec(50, 0);
			}
			pos = new QVec(50 * p.HealthMax, 50);
			Image.Source = p.EmptyHealthSource;
			for(int i = p.Health; i < p.HealthMax; ++i)
			{
				//DrawSource(renderer, pos);
				renderer.DrawImage(Image, Transform, pos);
				pos -= new QVec(50, 0);
			}
		}
	}

	public class PlayerSpawner : QBehavior, IQStart
	{
		public void OnStart(QGetContent content)
		{
			Instantiate(new Player());
		}

		public PlayerSpawner() : base("PlayerSpawner") { }
	}

	public class Player : QBehavior, IQLoad, IQStart, IQUpdate, IQDrawSprite
	{
		public Player() : base("Player") { }

		QAnimation Left, Right, Current, AttackRight, AttackLeft;

		List<QRect> Frames;

		List<QRect> AttackFrames;

		PlayerSpawner Spawner;

		SwordAttackCollider SwordCollision;

		public QSprite Sprite { get; set; }

		QRigiBody Rigibody;

		public enum PlayerState
		{
			Attacking,
			Moving
		}

		public PlayerState StateOfThePlayer { get; private set; }

		public const float Speed = 300;

		public const float RunSpeed = 500;

		public int Health = 3;

		public readonly int HealthMax = 5;

		public bool CanJump => TimeInAir < AirTime;

		//left is false and right is true
		public bool CurrentDirection = true;

		public const float Velocity = 0.1f;

		public const float AirTime = 0.1f;

		public const float MaxVelocity = 9;

		public const float MaxAttackRecover = 0.2f;

		public float AttackRecover { get; private set; }

		public bool Attacking { get; private set; }

		public QRect HearthSource { get; private set; }

		public QRect EmptyHealthSource { get; private set; }

		public float TimeInAir { get; private set; } = 0;

		bool MoveLeft = true;

		bool MoveRight = true;

		bool canTakeDamage = true;

		double accumiFrames = 0;

		const double MaxiFrames = 2f;

		public void OnLoad(QAddContent content)
		{
//			Texture("BryanSpriteSheet", Assets.Bryan + "BryanSpriteSheet");
//			Texture("BryanStuff1", Assets.Bryan + "BryanStuff1");
//			Texture("SwordAttack2", Assets.Bryan + "SwordAttack2");
//			Texture("Player_Shadow", Assets.Bryan + "Player_Shadow");
			content.Texture(Assets.Bryan + "BryanSpriteSheet");
			content.Texture(Assets.Bryan + "BryanStuff1");
			content.Texture(Assets.Bryan + "SwordAttack2");
			content.Texture(Assets.Bryan + "Player_Shadow");
		}

		public void OnStart(QGetContent content)
		{
			StateOfThePlayer = PlayerState.Moving;

			//Instantiate(SwordCollision = new SwordAttackCollider());
			
			Health = HealthMax;

			Instantiate(new PlayerHealth());

			//Spawner = GetComponent<PlayerSpawner>("PlayerSpawner");

			AttackFrames = Scene.MegaTexture["SwordAttack2"].Split(32, 32);
			Frames = Scene.MegaTexture["BryanSpriteSheet"].Split(32, 32);
			HearthSource = Scene.MegaTexture["BryanStuff1"].Split(32, 32)[21];
			EmptyHealthSource = Scene.MegaTexture["BryanStuff1"].Split(32, 32)[22];

			Sprite = new QSprite(this, Frames[0]);
			Sprite.Layer = 5f;

			Transform.Scale = new QVec(4);

			const float attackTime = 0.1f;
			Left = new QAnimation(Frames, 0.1f, 12, 16);
			Right = new QAnimation(Frames, 0.1f, 4, 8);
			AttackRight = new QAnimation(AttackFrames, attackTime, 1, 5)
			{
				Loop = false
			};
			AttackLeft = new QAnimation(AttackFrames, attackTime, 7, 11)
			{
				Loop = false
			};
			Current = Right;

			//			Rigibody = QBody.Capsule(
			//				27 * Sprite.Scale.Y,
			//				(15 * Sprite.Scale.X) / 2f, 20,
			//				(15 * Sprite.Scale.X) / 2f, 20, 1f,
			//				Position, this);
			Rigibody = World.CreateCapsule(this, 27 * Transform.Scale.Y, (15 * Transform.Scale.X) / 2f, 20, Transform.Position, 0, QBodyType.Dynamic);

			Rigibody.IsIgnoreCcd = true;
			Rigibody.FixedRotation = true;
			Rigibody.Friction = 1f;

			//Rigibody.OnCollision += OnCollision;

			World.Gravity = new QVec(0, 20f);
			Camera.Position = Transform.Position;// = Spawner.Transform.Position;

			MoveLeft = true;
			MoveRight = true;
			TimeInAir = 0;
		}

		public void OnUpdate(QTime time)
		{
			if(StateOfThePlayer == PlayerState.Moving)
				Move(time);
			else if(StateOfThePlayer == PlayerState.Attacking)
				AttackingState(time);
			MoveCam(time);
			CheckVelocity();
		}

		void Jump()
		{
			MoveLeft = true;
			MoveRight = true;
			Rigibody.LinearVelocity += new QVec(0, -Velocity);
		}

		void AttackingState(QTime time)
		{
			var speed = Speed;
			Attacking = true;
			//if in air, you are able to move left and right
			if(TimeInAir > 0)
			{
				var temp = QVec.Zero;
				if(Input.IsKeyHeld(QKeys.LeftShift) || Input.IsKeyHeld(QKeys.RightShift))
				{
					speed = RunSpeed;
				}
				if(Input.IsKeyHeld(QKeys.A))
				{
					Transform.Position += new QVec(-1, 0) * speed * time.Delta;
				}
				if(Input.IsKeyHeld(QKeys.D))
				{
					Transform.Position += new QVec(1, 0) * speed * time.Delta;
				}
				if(Input.IsKeyHeld(QKeys.Space) && CanJump || Input.IsKeyHeld(QKeys.W) && CanJump)
				{
					//-y = up, y = down
					//applying force is more buggy than just setting the velocity
					if(Rigibody.LinearVelocity.Y < 2f)
					{
						MoveLeft = true;
						MoveRight = true;
						Rigibody.LinearVelocity += new QVec(0, -Velocity) * time.Delta * 1000;
					}
					TimeInAir += time.Delta;
				}
			}
			//			if(Current != AttackRight)
			//			{
			//				Current = AttackRight;
			//				Current.Reset();
			//			}
			if(Current.IsDone)
			{
				AttackRecover = 0;
				Current.Reset();
				Attacking = false;
				StateOfThePlayer = PlayerState.Moving;
			}
			Sprite.Source = Current.Play(time.Delta);
		}

		void Move(QTime time)
		{
			bool run = false;
			Attacking = false;
			var speed = Speed;
			QVec temp = QVec.Zero;
			accumiFrames += time.Delta;
			AttackRecover += time.Delta;
			if(accumiFrames > MaxiFrames)
			{
				canTakeDamage = true;
				accumiFrames = 0;
			}

			if(Health < 1)
			{
				Scene.ResetScene();
			}

			if(Input.IsKeyDown(QKeys.Escape))
			{
				ExitGame();
			}

			if(Input.IsKeyHeld(QKeys.LeftShift) || Input.IsKeyHeld(QKeys.RightShift))
			{
				run = true;
				speed = RunSpeed;
			}

			if(Input.IsKeyHeld(QKeys.D) && MoveRight)
			{
				Current = Right;
				temp += new QVec(1, 0);
				MoveLeft = true;
				CurrentDirection = true;
			}

			if(Input.IsKeyHeld(QKeys.A) && MoveLeft)
			{
				Current = Left;
				temp += new QVec(-1, 0);
				MoveRight = true;
				CurrentDirection = false;
			}

			if(Input.IsKeyUp(QKeys.D))
			{
				Sprite.Source = Frames[0];
			}

			if(Input.IsKeyUp(QKeys.A))
			{
				Sprite.Source = Frames[2];
			}

			if(Input.IsKeyDown(QKeys.R))
			{
				Camera.Zoom = 100;
				//ChangeScene(SceneName);
			}

			if(Input.IsKeyHeld(QKeys.Space) && CanJump || Input.IsKeyHeld(QKeys.W) && CanJump)
			{
				//-y = up, y = down
				//applying force is more buggy than just setting the velocity
				if(Rigibody.LinearVelocity.Y < 2f)
				{
					MoveLeft = true;
					MoveRight = true;
					Rigibody.LinearVelocity += new QVec(0, -Velocity) * time.Delta * 1000;
				}
				TimeInAir += time.Delta;
			}

			if(Input.IsKeyUp(QKeys.Space))
			{
				TimeInAir = AirTime;
			}

			if(Input.IsMouseScrolledUp())
			{
				Camera.Zoom += Camera.Zoom * 0.1f;
				Console.WriteLine($"Zoom: {Camera.Zoom}");
			}

			if(Input.IsMouseScrolledDown())
			{
				Camera.Zoom -= Camera.Zoom * 0.1f;
				Console.WriteLine($"Zoom: {Camera.Zoom}");
			}

			if(temp != QVec.Zero)
			{
//				Sprite.Effect = SpriteEffects.None;
				Transform.Position += temp.Normalize() * speed * time.Delta;
				Current.Multiplyer = run ? 2 : 1;
				Sprite.Source = Current.Play(time.Delta);
			}

			if(Input.IsKeyHeld(QKeys.J) && AttackRecover > MaxAttackRecover)
			{
				StateOfThePlayer = PlayerState.Attacking;
				if(CurrentDirection)
					Current = AttackRight;
				else
					Current = AttackLeft;
				Current.Reset();
			}
		}

		void MoveCam(QTime time)
		{
			if(QVec.Distance(Transform.Position, Camera.Position) > 10)
			{
				Rigibody.Awake = true;
				Camera.Lerp(Transform.Position, 4, time.Delta);
				//MainCamera.MoveTo(Position, 500 * time.Delta);
			}
		}

		void CheckVelocity()
		{
			Rigibody.LinearVelocity = QVec.Clamp(
				Rigibody.LinearVelocity,
				new QVec(-MaxVelocity, -MaxVelocity),
				new QVec(MaxVelocity, MaxVelocity * 2f)
			);
		}

//		public bool OnCollision(Fixture a, Fixture b, Contact c)
//		{
//			switch(b.Body.UserData)
//			{
//				case Bat bat:
//				{
//					//					switch(c.Direction())
//					//					{
//					//						case QTools.CollisionDirection.Top:
//					//						{
//					//							if(bat.CanTakeDamage)
//					//							{
//					//								var f = 500;
//					//								bat.Health--;
//					//								bat.Rigibody.ApplyForce(new Vector2(0, 500));
//					//								bat.CanTakeDamage = false;
//					//								TimeInAir = 0;
//					//								canTakeDamage = false;
//					//							}
//					//							return true;
//					//						}
//					//					}
//					if(canTakeDamage)
//					{
//						var force = 300;
//						var batForce = 200;
//						Health--;
//						canTakeDamage = false;
//						if(bat.Position.X > Position.X)
//						{
//							Rigibody.ApplyForce(new Vector2(-force, -force));
//							bat.Rigibody.ApplyForce(new Vector2(batForce, 0));
//						}
//						else if(bat.Position.X < Position.X)
//						{
//							Rigibody.ApplyForce(new Vector2(force, -force));
//							bat.Rigibody.ApplyForce(new Vector2(-batForce, 0));
//						}
//						TimeInAir = AirTime;
//						MoveLeft = false;
//						MoveRight = false;
//					}
//					return true;
//				}
//				case CavernFloorTile floor:
//				{
//					MoveLeft = true;
//					MoveRight = true;
//					var push = 5;
//					switch(c.Direction())
//					{
//						case QTools.CollisionDirection.Top:
//						{
//							TimeInAir = 0;
//							return true;
//						}
//						case QTools.CollisionDirection.Left:
//						{
//							MoveLeft = false;
//							Position += new Vector2(push, 0);
//							return true;
//						}
//						case QTools.CollisionDirection.Right:
//						{
//							MoveRight = false;
//							Position += new Vector2(-push, 0);
//							return true;
//						}
//					}
//					return true;
//				}
//				case Spikes spikes:
//				{
//					MainCamera.Position = Position = Spawner.Position + new Vector2(0, -50);
//					MoveLeft = true;
//					MoveRight = true;
//					TimeInAir = 0;
//					if(canTakeDamage)
//					{
//						Health--;
//						canTakeDamage = false;
//					}
//					return true;
//				}
//				case CavernWalls w:
//				{
//					var push = 6;
//					switch(c.Direction())
//					{
//						case QTools.CollisionDirection.Right:
//						{
//							MoveLeft = false;
//							MoveRight = true;
//							Position += new Vector2(push, 0);
//							return true;
//						}
//						case QTools.CollisionDirection.Left:
//						{
//							MoveLeft = true;
//							MoveRight = false;
//							Position += new Vector2(-push, 0);
//							return true;
//						}
//						case QTools.CollisionDirection.Bottom:
//						{
//							TimeInAir = AirTime;
//							MoveLeft = true;
//							MoveRight = true;
//							return true;
//						}
//					}
//					return true;
//				}
//			}
//			return true;
//		}

		public void OnDrawSprite(QSpriteRenderer renderer)
		{
			renderer.Draw(Sprite, Transform);
		}
	}
}