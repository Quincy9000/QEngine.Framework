﻿using System.Collections.Generic;
using System.Net;
using QPhysics;
using QPhysics.Dynamics;
using QPhysics.Factories;

namespace QEngine
{
	public class QWorldManager
	{
		internal World world;

		internal List<QRigiBody> Bodies { get; set; } = new List<QRigiBody>();

		public QRigiBody CreateRectangle(QBehavior script, float w = 10, float h = 10, float density = 1, QBodyType bodyType = QBodyType.Dynamic)
		{
			var b = (BodyType)bodyType;
			var body = new QRigiBody(script, BodyFactory.CreateRectangle(world, w.ToSim(), h.ToSim(), density, script.Transform.Position.ToSim(), script.Transform.Rotation, b, script));
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateRoundedRect(QBehavior script, float w = 10, float h = 10, float density = 1, QBodyType bodyType = QBodyType.Dynamic)
		{
			var d = 4f;
			var bf = BodyFactory.CreateRoundedRectangle(world, w.ToSim(), h.ToSim(), w.ToSim() / d, h.ToSim() / d, 10, density, script.Transform.Position.ToSim(), script.Transform.Rotation, (BodyType)bodyType, script);
			var body = new QRigiBody(script, bf);
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateCircle(QBehavior script, float radius = 10, float density = 1, QBodyType bodyType = QBodyType.Dynamic)
		{
			var b = (BodyType)bodyType;
			var body = new QRigiBody(script, BodyFactory.CreateCircle(world, radius.ToSim(), density, script.Transform.Position.ToSim(), b, script));
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateEdge(QBehavior script, QVec start, QVec end)
		{
			var body = new QRigiBody(script, BodyFactory.CreateEdge(world, start.ToSim(), end.ToSim(), script));
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateCapsule(QBehavior script, float height = 10f, float radius = 5f, float density = 1, QBodyType bodyType = QBodyType.Dynamic)
		{
			var b = (BodyType)bodyType;
			var body = new QRigiBody(script, BodyFactory.CreateCapsule(world, height.ToSim(), radius.ToSim(), density, script.Transform.Position.ToSim(), script.Transform.Rotation, b, script));
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
		internal bool TryStep(QTime time, QGameObjectManager m)
		{
			return step(time, m);
		}

		float PhysicsAccum { get; set; } = 0;

		const float simulation = 1 / 60f;

		/// <summary>
		/// Moves all the bodies to the most recent transform, then simulates and then moves the transforms to the correct position where the body was moved, simulation time
		/// </summary>
		/// <param name="time"></param>
		bool step(QTime t, QGameObjectManager m)
		{
			bool step = false;
			var delta = t.Delta;
//This code was never proper it seemed to work and update objects but wasnt needed
//            for(int i = 0; i < Bodies.Count; i++)
//            {
//                Bodies[i].Position = Bodies[i].Transform.Position;
//                Bodies[i].Rotation = Bodies[i].Transform.Rotation;
//            }
			//TODO FIX THIS GOD DAMN TRANSFORM MOVEMENT
			PhysicsAccum += delta;
			while(PhysicsAccum >= simulation)
			{
				world.Step(simulation);
				PhysicsAccum -= simulation;
				QGameObjectManager.For(m.FixedUpdateObjects, u => u.OnFixedUpdate(simulation));
				step = true;
			}
			QGameObjectManager.For(m.UpdateObjects, u => u.OnUpdate(delta));
			QGameObjectManager.For(m.LateUpdateObjects, l => l.OnLateUpdate(delta));
			if(step)
				ClearForces();
			/*Interpolation*/
			double alpha = PhysicsAccum / simulation;
			for(int i = 0; i < Bodies.Count; i++)
			{
				Bodies[i].Transform.Position = Bodies[i].Position * (float)alpha + Bodies[i].Transform.Position * (1.0f - (float)alpha);
				Bodies[i].Transform.Rotation = Bodies[i].Rotation * (float)alpha + Bodies[i].Transform.Rotation * (1.0f - (float)alpha);
			}
			return step;
		}

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
				//world.BodyList.Remove(body.body);
				world.RemoveBody(body.body);
			}
			world.ProcessChanges();
		}

		public const float DefaultGravity = 9.807f;

		void ctor(float x, float y)
		{
			world = new World(new QVec(x, y));
			world.Clear();
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