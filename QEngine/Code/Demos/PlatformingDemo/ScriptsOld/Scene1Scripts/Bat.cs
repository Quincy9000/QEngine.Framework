using System;
using System.Collections.Generic;
using QEngine;
using QEngine.Interfaces;
using QEngine.Prefabs;
using QEngine.Demos.PlatformingDemo.Scripts;

namespace PlatformingTest.Scripts.Scene1Scripts
{
	public class BatSpawner : QBehavior, IQStart
	{
		public void OnStart(QGetContent content)
		{
			Instantiate(new Bat(Id));
		}

		public BatSpawner() : base("BatSpawner") { }
	}

	public class Bat : QCharacterController
	{
		List<QRect> Frames;

		QAnimation BatFlap;

		BatSpawner bs;

		Guid spawnerId;

		QVec spawnerPosition;

		Player p;

		QRigiBody body;

		public int Speed = 150;

		public int Health = 3;

		public readonly int MaxHealth = 3;

		public bool CanTakeDamage { get; set; } = true;

		double damageAccum;

		public Bat(Guid spawnerId) : base("Bat")
		{
			this.spawnerId = spawnerId;

		}

		public override void OnLoad(QAddContent content)
		{
			content.Texture(Assets.Bryan + "BryanStuff1");
		}

		public override void OnStart(QGetContent content)
		{
			Health = MaxHealth;

			Frames = Scene.MegaTexture["BryanStuff1"].Split(32, 32);

			bs = GetComponent<BatSpawner>(spawnerId);
			p = GetComponent<Player>("Player");

			Sprite = new QSprite(this, Frames[28]);
			Transform.Scale = new QVec(4);
			Sprite.Origin = new QVec(16);

			BatFlap = new QAnimation(Frames, 0.1f, 28, 30);

			spawnerPosition = Transform.Position = bs.Transform.Position;

			body = World.CreateRectangle(this, 13 * Transform.Scale.X, 13 * Transform.Scale.Y, 1f, Transform.Position, 0);
			body.IgnoreGravity = true;
			body.FixedRotation = true;
			body.LinearDamping = 10f;
		}

		public override void OnUpdate(QTime time)
		{
			var s = Speed;
			if(Health < 1)
				Scene.Destroy(this);
			damageAccum += time.Delta;
			if(damageAccum > 0.5f)
			{
				CanTakeDamage = true;
				damageAccum = 0;
			}
			if(Health < MaxHealth)
			{
				s = Speed * 2;
			}
			if(Health == 1)
			{
				s = Speed * 3;
			}
			if(QVec.Distance(p.Transform.Position, Transform.Position) < 200)
			{
				//				if(p.Transform.Position.X > Transform.Position.X)
				//					Sprite.Effect = SpriteEffects.FlipHorizontally;
				//				else
				//					Sprite.Effect = SpriteEffects.Non
				Transform.Position = QVec.MoveTowards(Transform.Position, p.Transform.Position) * time.Delta * s;
				Sprite.Source = BatFlap.Play(time.Delta);
			}
			else if(QVec.Distance(p.Transform.Position, Transform.Position) < 800 && p.Transform.Position.Y > Transform.Position.Y)
			{
				//				if(p.Transform.Position.X > Transform.Position.X)
				//					Sprite.Effect = SpriteEffects.FlipHorizontally;
				//				else
				//					Sprite.Effect = SpriteEffects.None;
				Transform.Position += QVec.MoveTowards(Transform.Position, p.Transform.Position) * time.Delta * s;
				Sprite.Source = BatFlap.Play(time.Delta);
			}
			else
			{
				if(QVec.Distance(Transform.Position, spawnerPosition) > 1)
				{
					//					if(spawnerPosition.X > Transform.Position.X)
					//						Sprite.Effect = SpriteEffects.FlipHorizontally;
					//					else
					//						Sprite.Effect = SpriteEffects.None;
					Transform.Position += QVec.MoveTowards(Transform.Position, spawnerPosition) * time.Delta * Speed;
					Sprite.Source = BatFlap.Play(time.Delta);
				}
				else
				{
					//					Sprite.Effect = SpriteEffects.FlipVertically;
					Sprite.Source = Frames[30];
				}
			}
		}

		public override void OnDrawSprite(QSpriteRenderer renderer)
		{
			if(QVec.Distance(Transform.Position, Camera.Position) < Scene.Window.Width)
				renderer.Draw(Sprite, Transform);
		}

		public override void OnUnload() { }
	}
}