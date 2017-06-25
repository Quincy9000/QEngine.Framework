using System;
using System.Runtime.Remoting.Messaging;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;

namespace QEngine
{
	public class QRigiBody
	{
		internal Body body;

		public QBehavior AttachedScript { get; }

		internal QTransform Transform;

		internal Guid Id => AttachedScript.Id;

		internal QVec Position
		{
			get => ((QVec)body.Position).ToPixel();
			set
			{
				Awake = true;
				body.Position = value.ToSim();
			}
		}

		public delegate void Collision(QRigiBody other);

		public event Collision OnCollision;

		internal float Rotation
		{
			get => body.Rotation;
			set => body.Rotation = value;
		}

		public QVec LinearVelocity
		{
			get => body.LinearVelocity;
			set => body.LinearVelocity = value;
		}

		public object Data()
		{
			return body.UserData;
		}

		public bool FixedRotation
		{
			get => body.FixedRotation;
			set => body.FixedRotation = value;
		}

		public float Mass
		{
			get => body.Mass;
			set => body.Mass = value;
		}

		public float Friction
		{
			set => body.Friction = value;
		}

		/// <summary>
		/// "Flick" the body towards a direction with a certain amount of force
		/// </summary>
		/// <param name="force"></param>
		public void ApplyForce(QVec force)
		{
			body.ApplyForce(force);
		}

		public bool Enabled
		{
			get => body.Enabled;
			set => body.Enabled = value;
		}

		/// <summary>
		/// If this body does not get a lot of interaction, it will fall asleep increaing performance
		/// </summary>
		public bool AllowSleep
		{
			get => body.SleepingAllowed;
			set => body.SleepingAllowed = value;
		}

		public float LinearDamping
		{
			get => body.LinearDamping;
			set => body.LinearDamping = value;
		}

		public float GravityScale
		{
			get => body.GravityScale;
			set => body.GravityScale = value;
		}

		public float Restitution
		{
			set => body.Restitution = value;
		}

		public bool IgnoreGravity
		{
			get => body.IgnoreGravity;
			set => body.IgnoreGravity = value;
		}

		/// <summary>
		/// makes the object have more accurate collision that enables it to act as a bullet
		/// </summary>
		public bool IsBullet
		{
			get => body.IsBullet;
			set => body.IsBullet = value;
		}

		/// <summary>
		/// Turns the physics into a trigger for events instead of interacting as an object
		/// </summary>
		public bool IsSensor
		{
			set => body.IsSensor = value;
		}

		public static QCollisionDirection Direction(QRigiBody b, QRigiBody b2)
		{
			QVec dir = (b.Transform.Position - b2.Transform.Position).Normalize();
			QCollisionDirection d;
			if(Math.Abs(dir.X) > Math.Abs(dir.Y))
			{
				if(dir.X > 0)
					d = QCollisionDirection.Right;
				d = QCollisionDirection.Left;
			}
			else
			{
				if(dir.Y > 0)
					d = QCollisionDirection.Bottom;
				d = QCollisionDirection.Top;
			}
			return d;
		}

		public bool Awake
		{
			get => body.Awake;
			set => body.Awake = value;
		}

		public bool IsIgnoreCcd
		{
			get => body.IgnoreCCD;
			set => body.IgnoreCCD = value;
		}

		internal QRigiBody(QBehavior sc, Body bo)
		{
			AttachedScript = sc;
			body = bo;
			Transform = sc.Transform;
			sc.OnDestory += () =>
			{
				//TODO FIX THIS DUPE SHIT // FIXED I THINK
				//MIGHT HAVE BEEN BUG IN VELCRO PHYSICS THAT I FIXED? BY CHECKING FOR NULL??
				//if(sc.World.world.BodyList.Contains(body))
				sc.World.world.RemoveBody(body);
				sc.World.Bodies.Remove(this);
			};
			body.OnCollision += (a, b, contact) =>
			{
				var script = b.Body.UserData as QBehavior;
				var booby = script.World.Bodies.Find(r => r.Id == script.Id);
				if(booby != null)
					OnCollision?.Invoke(booby);
			};
		}
	}
}