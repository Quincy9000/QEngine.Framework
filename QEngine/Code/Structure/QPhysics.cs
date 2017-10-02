using System.Collections.Generic;
using QEngine.Physics.Factories;
using QEngine.Physics.Dynamics;

namespace QEngine
{
	public class QPhysics
	{
		internal World PhysicsWorld { get; private set; }

		internal List<QBody> Bodies { get; set; } = new List<QBody>();

		/// <summary>
		/// Gets called when two rigibodies are about to collide
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public delegate void NearCollision(QBody a, QBody b);

		/// <summary>
		/// General collision event that isnt tied to specific objects, must check yourself
		/// </summary>
		public event NearCollision OnCollision;

		/// <summary>
		/// Creates a rectangle, turns this script into a physics object, 
		/// you then need to do all updates in fixedUpdate
		/// </summary>
		/// <param name="script"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="density"></param>
		/// <param name="bodyType"></param>
		/// <returns></returns>
		public QBody CreateRectangle(QBehavior script, float w = 10, float h = 10, float density = 1,
			QBodyTypes bodyType = QBodyTypes.Dynamic)
		{
			QBody body = FindBody(script);
			if(body != null)
				return body;
			body = new QBody(script,
				BodyFactory.CreateRectangle(PhysicsWorld,
					w.ToSim(),
					h.ToSim(),
					density,
					script.Transform.Position.ToSim(),
					script.Transform.Rotation,
					(BodyType)bodyType,
					script));
			return AddBody(body);
		}

//		public QRigiBody CreateRoundedRect(QBehavior script, float w = 10, float h = 10, float density = 1, QBodyTypes bodyType = QBodyTypes.Dynamic)
//		{
//			var d = 4f;
//			var body = new QRigiBody(script, BodyFactory.CreateRoundedRectangle(world, w.ToSim(), h.ToSim(), w.ToSim() / d, h.ToSim() / d, 10, density, script.Transform.Position.ToSim(), script.Transform.Rotation, (BodyType)bodyType, script));
//			script.Transform.Body = body;
//			Bodies.Add(body);
//			return body;
//		}

		/// <summary>
		/// Creates a Circle, turns this script into a physics object, 
		/// you then need to do all updates in fixedUpdate
		/// </summary>
		/// <param name="script"></param>
		/// <param name="radius"></param>
		/// <param name="density"></param>
		/// <param name="bodyType"></param>
		/// <returns></returns>
		public QBody CreateCircle(QBehavior script, float radius = 10, float density = 1,
			QBodyTypes bodyType = QBodyTypes.Dynamic)
		{
			//if the body already exists we just return that one
			QBody body = FindBody(script);
			if(body != null)
				return body;
			body = new QBody(
				script,
				BodyFactory.CreateCircle(PhysicsWorld,
					radius.ToSim(),
					density,
					script.Transform.Position.ToSim(),
					(BodyType)bodyType,
					script));
			return AddBody(body);
		}

		/// <summary>
		/// Creates an edge, line collision from point a to b
		/// </summary>
		/// <param name="script"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public QBody CreateEdge(QBehavior script, QVector2 start, QVector2 end)
		{
			//if the body already exists we just return that one
			QBody body = FindBody(script);
			if(body != null)
				return body;
			body = new QBody(script, BodyFactory.CreateEdge(PhysicsWorld, start.ToSim(), end.ToSim(), script));
			return AddBody(body);
		}

		/// <summary>
		/// Creates a capsule, turns this script into a physics object, 
		/// you then need to do all updates in fixedUpdate
		/// </summary>
		/// <param name="script"></param>
		/// <param name="height"></param>
		/// <param name="radius"></param>
		/// <param name="density"></param>
		/// <param name="bodyType"></param>
		/// <returns></returns>
		public QBody CreateCapsule(QBehavior script, float height = 10f, float radius = 5f, float density = 1,
			QBodyTypes bodyType = QBodyTypes.Dynamic)
		{
			//if the body already exists we just return that one
			QBody body = FindBody(script);
			if(body != null)
				return body;
			body = new QBody(script,
				BodyFactory.CreateCapsule(PhysicsWorld,
					height.ToSim(),
					radius.ToSim(),
					density,
					script.Transform.Position.ToSim(),
					script.Transform.Rotation,
					(BodyType)bodyType,
					script));
			return AddBody(body);
		}

		QBody FindBody(QBehavior script)
		{
			return Bodies.Find(bd => script.Id == bd.Id);
		}

		QBody AddBody(QBody body)
		{
//			script.Transform.Body = body;
			Bodies.Add(body);
			return body;
		}

		/*Trying to see if using linked list is faster than creating new lists for each ray cast*/
		//LinkedList<QRigiBody> hitListNOGC = new LinkedList<QRigiBody>();
		readonly List<QBody> _hitListNogc = new List<QBody>(10);

		/// <summary>
		/// returns true if it hits a body and outs the first body that it hits
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="hit"></param>
		/// <returns></returns>
		public bool WhatDidRaycastHit(QVector2 a, QVector2 b, out List<QBody> hit)
		{
			var aSim = a.ToSim();
			var bSim = b.ToSim();
			var fl = PhysicsWorld.RayCast(aSim, aSim + bSim);
			hit = null;
			if(fl.Count > 0)
			{
				//List<QRigiBody> qbodies = new List<QRigiBody>();
				while(_hitListNogc.Count > 0)
				{
					//hitListNOGC.RemoveFirst();
					_hitListNogc.Clear();
				}
				for(int i = 0; i < fl.Count; i++)
				{
					QBehavior script = fl[i].Body.UserData as QBehavior;
					var qbod = script?.World.Physics.Bodies.Find(bod => script.Id == bod.Id);
					if(qbod != null)
						_hitListNogc.Add(qbod);
				}
				hit = _hitListNogc;
				return true;
			}
			return false;
		}

		public bool DidRaycastHit(QVector2 a, QVector2 b)
		{
			var aSim = a.ToSim();
			var bSim = b.ToSim();
			var fl = PhysicsWorld.RayCast(aSim, aSim + bSim);
			if(fl.Count > 0)
			{
				return true;
			}
			return false;
		}

		public QVector2 Gravity
		{
			get => PhysicsWorld.Gravity;
			set => PhysicsWorld.Gravity = value;
		}

		float PhysicsAccumulator { get; set; }

		const float StepSimluation = 1 / 60f;

		/// <summary>
		/// Moves all the bodies to the most recent transform 
		/// then simulates and then moves the transforms to the correct position 
		/// where the body was moved, simulation time
		/// https://gafferongames.com/post/fix_your_timestep/
		/// </summary>
		/// <param name="fDelta"></param>
		/// <param name="gameManager"></param>
		internal void TryStep(float fDelta, QEntityManager gameManager)
		{
			Bodies.For(body =>
			{
				body.Position = body.Script.Position;
				body.Rotation = body.Script.Rotation;
			});

			bool step = false;
			PhysicsAccumulator += fDelta;
			while(PhysicsAccumulator > StepSimluation)
			{
				gameManager.FixedUpdateObjects.For(u => u.OnFixedUpdate());
				PhysicsWorld.Step(StepSimluation);
				PhysicsAccumulator -= StepSimluation;
				step = true;
			}

			Bodies.For(body =>
			{
				if(!body.IsStatic)
				{
					body.Script.Position = body.Position;
					body.Script.Rotation = body.Rotation;
				}
			});

			if(step)
				ClearForces();
		}

		/// <summary>
		/// Clears the forces after a physics sub step, using this only once after all substeps have been completed
		/// </summary>
		internal void ClearForces()
		{
			PhysicsWorld.ClearForces();
		}

		internal void Clear()
		{
			PhysicsWorld.Clear();
		}

		public void RemoveBody(QBody body)
		{
			Bodies.Remove(body);
			PhysicsWorld.RemoveBody(body.body);
		}

		public const float DefaultGravity = 9.807f;

		internal QPhysics(float x = 0, float y = DefaultGravity)
		{
			PhysicsWorld = new World(new QVector2(x, y));
			PhysicsWorld.ContactManager.OnBroadphaseCollision += (ref FixtureProxy a, ref FixtureProxy b) =>
			{
				//Finds the bodies if they have parent, and then passes them to the collision event
				QBehavior qa = a.Fixture.Body.UserData as QBehavior;
				QBehavior qb = b.Fixture.Body.UserData as QBehavior;
				if(qa == null || qb == null) return;
				QBody bodya = qa.World.Physics.Bodies.Find(r => r.Id == qa.Id);
				QBody bodyb = qb.World.Physics.Bodies.Find(r => r.Id == qb.Id);
				if(bodya != null && bodyb != null)
					OnCollision?.Invoke(bodya, bodyb);
			};
		}

		internal QPhysics(QVector2 gravity) : this(gravity.X, gravity.Y) { }
	}
}

/*This issue has been fixed, you have to use the right position for it to work smoothly*/

//This code was never proper it seemed to work and update objects but wasnt needed
//            for(int i = 0; i < Bodies.Count; i++)
//            {
//                Bodies[i].Position = Bodies[i].Transform.Position;
//                Bodies[i].Rotation = Bodies[i].Transform.Rotation;
//            }
//TODO FIX THIS GOD DAMN TRANSFORM MOVEMENT

//http://www.gamasutra.com/blogs/BramStolk/20160408/269988/Fixing_your_time_step_the_easy_way_with_the_golden_48537_ms.php
//		static int stepsToTake(double elapsed)
//		{
//			// Our simulation frequency is 480Hz, a 2𐧶 (two one twelfth) ms.
//			const double e = 1 / 480.0;
//
//			// We will pretend our display sync rate is one of these:
//			if(elapsed > 15.5 * e)
//				return 16; // 30 Hz        ( .. to 30.97 Hz )
//			else if(elapsed > 14.5 * e)
//				return 15; // 32 Hz        ( 30.97 Hz to 33.10 Hz )
//			else if(elapsed > 13.5 * e)
//				return 14; // 36.92 Hz     ( 33.10 Hz to 38.4 Hz )
//			else if(elapsed > 12.5 * e)
//				return 13; // 40 Hz        ( 38.4 Hz to 41.74 Hz )
//			else if(elapsed > 11.5 * e)
//				return 12; // 43.64Hz      ( 41.74 Hz to 45.71 Hz )
//			else if(elapsed > 10.5 * e)
//				return 11; // 48 Hz        ( 45.71 Hz to 50.53 Hz )
//			else if(elapsed > 9.5 * e)
//				return 10; // 53.33 Hz     ( 50.53 Hz to 56.47 Hz )
//			else if(elapsed > 8.5 * e)
//				return 9; // 60 Hz        ( 56.47 Hz to 64 Hz )
//			else if(elapsed > 7.5 * e)
//				return 8; // 68.57 Hz     ( 64 Hz to 73.85 Hz )
//			else if(elapsed > 6.5 * e)
//				return 7; // 80 Hz        ( 73.85 Hz to 87.27 Hz )
//			else if(elapsed > 5.5 * e)
//				return 6; // 96 Hz        ( 87.27 Hz to 106.67 Hz )
//			else if(elapsed > 4.5 * e)
//				return 5; // 120 Hz       ( 106.67 Hz to 137.14 Hz )
//			else if(elapsed > 3.5 * e)
//				return 4; // 160 Hz       ( 137.14 Hz to 192 Hz )
//			else if(elapsed > 2.5 * e)
//				return 2; // 240 Hz       ( 192 Hz to 320 Hz )
//			else if(elapsed > 1.5 * e)
//				return 2; // 480 Hz       ( 320 Hz to .. )
//			else
//				return 1;
//		}

//QGameObjectManager.For(Bodies, b =>
//{
////Only move objects that need to be moved, and that actively move
//if(b.IsDynamic)
//{
//b.Script.Position = b.Position * (float)alpha +
//b.Script.Position * (float)(1.0 - alpha);
//b.Script.Rotation = b.Rotation * (float)alpha +
//b.Script.Rotation * (float)(1.0 - alpha);
//}
//});

//		float Interpolate(float alpha)
//		{
//			for (int i = 0; i < Bodies.Count; i++)
//			{
//				QRigidBody body = Bodies[i];
//				if (body.IsDynamic)
//				{
//					body.Script.Transform.Position = body.Position * alpha + body.Script.Transform.position * (1.0f - alpha);
//					body.Script.Transform.Rotation = body.Rotation * alpha + body.Script.Transform.rotation * (1.0f - alpha);
//				}
//			}
//			return alpha;
//		}