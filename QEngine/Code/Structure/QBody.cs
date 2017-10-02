using System;
using QEngine.Physics.Collision.ContactSystem;
using QEngine.Physics.Dynamics;

namespace QEngine
{
	public class QBody
	{
		internal Guid Id => Script.Id;

		internal Body body { get; set; }

		public QBehavior Script { get; }

		public QVector2 Position
		{
			get => ((QVector2) body.Position).ToDis();
			set
			{
				body.Position = value.ToSim();
				Awake = true;
			}
		}

		/// <summary>
		/// Set or get the rotation that the body is, in radians, 2Pi = 360 degree etc
		/// </summary>
		public float Rotation
		{
			get => body.Rotation;
			set => body.Rotation = value;
		}

		public delegate void EnterCollision(QBody other);

		public delegate void WhileCollision(QBody other);

		public delegate void ExitCollision(QBody other);

		/// <summary>
		/// Fires only once on the start of two bodies colliding
		/// </summary>
		public event EnterCollision OnCollisionEnter;

		/// <summary>
		/// Event fires while the collision is still touching
		/// </summary>
		public event WhileCollision OnCollisionStay;

		/// <summary>
		/// Fires when two bodies stop colliding
		/// </summary>
		public event ExitCollision OnCollisionExit;

		/// <summary>
		/// Direction and speed of the bodies movement
		/// </summary>
		public QVector2 LinearVelocity
		{
			get => body.LinearVelocity;
			set => body.LinearVelocity = value;
		}


		/// <summary>
		/// Whether it rotates or not
		/// </summary>
		public bool FixedRotation
		{
			get => body.FixedRotation;
			set => body.FixedRotation = value;
		}

		/// <summary>
		/// Mass of body in kilo
		/// </summary>
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
		public void ApplyForce(QVector2 force)
		{
			body.ApplyForce(force);
		}

		/// <summary>
		/// Applies force instantly to a body
		/// </summary>
		/// <param name="force"></param>
		public void ApplyImpulse(QVector2 force)
		{
			body.ApplyLinearImpulse(force);
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
		public static QCollisionDirections Direction(QBody a, QBody b)
		{
			QVector2 dir = (a.Script.Position - b.Script.Position).Normalize();
			QCollisionDirections d;
			if (Math.Abs(dir.X) > Math.Abs(dir.Y))
			{
				if (dir.X > 0)
					d = QCollisionDirections.Right;
				else
					d = QCollisionDirections.Left;
			}
			else
			{
				if (dir.Y > 0)
					d = QCollisionDirections.Bottom;
				else
					d = QCollisionDirections.Top;
			}
			return d;
		}

		public bool Awake
		{
			get => body.Awake;
			set => body.Awake = value;
		}

		/// <summary>
		/// Is Constant OnCollisionEnter Detection On?
		/// </summary>
		public bool IsConstantCollisionDetection
		{
			get => body.IgnoreCCD;
			set => body.IgnoreCCD = value;
		}

		void OnDestroyHandler()
		{
			Script.World.Physics.RemoveBody(this);
			OnCollisionEnter = null;
			OnCollisionStay = null;
			OnCollisionExit = null;
			body.UserData = null;
			body = null;
		}

		void OnCollisionEnterHandler(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			if (fixtureB.Body.UserData == null)
				return;
			Guid otherId = ((QBehavior) fixtureB.Body.UserData).Id;
			QBody otherBody = Script.Physics.Bodies.Find(bd => bd.Id == otherId);
			if (otherBody != null)
				OnCollisionEnter?.Invoke(otherBody);
		}

		void OnCollisionStayHandler(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			if (fixtureB.Body.UserData == null)
				return;
			Guid otherId = ((QBehavior) fixtureB.Body.UserData).Id;
			QBody otherBody = Script.Physics.Bodies.Find(bd => bd.Id == otherId);
			if (otherBody != null)
				OnCollisionStay?.Invoke(otherBody);
		}

		void OnCollisionExitHandler(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			if (fixtureB.Body.UserData == null)
				return;
			Guid otherId = ((QBehavior) fixtureB.Body.UserData).Id;
			QBody otherBody = Script.Physics.Bodies.Find(bd => bd.Id == otherId);
			if (otherBody != null)
				OnCollisionExit?.Invoke(otherBody);
		}

		internal QBody(QBehavior script, Body body)
		{
			Script = script;
			this.body = body;
			Script.OnDestroyEvent += OnDestroyHandler;
			this.body.OnCollision += OnCollisionEnterHandler;
			this.body.OnCollisionStay += OnCollisionStayHandler;
			this.body.OnSeparation += OnCollisionExitHandler;
		}
	}
}