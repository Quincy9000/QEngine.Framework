using System.Linq;
using System.Collections.Generic;
using QEngine.Demos.CircleSpawnDemo;
using QPhysics;
using QPhysics.Dynamics;
using QPhysics.Factories;

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

		/// <summary>
		/// returns true if it hits a body and outs the first body that it hits
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="hit"></param>
		/// <returns></returns>
		public bool WhatDidRaycastHit(QVec a, QVec b, out List<QRigiBody> hit)
		{
			var aSim = a.ToSim();
			var bSim = b.ToSim();
			var fl = world.RayCast(aSim, aSim + bSim);
			if(fl.Count > 0)
			{
				List<QRigiBody> qbodies = new List<QRigiBody>();
				for(int i = 0; i < fl.Count; i++)
				{
					QBehavior script = fl[i].Body.UserData as QBehavior;
					var qbod = script?.World.Bodies.Find(bod => script.Id == bod.Id);
					if(qbod != null)
						qbodies.Add(qbod);
				}
				hit = qbodies;
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
		internal void TryStep(QTime time, QGameObjectManager m)
		{
			Step(time, m);
		}

		float PhysicsAccum { get; set; } = 60;

		const float Simulation = 1 / 60f;

		/// <summary>
		/// Moves all the bodies to the most recent transform 
		/// then simulates and then moves the transforms to the correct position 
		/// where the body was moved, simulation time
		/// </summary>
		/// <param name="t"></param>
		/// <param name="m"></param>
		void Step(QTime t, QGameObjectManager m)
		{
			bool step = false;
			PhysicsAccum += t.Delta;
			while(PhysicsAccum >= Simulation)
			{
				QGameObjectManager.For(m.FixedUpdateObjects, u => u.OnFixedUpdate(Simulation));
				world.Step(Simulation);
				PhysicsAccum -= Simulation;
				step = true;
			}
			QGameObjectManager.For(m.UpdateObjects, u => u.OnUpdate(t));
			QGameObjectManager.For(m.LateUpdateObjects, l => l.OnLateUpdate(t));
			if(step)
				ClearForces();
			/*Interpolation*/
			double alpha = PhysicsAccum / Simulation;
			QGameObjectManager.For(Bodies, body =>
			{
				//Only move objects that need to be moved
				if(!body.IsStatic)
				{
					body.Script.Position = body.Position * (float)alpha +
					                       body.Script.Position * (1.0f - (float)alpha);
					body.Script.Rotation = body.Rotation * (float)alpha +
					                       body.Script.Rotation * (1.0f - (float)alpha);
				}
			});
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
			if(Bodies.Contains(body))
				Bodies.Remove(body);
			if(world.BodyList.Contains(body.body))
			{
				/*Was removing them manually but I think process changes is a better way to remove a body
				annoy AF*/
				//world.BodyList.Remove(body.body);
				world.RemoveBody(body.body);
			}
			world.ProcessChanges();
		}

		public const float DefaultGravity = 9.807f;

		internal QWorldManager(float x = 0, float y = DefaultGravity)
		{
			world = new World(new QVec(x, y));
			for(int i = 0; i < 100; i++)
				BodyFactory.CreateRectangle(world, 1, 1, 1);
			BodyFactory.CreateRectangle(world, 1, 1, 1);
			world.Clear();
			world.ProcessChanges();
			world.ClearForces();
			world.ContactManager.OnBroadphaseCollision += (ref FixtureProxy a, ref FixtureProxy b) =>
			{
				//Finds the bodies if they have parent, and then passes them to the collision event
				QBehavior qa = a.Fixture.Body.UserData as QBehavior;
				QBehavior qb = b.Fixture.Body.UserData as QBehavior;
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