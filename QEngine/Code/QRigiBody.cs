using System;
using QPhysics.Dynamics;

namespace QEngine
{
	public class QRigiBody
	{
		internal Body body;

		public QBehavior Script { get; }

		internal Guid Id => Script.Id;

		internal QVec Position
		{
			get => ((QVec)body.Position).ToDis();
			set
			{
				Awake = true;
				body.Position = value.ToSim();
			}
		}

		internal void MoveBody(ref QVec v)
		{
			body.SetTransform(v.ToSim(), Rotation);
		}

		internal void RotateBody(ref float f)
		{
			body.SetTransform(Position.ToSim(), f);
		}

		public delegate void Collision(QRigiBody other);

		public event Collision OnCollision;

		/// <summary>
		/// Set or get the rotation that the body is, in radians, 2Pi = 360 degree etc
		/// </summary>
		internal float Rotation
		{
			get => body.Rotation;
			set => body.Rotation = value;
		}

		/// <summary>
		/// Direction and speed of the bodies movement
		/// </summary>
		public QVec LinearVelocity
		{
			get => body.LinearVelocity;
			set => body.LinearVelocity = value;
		}

		public object Data()
		{
			return body.UserData;
		}

		public T Data<T>()
		{
			return (T)body.UserData;
		}

		/// <summary>
		/// Whether it rotates or not
		/// </summary>
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

		/// <summary>
		/// Rubbing Friction
		/// </summary>
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

		/// <summary>
		/// Air friction
		/// </summary>
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

		/// <summary>
		/// Bounciness
		/// </summary>
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

		public bool IsStatic => body.IsStatic;

		public bool IsDynamic => body.IsDynamic;

		public bool IsKinematic => body.IsKinematic;

		/// <summary>
		/// useful for the direction of collision, not much else, dont use with platforms that are really big
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static QCollisionDirection Direction(QRigiBody a, QRigiBody b)
		{
			QVec dir = (a.Script.Position - b.Script.Position).Normalize();
			QCollisionDirection d;
			if(Math.Abs(dir.X) > Math.Abs(dir.Y))
			{
				if(dir.X > 0)
					d = QCollisionDirection.Right;
				else
					d = QCollisionDirection.Left;
			}
			else
			{
				if(dir.Y > 0)
					d = QCollisionDirection.Bottom;
				else
					d = QCollisionDirection.Top;
			}
			return d;
		}

		public bool Awake
		{
			get => body.Awake;
			set => body.Awake = value;
		}

		/// <summary>
		/// Is Constant Collision Detection On?
		/// </summary>
		public bool IsCCD
		{
			get => !body.IgnoreCCD;
			set => body.IgnoreCCD = !value;
		}

		internal QRigiBody(QBehavior sc, Body bo)
		{
			Script = sc;
			body = bo;
			sc.OnDestroyEvent += () =>
			{
				sc.World.RemoveBody(this);
			};
			body.OnCollision += (a, b, contact) =>
			{
				QBehavior script = (QBehavior)b.Body.UserData;
				if(script == null) return;

				var bodySearch = script.World.Bodies.Find(r => r.Id == script.Id);
				if(bodySearch != null)
					OnCollision?.Invoke(bodySearch);
			};
		}
	}
}