using System;
using QEngine.Demos.PlatformingDemo.Scripts;

namespace QEngine.Demos
{
	public class Player : QCharacterController
	{
		QAnimator Animator;

		QSprite Sprite;

		QRect LeftIdle, RightIdle;

		QRigiBody Body;

		const float PlayerSpeed = 5;

		const float MaxPlayerVelocity = 7;

		const float MaxPlayerSprintVel = 8;

		const float JumpSpeed = 80;

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

		QDirections PlayerDirection;

		PlayerStates PlayerState;

		public override void OnLoad(QAddContent add)
		{
			add.Texture(Assets.Bryan + "BryanSpriteSheet");
			add.Texture(Assets.Bryan + "SwordAttack2");
		}

		public override void OnStart(QGetContent getContent)
		{
			Scene.SpriteRenderer.Filter = QFilteringState.Point;
			World.Gravity = new QVec(0, 20);
			var Frames = getContent.TextureSource("BryanSpriteSheet").Split(32, 32);
			var AttackFrames = getContent.TextureSource("SwordAttack2").Split(32, 32);
			Sprite = new QSprite(this, Frames[0]);
			Transform.Scale = new QVec(4);
			Transform.Rotation = 90;
			LeftIdle = Frames[2];
			RightIdle = Frames[0];

			Body = World.CreateCapsule(this, (Sprite.Height * 4) / 3f - 15, (Sprite.Width * 4) / 3f - 5, 100, Transform.Position, Transform.Rotation);
			//Body = World.CreateRectangle(this, (Sprite.Width * 4) / 3f + 20, (Sprite.Height * 4) / 1.2f - 5, 100, Transform.Position, 0);
			Body.FixedRotation = true;
			Body.Friction = 0.01f;

			Body.OnCollision += other =>
			{
//				switch(QRigiBody.Direction(Body, other))
//				{
//					case QCollisionDirection.Top:
//					{
//						if(other.AttachedScript is Platform p)
//						{
//							JumpGas = MaxJumpGas;
//						}
//						break;
//					}
//				}
			};

			Camera.Position = Transform.Position;

			Animator = new QAnimator();
			Animator.AddAnimation("Right", new QAnimation(Frames, 0.1, 4, 8));
			Animator.AddAnimation("Left", new QAnimation(Frames, 0.1, 12, 16));
			Animator.AddAnimation("RightAttack", new QAnimation(AttackFrames, 0.1, 0, 6));
			Animator.AddAnimation("LeftAttack", new QAnimation(AttackFrames, 0.1, 6, 12));
			Animator.Swap("Right");
			PlayerDirection = QDirections.Right;
			PlayerState = PlayerStates.None;
		}

		float JumpGas = MaxJumpGas;

		const float MaxJumpGas = 0.1f;

		public override void OnUpdate(QTime time)
		{
			var delta = time.Delta;
			QVec temp = QVec.Zero;
			bool sprint = false;
			if(Input.IsLeftMouseButtonHeld() && Accumulator.CheckAccum("Spawner", 0.03f))
				Instantiate(new Block(), Camera.ScreenToWorld(Input.MousePosition()));
			if(Input.IsMouseScrolledUp())
				Camera.Zoom += Camera.Zoom * 0.1f;
			if(Input.IsMouseScrolledDown())
				Camera.Zoom -= Camera.Zoom * 0.1f;
			if(Input.IsKeyPressed(QKeys.Escape))
				Scene.ResetScene();
			if(Input.IsKeyDown(QKeys.LeftShift) || Input.IsKeyDown(QKeys.RightShift))
			{
				sprint = true;
			}
			if(Input.IsKeyDown(QKeys.W) || Input.IsKeyDown(QKeys.Space))
			{
				if(JumpGas > 0)
				{
					Body.LinearVelocity += QVec.Up * JumpSpeed * delta;
					JumpGas -= delta;
				}
			}
			if(Input.IsKeyDown(QKeys.A))
			{
				temp += QVec.Left;
				Animator.Swap("Left");
			}
			if(Input.IsKeyDown(QKeys.D))
			{
				temp += QVec.Right;
				Animator.Swap("Right");
			}
			var rayDis = World.RayCastHitDistance(Transform.Position, new QVec(0, 53));
			if (rayDis > 0)
			{
				JumpGas = MaxJumpGas;
			}
			if (temp != QVec.Zero)
			{
				if(!sprint && Body.LinearVelocity.X < MaxPlayerVelocity && Body.LinearVelocity.X > -MaxPlayerVelocity)
					Body.LinearVelocity = new QVec(temp.Normalize().X * PlayerSpeed, Body.LinearVelocity.Y);
				else
					Body.LinearVelocity = new QVec(temp.Normalize().X * PlayerSpeed * 2, Body.LinearVelocity.Y);
				Animator.Play(Sprite, delta);
			}
			if(Input.IsKeyReleased(QKeys.A))
			{
				Sprite.Source = LeftIdle;
				Body.LinearVelocity = new QVec(-0.1f, Body.LinearVelocity.Y);
				//Body.LinearVelocity += QVec.Left * time.Delta * 400;
			}
			if(Input.IsKeyReleased(QKeys.D))
			{
				Sprite.Source = RightIdle;
				Body.LinearVelocity = new QVec(0.1f, Body.LinearVelocity.Y);
				//Body.LinearVelocity += QVec.Right * time.Delta * 400;
			}
			QVec middle = QVec.Middle(Transform.Position, Camera.ScreenToWorld(Input.MousePosition()));
			Camera.Lerp(middle, 9, time.Delta);
			//Camera.Position = QVec.PixelMove(Transform.Position, 16);
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(Sprite, Transform);
		}
	}
}