using System.Collections.Generic;
using System.Net;
using OpenGL;
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

		public QRigiBody CreateRectangle(QBehavior script, float w = 10, float h = 10, float density = 1, QBodyType bodyType = QBodyType.Dynamic)
		{
			var b = (BodyType)bodyType;
			var body = new QRigiBody(script, BodyFactory.CreateRectangle(world, w.ToSim(), h.ToSim(), density, script.Transform.Position.ToSim(), script.Transform.Rotation, b, script));
			script.Transform.Body = body;
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateRoundedRect(QBehavior script, float w = 10, float h = 10, float density = 1, QBodyType bodyType = QBodyType.Dynamic)
		{
			var d = 4f;
			var body = new QRigiBody(script, BodyFactory.CreateRoundedRectangle(world, w.ToSim(), h.ToSim(), w.ToSim() / d, h.ToSim() / d, 10, density, script.Transform.Position.ToSim(), script.Transform.Rotation, (BodyType)bodyType, script));
			script.Transform.Body = body;
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateCircle(QBehavior script, float radius = 10, float density = 1, QBodyType bodyType = QBodyType.Dynamic)
		{
			var b = (BodyType)bodyType;
			var body = new QRigiBody(script, BodyFactory.CreateCircle(world, radius.ToSim(), density, script.Transform.Position.ToSim(), b, script));
			script.Transform.Body = body;
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateEdge(QBehavior script, QVec start, QVec end)
		{
			var body = new QRigiBody(script, BodyFactory.CreateEdge(world, start.ToSim(), end.ToSim(), script));
			script.Transform.Body = body;
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateCapsule(QBehavior script, float height = 10f, float radius = 5f, float density = 1, QBodyType bodyType = QBodyType.Dynamic)
		{
			var b = (BodyType)bodyType;
			var body = new QRigiBody(script, BodyFactory.CreateCapsule(world, height.ToSim(), radius.ToSim(), density, script.Transform.Position.ToSim(), script.Transform.Rotation, b, script));
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
		public bool DidRaycastHit(QVec a, QVec b, out QRigiBody hit)
		{
			var aSim = a.ToSim();
			var bSim = b.ToSim();
			var fl = world.RayCast(aSim, aSim + bSim);
			if(fl.Count > 0)
			{
				var bd = fl[0].Body.UserData as QBehavior;
				hit = bd.World.Bodies.Find(r => r.Id == bd.Id);
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

		float PhysicsAccum { get; set; } = 0;

		const float Simulation = 1 / 61f;

		/// <summary>
		/// Moves all the bodies to the most recent transform 
		/// then simulates and then moves the transforms to the correct position 
		/// where the body was moved, simulation time
		/// </summary>
		/// <param name="t"></param>
		/// <param name="m"></param>
		bool Step(QTime t, QGameObjectManager m)
		{
			bool step = false;
			var delta = t.Delta;
			var simulation = Simulation;
//			if(t.IsLagging)
//				simulation = Simulation * 2;
			PhysicsAccum += delta;
			while(PhysicsAccum >= simulation)
			{
				QGameObjectManager.For(m.FixedUpdateObjects, u => u.OnFixedUpdate(simulation));
				world.Step(simulation);
				PhysicsAccum -= simulation;
				step = true;
			}
			QGameObjectManager.For(m.UpdateObjects, u => u.OnUpdate(delta));
			QGameObjectManager.For(m.LateUpdateObjects, l => l.OnLateUpdate(delta));
			if(step)
				ClearForces();
			/*Interpolation*/
			double alpha = PhysicsAccum / simulation;
			QGameObjectManager.For(Bodies, body =>
			{
				body.Script.Position = body.Position * (float)alpha +
				                       body.Script.Position * (1.0f - (float)alpha);
				body.Script.Rotation = body.Rotation * (float)alpha +
				                       body.Script.Rotation * (1.0f - (float)alpha);
			});
			return step;
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

		void ctor(float x, float y)
		{
//			body.OnCollision += (a, b, contact) =>
//			{
//				QBehavior script = b.Body.UserData as QBehavior;
//				if(script != null)
//				{
//					var booby = script.World.Bodies.Find(r => r.Id == script.Id);
//					if(booby != null)
//						OnCollision?.Invoke(booby);
//				}
//			};
			world = new World(new QVec(x, y));
			world.Clear();
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

		internal QWorldManager(float x = 0, float y = DefaultGravity)
		{
			ctor(x, y);
		}

		internal QWorldManager(QVec gravity)
		{
			ctor(gravity.X, gravity.Y);
		}
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