using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Win32.SafeHandles;
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

		const float PlayerSpeed = 5;

		const float JumpSpeed = 5f;

		const float MaxJumpTime = 0.15f;

		float JumpTime { get; set; } = MaxJumpTime;

		int _health;

		bool CanMoveLeft = true;

		bool CanMoveRight = true;

		bool CanMove => CanMoveLeft && CanMoveRight;

		public int MaxHealth = 5;

		public PlayerDirections DirectionState { get; private set; }

		public PlayerMovementStates MovementState { get; private set; }

		public PlayerCombatStates CombatState { get; private set; }

		public PlayerJumpingStates JumpingState { get; private set; }

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
		}

		public override void OnStart(QGetContent get)
		{
			Instantiate(new HealthBar());

			Instantiate(new PlayerAttackCollider(), Position);

			Health = MaxHealth;

			Scene.SpriteRenderer.Filter = QFilteringState.Point;
			World.Gravity = new QVec(0, 20);
			var frames = get.TextureSource("BryanSpriteSheet").Split(32, 32);
			var attackFrames = get.TextureSource("SwordAttack2").Split(32, 32);
			Sprite = new QSprite(this, frames[0]);
			Transform.Scale = new QVec(4);
			LeftIdle = frames[2];
			RightIdle = frames[0];

			//Body = World.CreateCapsule(this, Sprite.Height / 3f + 15, Sprite.Width / 6f, 10);
			Body = World.CreateRectangle(this, Sprite.Width / 3f, Sprite.Height / 1.3f, 10);
			//Body = World.CreateRoundedRect(this, Sprite.Width /3f + 20, Sprite.Height / 1.2f, 10);

			Body.FixedRotation = true;
			Body.Friction = 0.4f;

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

			if(Input.IsMouseScrolledUp())
				Camera.Zoom += Camera.Zoom * 0.1f;

			if(Input.IsMouseScrolledDown())
				Camera.Zoom -= Camera.Zoom * 0.1f;

			if(Input.IsKeyPressed(QKeys.Escape))
				Scene.ExitGame();

			if(Input.IsKeyPressed("r"))
				Scene.ResetScene();

			Move(time);
		}

		public override void OnFixedUpdate(float delta)
		{
			if(World.WhatDidRaycastHit(Body.Position, new QVec(-35, 0), out LinkedList<QRigiBody> rb1)) //left ray 
			{
				foreach(var qRigiBody in rb1)
				{
					if(qRigiBody.Script is BiomeWall || qRigiBody.Script is BiomeFloor)
					{
						if(DirectionState == PlayerDirections.Left)
							Sprite.Source = LeftIdle;
						else
							Sprite.Source = RightIdle;
						CanMoveLeft = false;
					}
				}
			}
			else
				CanMoveLeft = true;

			if(World.WhatDidRaycastHit(Body.Position, new QVec(35, 0), out LinkedList<QRigiBody> rb2)) //right ray
			{
				foreach(var qRigiBody in rb2)
				{
					if(qRigiBody.Script is BiomeWall || qRigiBody.Script is BiomeFloor)
					{
						if(DirectionState == PlayerDirections.Left)
							Sprite.Source = LeftIdle;
						else
							Sprite.Source = RightIdle;
						CanMoveRight = false;
					}
				}
			}
			else
				CanMoveRight = true;

			if(MovementState != PlayerMovementStates.Idle && CombatState == PlayerCombatStates.None)
			{
				float speed = MovementState == PlayerMovementStates.Sprinting ? 2.6f : 2;
				if(DirectionState == PlayerDirections.Left)
				{
					if(CanMoveLeft)
						Position += QVec.Left * PlayerSpeed * speed;
				}
				else
				{
					if(CanMoveRight)
						Position += QVec.Right * PlayerSpeed * speed;
				}
			}
			if(JumpingState == PlayerJumpingStates.Jumping)
			{
				//greater than 1 Y is falling
				if(JumpTime > 0 && Body.LinearVelocity.Y < 1)
				{
					Body.LinearVelocity = QVec.Up * JumpSpeed * 1.3f;
					JumpTime -= delta;
				}
				else
				{
					JumpingState = PlayerJumpingStates.NotJumping;
					MovementState = PlayerMovementStates.Idle;
				}
			}
		}

		void Move(QTime time)
		{
			//Position += QVec.Right * time.Delta * 8000;
			//taking damage means that you cant move a split second
			if(CombatState == PlayerCombatStates.TakingDamage && Accumulator.CheckAccum("TakingDamage", 0.25f, time))
			{
				CombatState = PlayerCombatStates.None;
			}

			if(CombatState != PlayerCombatStates.TakingDamage && CombatState != PlayerCombatStates.Attacking)
			{
				if(Input.IsKeyPressed("w") || Input.IsKeyPressed("space"))
				{
					JumpingState = PlayerJumpingStates.Jumping;
				}
				if(Input.IsKeyHeld("w") || Input.IsKeyHeld("space")) { }
				if(Input.IsKeyReleased("w") || Input.IsKeyReleased("space"))
				{
					if(JumpingState == PlayerJumpingStates.Jumping)
						JumpTime = 0;
					JumpingState = PlayerJumpingStates.NotJumping;
				}

				if(Input.IsKeyPressed("j"))
				{
					CombatState = PlayerCombatStates.Attacking;
					MovementState = PlayerMovementStates.Idle;
				}

				if(Input.IsKeyHeld("A"))
				{
					Animator.Swap("Left");
					DirectionState = PlayerDirections.Left;
					MovementState = PlayerMovementStates.Moving;
				}

				if(Input.IsKeyHeld("D"))
				{
					Animator.Swap("Right");
					DirectionState = PlayerDirections.Right;
					MovementState = PlayerMovementStates.Moving;
				}

				/*
					If the player is press left or right shift and they also pressed A or D
				*/
				if((Input.IsKeyHeld(QKeys.LeftShift) || Input.IsKeyHeld(QKeys.RightShift))
				   && MovementState == PlayerMovementStates.Moving)
					MovementState = PlayerMovementStates.Sprinting;

				if(MovementState != PlayerMovementStates.Idle && CanMove)
					Animator.Play(Sprite, time);

				if(Input.IsKeyReleased(QKeys.A))
				{
					Sprite.Source = LeftIdle;
					DirectionState = PlayerDirections.Left;
					MovementState = PlayerMovementStates.Idle;
				}

				if(Input.IsKeyReleased(QKeys.D))
				{
					Sprite.Source = RightIdle;
					DirectionState = PlayerDirections.Right;
					MovementState = PlayerMovementStates.Idle;
				}
			}

			if(CombatState == PlayerCombatStates.Attacking)
			{
				if(DirectionState == PlayerDirections.Left)
					Animator.Swap("LeftAttack");
				else
					Animator.Swap("RightAttack");
				Animator.Play(Sprite, time);
				if(Animator.Current.IsDone)
				{
					Animator.Current.Reset();
					if(DirectionState == PlayerDirections.Left)
						Sprite.Source = LeftIdle;
					else
						Sprite.Source = RightIdle;
					CombatState = PlayerCombatStates.None;
					MovementState = PlayerMovementStates.Idle;
				}
			}

			Debug.AppendLine($"Velocity: {Body.LinearVelocity}");
			Debug.AppendLine($"CanMoveLeft: {CanMoveLeft}");
			Debug.AppendLine($"CanMoveRight: {CanMoveRight}");
			Debug.AppendLine($"JumpTime: {JumpTime}");
			Debug.AppendLine($"JumpState: {JumpingState}");
			Debug.AppendLine($"DirectionState: {DirectionState}");
			Debug.AppendLine($"MovementState: {MovementState}");
			Debug.AppendLine($"CombatState: {CombatState}");
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
			if(other.Data() is DroppableItem potion)
			{
				if(!potion.IsDestroyed)
				{
					Health++;
					Scene.Destroy(potion);
				}
			}
			else if(other.Data() is BiomeBat bat)
			{
				const float force = 500;
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
			else if(other.Data() is BiomeFloor floor)
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

//			if(Input.IsLeftMouseButtonHeld() && Accumulator.CheckAccumGlobal("Spawner", 0.15f))
//			{
//				var mouseCamera = Camera.ScreenToWorld(Input.MousePosition());
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

//			var mouse = Camera.ScreenToWorld(Input.MousePosition());
//			var middle = QVec.Middle(Transform.Position, mouse);
//			if(Camera.Bounds.Contains(mouse))
//				Camera.Lerp(middle, cameraSpeed, time.Delta);
//			else
//				Camera.Lerp(Position, cameraSpeed, time.Delta);

//			if(Input.IsKeyPressed("j"))
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
//			if(IsJumpDone && Input.IsKeyPressed(QKeys.W) || Input.IsKeyPressed(QKeys.Space))
//			{
//				IsJumpDone = false;
//			}
//			if(CanJump && !IsJumpDone && (Input.IsKeyHeld(QKeys.W) || Input.IsKeyHeld(QKeys.Space)))
//			{
//				JumpGas -= time.Delta;
//
//				if(JumpGas > 0)
//					Body.LinearVelocity = new QVec(0, -JumpSpeed);
//				if(JumpGas < 0)
//					IsJumpDone = true;
//			}
//			if(Input.IsKeyReleased("W") || Input.IsKeyReleased("Space"))
//			{
//				if(!IsJumpDone)
//					JumpGas = -1;
//				IsJumpDone = true;
//			}