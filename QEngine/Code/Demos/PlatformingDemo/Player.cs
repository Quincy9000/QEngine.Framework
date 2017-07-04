using Microsoft.Xna.Framework.Input;
using QEngine.Demos.Physics;
using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo
{
	public class Player : QCharacterController
	{
		enum QDirections
		{
			Left,
			Right
		}

		enum PlayerStates
		{
			Jumping,
			Attacking,
			None
		}

		const float PlayerSpeed = 5;

		const float JumpSpeed = 7;

		const float MaxJumpGas = 0.3f;

		const float MaxVel = 5;

		//distance before you cant walk into wall anymore
		const float WalkingIntoWallsDistance = 42;

		int _health;

		QAnimator Animator;

		QRigiBody Body;

		QMusic spaceJam;

		public int HealthMax = 5;

		bool CanJump => JumpGas > 0 && Body.LinearVelocity.Y < 3;

		bool CanMove; //getting disabled by enemies

		bool IsJumpDone = false;

		float JumpGas = MaxJumpGas;

		QRect LeftIdle, RightIdle;

		QDirections PlayerDirection;

		PlayerStates PlayerState;

		QSprite Sprite;

		public int Health
		{
			get => _health;
			set
			{
				_health = value;
				if(_health < 0)
					_health = 0;
			}
		}

		public override void OnLoad(QAddContent add)
		{
			add.Texture(Assets.Bryan + "BryanSpriteSheet");
			add.Texture(Assets.Bryan + "SwordAttack2");
			add.Music("Audio/areYouReadyForThis");
		}

		public override void OnStart(QGetContent get)
		{
			Instantiate(new HealthBar());

			Health = HealthMax;

			Scene.SpriteRenderer.Filter = QFilteringState.Point;
			World.Gravity = new QVec(0, 30);
			var frames = get.TextureSource("BryanSpriteSheet").Split(32, 32);
			var attackFrames = get.TextureSource("SwordAttack2").Split(32, 32);
			Sprite = new QSprite(this, frames[0]);
			Transform.Scale = new QVec(4);
			LeftIdle = frames[2];
			RightIdle = frames[0];
			spaceJam = get.Music("areYouReadyForThis");
			//spaceJam.Play();

			Body = World.CreateCapsule(this, Sprite.Height / 3f + 15, Sprite.Width / 6f, 10);
			//Body = World.CreateRectangle(this, Sprite.Width / 3f, Sprite.Height / 3f, 10);
			//Body = World.CreateRoundedRect(this, Sprite.Width /3f + 20, Sprite.Height / 1.2f, 10);

			Body.FixedRotation = true;
			Body.Friction = 0.4f;
			Body.IsCCD = true;
			//Body.Restitution = 1f;

			Body.OnCollision += Collision;

			Camera.Position = Transform.Position;

			Animator = new QAnimator();
			Animator.AddAnimation("Right", new QAnimation(frames, 0.1, 4, 8));
			Animator.AddAnimation("Left", new QAnimation(frames, 0.1, 12, 16));
			Animator.AddAnimation("RightAttack", new QAnimation(attackFrames, 0.1, 0, 6));
			Animator.AddAnimation("LeftAttack", new QAnimation(attackFrames, 0.1, 6, 12));
			Animator.Swap("Right");
			PlayerDirection = QDirections.Right;
			PlayerState = PlayerStates.None;
		}

		public override void OnUpdate(float time)
		{
//            Body.Rotation += time * 20;
			if(Health == 0)
				Scene.ResetScene();
			var delta = time;
			var temp = QVec.Zero;
			var sprint = false;
			var right = true;
			var left = true;
//			if(World.DidRaycastHit(Transform.Position + new QVec(0, Sprite.Height / 3f - 15), new QVec(0, 25)))
//			{
//				if(IsTouchingFloor)
//					JumpGas = MaxJumpGas;
//			}
			if(Input.IsMouseScrolledUp())
				Camera.Zoom += Camera.Zoom * 0.1f;
			if(Input.IsMouseScrolledDown())
				Camera.Zoom -= Camera.Zoom * 0.1f;
			if(World.DidRaycastHit(Transform.Position + new QVec(0, Sprite.Width / 3f - 5), new QVec(WalkingIntoWallsDistance, 0), out QRigiBody b))
			{
				if(b.Script is BiomeFloor)
				{
					Transform.Position += QVec.Left * 2 * time;
					Sprite.Source = RightIdle;
					right = false;
				}
			}
			if(World.DidRaycastHit(Transform.Position + new QVec(0, Sprite.Width / 3f - 5), new QVec(-WalkingIntoWallsDistance, 0), out QRigiBody bb))
			{
				if(bb.Script is BiomeFloor)
				{
					Transform.Position += QVec.Right * 2 * time;
					Sprite.Source = LeftIdle;
					left = false;
				}
			}
			if(!CanMove && Accumulator.CheckAccum("Damaged", 0.5f))
				CanMove = true;
			if(Input.IsLeftMouseButtonHeld() && Accumulator.CheckAccum("Spawner", 0.03f))
			{
				int i = QRandom.Number(0, 1);
				if(i == 0)
					Instantiate(new Block(40, 40), Camera.ScreenToWorld(Input.MousePosition()));
				else
					Instantiate(new Ball(40), Camera.ScreenToWorld(Input.MousePosition()));
			}
			if(Input.IsKeyPressed(QKeys.Escape))
				Scene.ExitGame();
			if(Input.IsKeyPressed("r"))
				Scene.ResetScene();
			if(Input.IsKeyPressed(QKeys.C))
			{
				BiomeFloor.CheckDistance = !BiomeFloor.CheckDistance;
			}
			if(Input.IsKeyHeld(QKeys.LeftShift) || Input.IsKeyHeld(QKeys.RightShift))
				sprint = true;
			if(IsJumpDone && Input.IsKeyPressed(QKeys.W) || Input.IsKeyPressed(QKeys.Space))
			{
				IsJumpDone = false;
			}
			if(CanJump && !IsJumpDone && (Input.IsKeyHeld(QKeys.W) || Input.IsKeyHeld(QKeys.Space)))
			{
				Body.LinearVelocity = new QVec(0, -JumpSpeed);
				JumpGas -= delta;
				if(JumpGas < 0)
					IsJumpDone = true;
			}
			if(Input.IsKeyReleased("W") || Input.IsKeyReleased("Space"))
			{
				if(!IsJumpDone)
					JumpGas = -1;
				IsJumpDone = true;
			}
			if(CanMove)
			{
				if(Input.IsKeyHeld("A") && left)
				{
					temp += QVec.Left;
					Animator.Swap("Left");
				}
				if(Input.IsKeyHeld("D") && right)
				{
					temp += QVec.Right;
					Animator.Swap("Right");
				}
			}
			if(temp != QVec.Zero)
			{
//				if(!sprint)
//					Position += temp * delta * 1000;
//				else
//					Body.LinearVelocity = new QVec(temp.X * PlayerSpeed * 2, Body.LinearVelocity.Y);
				if(!sprint)
					Position += temp * delta * 500;
				else
					Position += temp * delta * 800;
				Animator.Play(Sprite, delta);
			}
			if(Input.IsKeyReleased(QKeys.A))
			{
				Sprite.Source = LeftIdle;
//				Body.LinearVelocity = new QVec(0, Body.LinearVelocity.Y);
			}
			if(Input.IsKeyReleased(QKeys.D))
			{
				Sprite.Source = RightIdle;
//				Body.LinearVelocity = new QVec(0, Body.LinearVelocity.Y);
			}
			Debug.Label.AppendLine($"Velocity: {Body.LinearVelocity}");
		}

		public override void OnLateUpdate(float delta)
		{
			const float cameraSpeed = 11;
			var mouse = Camera.ScreenToWorld(Input.MousePosition());
			var middle = QVec.Middle(Transform.Position, mouse);
			if(Camera.Bounds.Contains(mouse))
				Camera.Lerp(middle, cameraSpeed, delta);
			else
				Camera.Lerp(Position, cameraSpeed, delta);
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(Sprite, Transform);
		}

		public void Collision(QRigiBody other)
		{
			if(!Accumulator.CheckAccum("Collision", 1 / 60f)) return;
			if(other.Data() is Bat bat)
			{
				Health--;
				CanMove = false;
				const float force = 2000;
				Console.WriteLine("Collide");
				if(Position.X < bat.Position.X)
				{
					Body.ApplyForce(new QVec(-force, -force));
					other.ApplyForce(new QVec(450, -150));
				}
				else
				{
					Body.ApplyForce(new QVec(force, -force));
					other.ApplyForce(new QVec(-450, -150));
				}
			}
			else if(other.Data() is BiomeFloor floor)
			{
				if(Position.Y < floor.Position.Y)
				{
					JumpGas = MaxJumpGas;
				}
			}
			else if(other.IsDynamic && other.Mass > 1)
			{
				if(Position.Y < other.Position.Y)
				{
					JumpGas = MaxJumpGas;
				}
			}
		}
	}
}