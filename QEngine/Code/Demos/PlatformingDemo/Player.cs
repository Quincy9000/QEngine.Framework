using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata;
using System.Xml.Serialization;
using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo
{
	public enum PlayerDirections
	{
		Left,
		Right,
	}

	public enum PlayerMovementStates
	{
		Moving,
		Sprinting,
		Idle,
	}

	public enum PlayerJumpingStates
	{
		Jumping,
		NotJumping,
	}

	public enum PlayerTouchingGroundStates
	{
		TouchingSomething,
		TouchingGround,
		TouchingNothing,
	}

	public enum PlayerCombatStates
	{
		None,
		Attacking,
		TakingDamage,
	}

	public class Player : QCharacterController
	{
		QAnimator Animator;

		QRigiBody Body;

		QMusic spaceJam;

		QRect LeftIdle, RightIdle;

		QSprite Sprite;

		const float PlayerSpeed = 5f;

		const float JumpSpeed = 8f;

		const float MaxJumpTime = 0.2f;

		float JumpTime { get; set; } = MaxJumpTime;

		int _health;

		bool CanMoveLeft = true;

		bool CanMoveRight = true;

		public int MaxHealth = 5;

		public PlayerDirections DirectionState { get; private set; }

		public PlayerMovementStates MovementState { get; private set; }

		public PlayerCombatStates CombatState { get; private set; }

		public PlayerJumpingStates JumpingState { get; private set; }

		public PlayerTouchingGroundStates TouchingState { get; private set; }

		public int Health
		{
			get => _health;
			set
			{
				_health = value;
				if(_health < 0)
					_health = 0;
				if(_health > MaxHealth)
					_health = MaxHealth;
			}
		}

		public override void OnLoad(QAddContent add)
		{
			add.Texture(Assets.Bryan + "BryanSpriteSheet");
			add.Texture(Assets.Bryan + "SwordAttack2");
		}

		public override void OnStart(QGetContent get)
		{
			Instantiate(new HealthBar());

			Instantiate(new PlayerAttackCollider(), Position);

			Health = MaxHealth;

			Scene.SpriteRenderer.Filter = QFilteringState.Point;
			World.Gravity = new QVec(0, 30);
			var frames = get.TextureSource("BryanSpriteSheet").Split(32, 32);
			var attackFrames = get.TextureSource("SwordAttack2").Split(32, 32);
			Sprite = new QSprite(this, frames[0]);
			Sprite.Scale = new QVec(4);
			LeftIdle = frames[2];
			RightIdle = frames[0];

			Body = World.CreateCapsule(this, Sprite.Height / 3f + 15, Sprite.Width / 6f, 10);
			//Body = World.CreateRectangle(this, Sprite.Width / 3f, Sprite.Height / 1.3f, 10);
			//Body = World.CreateRoundedRect(this, Sprite.Width /3f + 20, Sprite.Height / 1.2f, 10);

			Body.FixedRotation = true;
			Body.Friction = 0f;
			Body.Restitution = 0f;

			Body.OnCollisionStay += OnCollisionStay;

			Camera.Position = Transform.Position;

			Animator = new QAnimator();
			Animator.AddAnimation("Right", new QAnimation(frames, 0.1, 4, 8));
			Animator.AddAnimation("Left", new QAnimation(frames, 0.1, 12, 16));
			Animator.AddAnimation("RightAttack", new QAnimation(attackFrames, 0.1, 0, 6));
			Animator.AddAnimation("LeftAttack", new QAnimation(attackFrames, 0.1, 6, 12));
			Animator.EditAnimation("LeftAttack", animation =>
			{
				animation.Multiplyer = 2f;
				animation.Loop = false;
			});
			Animator.EditAnimation("RightAttack", animation =>
			{
				animation.Multiplyer = 2f;
				animation.Loop = false;
			});
			Animator.Swap("Right");
			DirectionState = PlayerDirections.Right;
			MovementState = PlayerMovementStates.Idle;
			JumpingState = PlayerJumpingStates.NotJumping;
			CombatState = PlayerCombatStates.None;
		}

		public override void OnUpdate(QTime time)
		{
			if(Health == 0)
				Scene.ResetScene();

			if(QInput.IsMouseScrolledUp())
				Camera.Zoom += Camera.Zoom * 0.1f;

			if(QInput.IsMouseScrolledDown())
				Camera.Zoom -= Camera.Zoom * 0.1f;

			if(QInput.IsKeyPressed(QKeyStates.Escape))
				Scene.ExitGame();

			if(QInput.IsKeyPressed("r"))
				Scene.ResetScene();

			Move(time);
		}

		public override void OnFixedUpdate(float delta)
		{
			var b = Body;

			if(MovementState == PlayerMovementStates.Idle)
				Body.LinearVelocity = new QVec(0, b.LinearVelocity.Y);
			else
			{
				float speed = MovementState == PlayerMovementStates.Sprinting ? PlayerSpeed * 2.1f : PlayerSpeed;
				if(DirectionState == PlayerDirections.Left)
				{
					if(CanMoveLeft)
						b.LinearVelocity = new QVec(-speed, b.LinearVelocity.Y);
				}
				else
				{
					if(CanMoveRight)
						b.LinearVelocity = new QVec(speed, b.LinearVelocity.Y);
				}
			}

			if(JumpingState == PlayerJumpingStates.Jumping)
			{
				if(JumpTime > 0f && b.LinearVelocity.Y < 1)
				{
					if(b.LinearVelocity.Y > -JumpSpeed)
						b.LinearVelocity += new QVec(0, -JumpSpeed * 0.26f);
					JumpTime -= delta;
				}
				else
				{
					JumpingState = PlayerJumpingStates.NotJumping;
				}
			}
		}

		void Move(QTime time)
		{
			if(QInput.Held("a") && CanMoveLeft)
			{
				Animator.Swap("Left");
				DirectionState = PlayerDirections.Left;
				MovementState = PlayerMovementStates.Moving;
			}

			if(QInput.Held("d") && CanMoveRight)
			{
				Animator.Swap("Right");
				DirectionState = PlayerDirections.Right;
				MovementState = PlayerMovementStates.Moving;
			}

			if((QInput.Held("leftShift") ||
			    QInput.Held("rightshift")) &&
			   MovementState == PlayerMovementStates.Moving)
			{
				MovementState = PlayerMovementStates.Sprinting;
			}

			if(QInput.Released("a"))
			{
				Sprite.Source = LeftIdle;
				MovementState = PlayerMovementStates.Idle;
				DirectionState = PlayerDirections.Left;
			}

			if(QInput.Released("d"))
			{
				Sprite.Source = RightIdle;
				MovementState = PlayerMovementStates.Idle;
				DirectionState = PlayerDirections.Right;
			}

			if(QInput.Pressed("space") || QInput.Pressed("w"))
			{
				JumpingState = PlayerJumpingStates.Jumping;
			}

			if(QInput.Released("space") || QInput.Released("w"))
			{
				if(JumpingState == PlayerJumpingStates.Jumping)
					JumpTime = -1;
				JumpingState = PlayerJumpingStates.NotJumping;
			}

			if(QInput.Pressed("j"))
			{
				CombatState = PlayerCombatStates.Attacking;
				MovementState = PlayerMovementStates.Idle;
			}

			if(CombatState == PlayerCombatStates.Attacking)
			{
				Animator.Swap(DirectionState == PlayerDirections.Left ? "LeftAttack" : "RightAttack");
				Animator.Play(Sprite, time);
				if(Animator.Current.IsDone)
				{
					CombatState = PlayerCombatStates.None;
					Animator.Current.Reset();
					Sprite.Source = DirectionState == PlayerDirections.Left ? LeftIdle : RightIdle;
				}
			}
			else if(MovementState != PlayerMovementStates.Idle)
			{
				Animator.Play(Sprite, time);
			}

			Debug.AppendLine($"CanMoveLeft: {CanMoveLeft}");
			Debug.AppendLine($"CanMoveRight: {CanMoveRight}");
			Debug.AppendLine($"Velocity: {Body.LinearVelocity}");
			Debug.AppendLine($"Touching: {TouchingState}");
			Debug.AppendLine($"CanMoveLeft: {CanMoveLeft}");
			Debug.AppendLine($"CanMoveRight: {CanMoveRight}");
			Debug.AppendLine($"JumpTime: {JumpTime}");
			Debug.AppendLine($"JumpState: {JumpingState}");
			Debug.AppendLine($"DirectionState: {DirectionState}");
			Debug.AppendLine($"MovementState: {MovementState}");
			Debug.AppendLine($"CombatState: {CombatState}");
			Debug.AppendLine($"Position: {Position}");
		}

		IEnumerator AttackDelay(QTime time)
		{
			yield return QCoroutine.WaitForSeconds(0.1);
			CombatState = PlayerCombatStates.Attacking;
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(Sprite, Transform);
		}

		public void OnCollisionStay(QRigiBody other)
		{
			TouchingState = PlayerTouchingGroundStates.TouchingGround;
			if(other.Data() is DroppableItem potion)
			{
				if(Health != MaxHealth)
				{
					Health++;
					Scene.Destroy(potion);
				}
			}
			else if(other.Data() is BiomeBat bat)
			{
				const float force = 1000;
				if(CombatState != PlayerCombatStates.TakingDamage)
				{
					Health--;
					CombatState = PlayerCombatStates.TakingDamage;
					if(Position.X < bat.Position.X)
					{
						Body.ApplyForce(new QVec(-force, -force));
						bat.Flick(new QVec(force, -force));
					}
					else
					{
						Body.ApplyForce(new QVec(force, -force));
						bat.Flick(new QVec(-force, -force));
					}
				}
			}
			else if(other.Data() is BiomeFloor floor && JumpingState == PlayerJumpingStates.NotJumping)
			{
				if(Position.Y < floor.Position.Y)
				{
					JumpTime = MaxJumpTime;
				}
			}
			else if(other.IsDynamic)
			{
				if(Position.Y < other.Position.Y)
				{
					JumpTime = MaxJumpTime;
				}
			}
		}
	}
}

//			if(QInput.IsLeftMouseButtonHeld() && Accumulator.CheckAccumGlobal("Spawner", 0.15f))
//			{
//				var mouseCamera = Camera.ScreenToWorld(QInput.MousePosition());
//				var direction = QVec.MoveTowards(Position, mouseCamera);
//				QVec t;
//				if(mouseCamera.X > Position.X)
//					t = new QVec(50, 0);
//				else
//					t = new QVec(-50, 0);
//				var force = 200f;
//				if(QRandom.Number(0, 1) == 0)
//					Instantiate(new Block(40, 40, direction * force), Position + t);
//				else
//					Instantiate(new Ball(40, direction * force), Position + t);
//			}

//			var mouse = Camera.ScreenToWorld(QInput.MousePosition());
//			var middle = QVec.Middle(Transform.Position, mouse);
//			if(Camera.Bounds.Contains(mouse))
//				Camera.Lerp(middle, cameraSpeed, time.Delta);
//			else
//				Camera.Lerp(Position, cameraSpeed, time.Delta);

//			if(QInput.IsKeyPressed("j"))
//			{
//				if(PlayerPlayerDirection == PlayerDirections.Left)
//					Animator.Swap("LeftAttack");
//				else
//					Animator.Swap("RightAttack");
//				Animator.Current.Loop = false; 
//				IsJumpDone = false;
//				Attacking = true;
//				Coroutine.Start(AttackDelay(time));
//				return;
//			}
//			if(Attacking) return;
//			if(IsJumpDone && QInput.IsKeyPressed(QKeys.W) || QInput.IsKeyPressed(QKeys.Space))
//			{
//				IsJumpDone = false;
//			}
//			if(CanJump && !IsJumpDone && (QInput.IsKeyHeld(QKeys.W) || QInput.IsKeyHeld(QKeys.Space)))
//			{
//				JumpGas -= time.Delta;
//
//				if(JumpGas > 0)
//					Body.LinearVelocity = new QVec(0, -JumpSpeed);
//				if(JumpGas < 0)
//					IsJumpDone = true;
//			}
//			if(QInput.IsKeyReleased("W") || QInput.IsKeyReleased("Space"))
//			{
//				if(!IsJumpDone)
//					JumpGas = -1;
//				IsJumpDone = true;
//			}