using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using QPhysics;
using QPhysics.Dynamics;
using QPhysics.Factories;
using QPhysics.Tools.PolygonManipulation;

namespace QEngine
{
	public class QWorldManager
	{
		internal World world;

		internal List<QRigiBody> Bodies { get; set; } = new List<QRigiBody>();

		/// <summary>
		/// Gets called when two rigibodies are about to collide
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public delegate void NearCollision(QRigiBody a, QRigiBody b);

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
		public QRigiBody CreateRectangle(QBehavior script, float w = 10, float h = 10, float density = 1,
			QBodyType bodyType = QBodyType.Dynamic)
		{
			QRigiBody body = Bodies.Find(bd => script.Id == bd.Id);
			if(body != null)
				return body;
			body = new QRigiBody(script,
				BodyFactory.CreateRectangle(world,
					w.ToSim(),
					h.ToSim(),
					density,
					script.Transform.Position.ToSim(),
					script.Transform.Rotation,
					(BodyType)bodyType,
					script));
			script.Transform.Body = body;
			Bodies.Add(body);
			return body;
		}

//		public QRigiBody CreateRoundedRect(QBehavior script, float w = 10, float h = 10, float density = 1, QBodyType bodyType = QBodyType.Dynamic)
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
		public QRigiBody CreateCircle(QBehavior script, float radius = 10, float density = 1,
			QBodyType bodyType = QBodyType.Dynamic)
		{
			//if the body already exists we just return that one
			QRigiBody body = Bodies.Find(bd => script.Id == bd.Id);
			if(body != null)
				return body;
			body = new QRigiBody(
				script,
				BodyFactory.CreateCircle(world,
					radius.ToSim(),
					density,
					script.Transform.Position.ToSim(),
					(BodyType)bodyType,
					script));
			script.Transform.Body = body;
			Bodies.Add(body);
			return body;
		}

		/// <summary>
		/// Creates an edge, line collision from point a to b
		/// </summary>
		/// <param name="script"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public QRigiBody CreateEdge(QBehavior script, QVec start, QVec end)
		{
			//if the body already exists we just return that one
			QRigiBody body = Bodies.Find(bd => script.Id == bd.Id);
			if(body != null)
				return body;
			body = new QRigiBody(script, BodyFactory.CreateEdge(world, start.ToSim(), end.ToSim(), script));
			script.Transform.Body = body;
			Bodies.Add(body);
			return body;
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
		public QRigiBody CreateCapsule(QBehavior script, float height = 10f, float radius = 5f, float density = 1,
			QBodyType bodyType = QBodyType.Dynamic)
		{
			//if the body already exists we just return that one
			QRigiBody body = Bodies.Find(bd => script.Id == bd.Id);
			if(body != null)
				return body;
			body = new QRigiBody(script,
				BodyFactory.CreateCapsule(world,
					height.ToSim(),
					radius.ToSim(),
					density,
					script.Transform.Position.ToSim(),
					script.Transform.Rotation,
					(BodyType)bodyType,
					script));
			script.Transform.Body = body;
			Bodies.Add(body);
			return body;
		}

		/*Trying to see if using linked list is faster than creating new lists for each ray cast*/
		LinkedList<QRigiBody> hitListNOGC = new LinkedList<QRigiBody>();

		/// <summary>
		/// returns true if it hits a body and outs the first body that it hits
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="hit"></param>
		/// <returns></returns>
		public bool WhatDidRaycastHit(QVec a, QVec b, out LinkedList<QRigiBody> hit)
		{
			var aSim = a.ToSim();
			var bSim = b.ToSim();
			var fl = world.RayCast(aSim, aSim + bSim);
			if(fl.Count > 0)
			{
				//List<QRigiBody> qbodies = new List<QRigiBody>();
				while(hitListNOGC.Count > 0)
					hitListNOGC.RemoveFirst();
				for(int i = 0; i < fl.Count; i++)
				{
					QBehavior script = fl[i].Body.UserData as QBehavior;
					var qbod = script?.World.Bodies.Find(bod => script.Id == bod.Id);
					if(qbod != null)
						hitListNOGC.AddFirst(qbod);
				}
				hit = hitListNOGC;
				return true;
			}
			hit = null;
			return false;
		}

		public bool DidRaycastHit(QVec a, QVec b)
		{
			var aSim = a.ToSim();
			var bSim = b.ToSim();
			var fl = world.RayCast(aSim, aSim + bSim);
			if(fl.Count > 0)
			{
				return true;
			}
			return false;
		}

		public QVec Gravity
		{
			get => world.Gravity;
			set => world.Gravity = value;
		}

		/// <summary>
		/// Will try to update physics if its been a certain amount of time
		/// </summary>
		/// <param name="time"></param>
		internal QPhysicsState TryStep(QTime time, QGameObjectManager m)
		{
			return Step(time, m);
		}

		float PhysicsAccumulator { get; set; }

		const float StepSimluation = 1 / 60f;

		/// <summary>
		/// Moves all the bodies to the most recent transform 
		/// then simulates and then moves the transforms to the correct position 
		/// where the body was moved, simulation time
		/// </summary>
		/// <param name="t"></param>
		/// <param name="m"></param>
		QPhysicsState Step(QTime t, QGameObjectManager m)
		{
//			for(int i = 0; i < Bodies.Count; i++)
//			{
//				QRigiBody b = Bodies[i];
//				if(b.Script.Transform.IsDirty)
//				{
//					b.Position = b.Script.Transform.Position;
//					b.Rotation = b.Script.Transform.Rotation;
//				}
//			}
			bool step = false;
			PhysicsAccumulator += t.Delta;
			QGameObjectManager.For(m.UpdateObjects, u => u.OnUpdate(t));
			while(PhysicsAccumulator >= StepSimluation)
			{
				//previous = current
//				for(int i = 0; i < Bodies.Count; i++)
//				{
//					QRigiBody b = Bodies[i];
//					if(b.IsDynamic)
//					{
//						b.PreviousPosition = b.Position;
//						b.PreviousRotation = b.Rotation;
//					}
//				}
				QGameObjectManager.For(m.FixedUpdateObjects, u => u.OnFixedUpdate(StepSimluation));
				world.Step(StepSimluation);
				PhysicsAccumulator -= StepSimluation;
				if(!step)
					step = true;
			}
			if(step)
				ClearForces();
			QGameObjectManager.For(m.LateUpdateObjects, l => l.OnLateUpdate(t));
			return 0;//Interpolate(PhysicsAccumulator / StepSimluation);
		}

		float Interpolate(QPhysicsState alpha)
		{
			float a = alpha;
			for(int i = 0; i < Bodies.Count; i++)
			{
				QRigiBody body = Bodies[i];
				if(body.IsDynamic)
				{
					body.Script.Transform.Position = body.Position * a + body.Script.Transform.Position * (1.0f - a);
					body.Script.Transform.Rotation = body.Rotation * a + body.Script.Transform.Rotation * (1.0f - a);
				}
			}
			return a;
		}

		/// <summary>
		/// Clears the forces after a physics sub step, using this only once after all substeps have been completed
		/// </summary>
		internal void ClearForces()
		{
			if(!Settings.AutoClearForces)
				world.ClearForces();
		}

		internal void Clear()
		{
			world.Clear();
		}

		public void RemoveBody(QRigiBody body)
		{
			Bodies.Remove(body);
			world.RemoveBody(body.body);
		}

		public const float DefaultGravity = 9.807f;

		internal QWorldManager(float x = 0, float y = DefaultGravity)
		{
			world = new World(new QVec(x, y));
			world.Clear();
			world.ContactManager.OnBroadphaseCollision += (ref FixtureProxy a, ref FixtureProxy b) =>
			{
				//Finds the bodies if they have parent, and then passes them to the collision event
				QBehavior qa = a.Fixture.Body.UserData as QBehavior;
				QBehavior qb = b.Fixture.Body.UserData as QBehavior;
				if(qa == null || qb == null) return;
				QRigiBody bodya = qa.World.Bodies.Find(r => r.Id == qa.Id);
				QRigiBody bodyb = qb.World.Bodies.Find(r => r.Id == qb.Id);
				if(bodya != null && bodyb != null)
					OnCollision?.Invoke(bodya, bodyb);
			};
		}

		internal QWorldManager(QVec gravity) : this(gravity.X, gravity.Y) { }
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