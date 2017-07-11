﻿using System;
using System.Collections.Generic;
using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo
{
	public class BiomeBat : QCharacterController
	{
		List<QRect> Frames;

		QAnimation BatFlap;

		QVec spawnerPosition;

		Player player;

		QRigiBody body;

		QSprite Sprite;

		public int Speed = 150; //default: 150

		public int Health = 3;

		public readonly int MaxHealth = 3;

		public bool CanTakeDamage { get; set; } = true;

		bool WillAttack;

		public void Flick(QVec flickDirection)
		{
			body?.ApplyForce(flickDirection);
		}

		public override void OnLoad(QAddContent add)
		{
			add.Texture(Assets.Bryan + "BryanStuff1");
		}

		public override void OnStart(QGetContent get)
		{
			spawnerPosition = Transform.Position;

			Health = MaxHealth;

			Frames = Scene.MegaTexture["BryanStuff1"].Split(32, 32);

			player = GetComponent<Player>("Player");

			Sprite = new QSprite(this, Frames[28]);
			Transform.Scale = new QVec(4);
			Sprite.Effect = QSpriteEffects.FlipVertically;
			Sprite.Source = Frames[30];

			BatFlap = new QAnimation(Frames, 0.1f, 28, 30);

			body = World.CreateRectangle(this, Sprite.Width / 3f, Sprite.Height / 3f, 5);
			body.IgnoreGravity = true;
			body.FixedRotation = true;
			body.LinearDamping = 10f;
		}

		public override void OnUpdate(QTime time)
		{
			var s = Speed;
			if(Health < 1)
				Scene.Destroy(this);
			if(!CanTakeDamage && Accumulator.CheckAccum("BatCanTakeDamage", 1f, time))
			{
				CanTakeDamage = true;
			}
			if(Health < MaxHealth)
			{
				s = Speed * 2;
			}
			if(Health == 1)
			{
				s = Speed * 3;
			}
			var distanceFromPlayer = QVec.Distance(player.Transform.Position, Transform.Position);
			if(player.Position.Y > Position.Y)
				WillAttack = true;
			if(!WillAttack)
				return;
			if(distanceFromPlayer < 800) // && player.Transform.Position.Y > Transform.Position.Y)
			{
				if(player.Transform.Position.X > Transform.Position.X)
					Sprite.Effect = QSpriteEffects.FlipHorizontally;
				else
					Sprite.Effect = QSpriteEffects.None;
				Position += QVec.MoveTowards(Transform.Position, player.Transform.Position) * time.Delta * s;
				//body.LinearVelocity += QVec.MoveTowards(Transform.Position, player.Position) * Speed * time.Delta;
				Sprite.Source = BatFlap.Play(time.Delta);
			}
			else
			{
				//runs away
				if(QVec.Distance(Transform.Position, spawnerPosition) > 1)
				{
					if(spawnerPosition.X > Transform.Position.X)
						Sprite.Effect = QSpriteEffects.FlipHorizontally;
					else
						Sprite.Effect = QSpriteEffects.None;
					Position += QVec.MoveTowards(Transform.Position, spawnerPosition) * time.Delta * Speed;
					//Position += QVec.MoveTowards(Transform.Position, spawnerPosition) * Speed * time.Delta;
					Sprite.Source = BatFlap.Play(time.Delta);
				}
				else if(distanceFromPlayer > 1000)
				{
					//closes eyes
					Sprite.Effect = QSpriteEffects.FlipVertically;
					Sprite.Source = Frames[30];
				}
				else if(distanceFromPlayer < 1000)
				{
					//opens eyes
					Sprite.Effect = QSpriteEffects.FlipVertically;
					Sprite.Source = Frames[28];
				}
			}
		}

		public override void OnDrawSprite(QSpriteRenderer renderer)
		{
			if(QVec.Distance(Transform.Position, Camera.Position) < Scene.Window.Width)
				renderer.Draw(Sprite, Transform);
		}

		public override void OnDestroy()
		{
			/*if(QRandom.Number(0,1)== 1)*/
			Console.WriteLine("FUCK");
			Instantiate(new DroppableItem(), Position);
		}

		public override void OnUnload() { }
	}
}

/*//if the bat sees the player, it goes on the attacks
if(distanceFromPlayer < 200)
{
	if(player.Transform.Position.X > Transform.Position.X)
		Sprite.Effect = QSpriteEffects.FlipHorizontally;
	else
		Sprite.Effect = QSpriteEffects.None;
	Transform.Position += QVec.MoveTowards(Transform.Position, player.Transform.Position) * delta * s;
	Sprite.Source = BatFlap.Play(delta);
}
//only attacks from above
else */