using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo
{
	public class Player : QCharacterController
	{
		public enum Directions
		{
			Left,
			Right
		}

		public enum PlayerStates
		{
			Moving,
			Attacking,
		}

		const float PlayerSpeed = 5;

		const float JumpSpeed = 8.3f;

		const float MaxJumpGas = 0.15f;

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

		bool Attacking = false;

		float JumpGas = MaxJumpGas;

		QRect LeftIdle, RightIdle;

		public Directions PlayerDirection { get; private set; }

		public PlayerStates PlayerState { get; private set; }

		bool CanTakeDamage = true;

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
			add.Music(Assets.Audio + "areYouReadyForThis");
		}

		public override void OnStart(QGetContent get)
		{
			Instantiate(new HealthBar());

			Instantiate(new PlayerAttackCollider(), Position);

			Health = HealthMax;

			Scene.SpriteRenderer.Filter = QFilteringState.Point;
			World.Gravity = new QVec(0, 25);
			var frames = get.TextureSource("BryanSpriteSheet").Split(32, 32);
			var attackFrames = get.TextureSource("SwordAttack2").Split(32, 32);
			Sprite = new QSprite(this, frames[0]);
			Transform.Scale = new QVec(4);
			LeftIdle = frames[2];
			RightIdle = frames[0];
			spaceJam = get.Music("areYouReadyForThis");
			//spaceJam.Play();

			//Body = World.CreateCapsule(this, Sprite.Height / 3f + 15, Sprite.Width / 6f, 10);
			Body = World.CreateRectangle(this, Sprite.Width / 3f, Sprite.Height / 1.3f, 10);
			//Body = World.CreateRoundedRect(this, Sprite.Width /3f + 20, Sprite.Height / 1.2f, 10);

			Body.FixedRotation = true;
			Body.Friction = 0.4f;
			Body.IsCCD = true;

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
			});
			Animator.EditAnimation("RightAttack", animation =>
			{
				animation.Multiplyer = 2f;
			});
			Animator.Swap("Right");
			PlayerDirection = Directions.Right;
			PlayerState = PlayerStates.Moving;
		}

		public override void OnUpdate(QTime time)
		{
			if(!CanTakeDamage && Accumulator.CheckAccum("CanTakeDamage", 0.5f, time))
			{
				CanTakeDamage = true;
			}
			switch(PlayerState)
			{
				case PlayerStates.Moving:
					Movement(time);
					break;
				case PlayerStates.Attacking:
					Attack(time);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		void Movement(QTime time)
		{
			if(Health == 0)
				Scene.ResetScene();
			QVec temp = QVec.Zero;
			bool sprint = false;
			bool right = true;
			bool left = true;
			if(Input.IsMouseScrolledUp())
				Camera.Zoom += Camera.Zoom * 0.1f;
			if(Input.IsMouseScrolledDown())
				Camera.Zoom -= Camera.Zoom * 0.1f;
			if(!CanMove && Accumulator.CheckAccum("Damaged", 0.5f, time))
				CanMove = true;
			if(Input.IsKeyPressed(QKeys.Escape))
				Scene.ExitGame();
			if(Input.IsKeyPressed("r"))
				Scene.ResetScene();
			if(Input.IsKeyHeld(QKeys.LeftShift) || Input.IsKeyHeld(QKeys.RightShift))
				sprint = true;

			if(World.WhatDidRaycastHit(Position, new QVec(-35, 0), out List<QRigiBody> rb1)) //left ray 
			{
				foreach(var qRigiBody in rb1)
				{
					//TODO make smoother
					if(qRigiBody.Script is BiomeWall floor || qRigiBody.Script is BiomeFloor)
					{
						if(QVec.Distance(Position, qRigiBody.Position) < 35)
						{
							Position += new QVec(35, 0);
						}
						left = false;
						if(PlayerDirection == Directions.Left)
							Sprite.Source = LeftIdle;
						else
							Sprite.Source = RightIdle;
					}
				}
			}
			
			if(World.WhatDidRaycastHit(Position, new QVec(35, 0), out List<QRigiBody> rb2)) //right ray
			{
				foreach(var qRigiBody in rb2)
				{
					if(qRigiBody.Script is BiomeWall floor || qRigiBody.Script is BiomeFloor)
					{
						if(QVec.Distance(Position, qRigiBody.Position) < 35)
						{
							Position += new QVec(-35, 0);
						}
						right = false;
						if(PlayerDirection == Directions.Left)
							Sprite.Source = LeftIdle;
						else
							Sprite.Source = RightIdle;
					}
				}
			}
			
			if(Input.IsKeyPressed("j"))
			{
				if(PlayerDirection == Directions.Left)
					Animator.Swap("LeftAttack");
				else
					Animator.Swap("RightAttack");
				Animator.Current.Loop = false;
				IsJumpDone = false;
				Attacking = true;
				Coroutine.Start(AttackDelay(time));
				return;
			}
			if(Attacking) return;
			if(IsJumpDone && Input.IsKeyPressed(QKeys.W) || Input.IsKeyPressed(QKeys.Space))
			{
				IsJumpDone = false;
			}
			if(CanJump && !IsJumpDone && (Input.IsKeyHeld(QKeys.W) || Input.IsKeyHeld(QKeys.Space)))
			{
				JumpGas -= time.Delta;
				if(JumpGas > 0)
					Body.LinearVelocity = new QVec(0, -JumpSpeed);
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
					PlayerDirection = Directions.Left;
				}
				if(Input.IsKeyHeld("D") && right)
				{
					temp += QVec.Right;
					Animator.Swap("Right");
					PlayerDirection = Directions.Right;
				}
			}
			if(temp != QVec.Zero)
			{
				if(!sprint)
					Position += temp * time.Delta * 500;
				else
					Position += temp * time.Delta * 800;
				Animator.Play(Sprite, time);
			}
			if(Input.IsKeyReleased(QKeys.A))
			{
				Sprite.Source = LeftIdle;
				PlayerDirection = Directions.Left;
			}
			if(Input.IsKeyReleased(QKeys.D))
			{
				Sprite.Source = RightIdle;
				PlayerDirection = Directions.Right;
			}
		}

		void Attack(QTime time)
		{
			Animator.Play(Sprite, time);
			if(Animator.Current.IsDone)
			{
				Animator.Current.Reset();
				PlayerState = PlayerStates.Moving;
				if(PlayerDirection == Directions.Left)
					Sprite.Source = LeftIdle;
				else
					Sprite.Source = RightIdle;
				Attacking = false;
			}
		}

		IEnumerator AttackDelay(QTime time)
		{
			yield return QCoroutine.WaitForSeconds(0.1);
			PlayerState = PlayerStates.Attacking;
		}

		public override void OnLateUpdate(QTime time)
		{
			const float cameraSpeed = 6;
//			var mouse = Camera.ScreenToWorld(Input.MousePosition());
//			var middle = QVec.Middle(Transform.Position, mouse);
//			if(Camera.Bounds.Contains(mouse))
//				Camera.Lerp(middle, cameraSpeed, time.Delta);
//			else
//				Camera.Lerp(Position, cameraSpeed, time.Delta);
			if(Camera.Bounds.Contains(Position))
			{
				if(QVec.Distance(Camera.Position, Position) < 100)
					return;
				Camera.Lerp(Position, cameraSpeed, time.Delta);
			}
			else
			{
				Camera.Lerp(Position, 20, time.Delta);
			}
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(Sprite, Transform);
		}

		public void OnCollisionStay(QRigiBody other)
		{
			if(other.Data() is BiomeBat bat && CanTakeDamage)
			{
				Health--;
				CanMove = false;
				CanTakeDamage = false;
				const float force = 2000;
				if(Position.X < bat.Position.X)
				{
					Body.ApplyForce(new QVec(-force, -force));
					bat.Flick(new QVec(450, -150));
				}
				else
				{
					Body.ApplyForce(new QVec(force, -force));
					bat.Flick(new QVec(-450, -150));
				}
			}
			else if(other.Data() is BiomeFloor floor)
			{
				if(Position.Y < floor.Position.Y)
				{
					JumpGas = MaxJumpGas;
				}
			}
			else if(other.IsDynamic)
			{
				if(Position.Y < other.Position.Y)
				{
					JumpGas = MaxJumpGas;
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