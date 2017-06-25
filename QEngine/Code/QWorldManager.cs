﻿using System.Collections.Generic;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace QEngine
{
	public class QWorldManager
	{
		internal World world;

		internal List<QRigiBody> Bodies { get; set; } = new List<QRigiBody>();

		public QRigiBody CreateRectangle(QBehavior script, float w = 10, float h = 10, float density = 1, QVec pos = default(QVec), float rotation = 0, QBodyType bodyType = QBodyType.Dynamic)
		{
			var b = (BodyType)bodyType;
			var body = new QRigiBody(script, BodyFactory.CreateRectangle(world, w.ToSim(), h.ToSim(), density, pos.ToSim(), rotation, b, script));
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateCircle(QBehavior script, float radius = 10, float density = 1, QVec pos = default(QVec), QBodyType bodyType = QBodyType.Dynamic)
		{
			var b = (BodyType)bodyType;
			var body = new QRigiBody(script, BodyFactory.CreateCircle(world, radius.ToSim(), density, pos.ToSim(), b, script));
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateEdge(QBehavior script, QVec start = default(QVec), QVec end = default(QVec))
		{
			var body = new QRigiBody(script, BodyFactory.CreateEdge(world, start.ToSim(), end.ToSim(), script));
			Bodies.Add(body);
			return body;
		}

		public QRigiBody CreateCapsule(QBehavior script, float height = 10f, float radius = 5f, float density = 1, QVec pos = default(QVec), float rotation = 0, QBodyType bodyType = QBodyType.Dynamic)
		{
			var b = (BodyType)bodyType;
			var body = new QRigiBody(script, BodyFactory.CreateCapsule(world, height.ToSim(), radius.ToSim(), density, pos.ToSim(), rotation, b, script));
			Bodies.Add(body);
			return body;
		}

		public float RayCastHitDistance(QVec a, QVec b)
		{
			var aSim = a.ToSim();
			var bSim = b.ToSim();
			var fl = world.RayCast(aSim, aSim + bSim);
			if(fl.Count > 0)
			{
				return QVec.Distance(a, ConvertUnits.ToDisplayUnits(fl[0].Body.Position));
			}
			return -1;
		}

		public QVec Gravity
		{
			get => world.Gravity;
			set => world.Gravity = value;
		}

		internal void Step(QTime time)
		{
			step(time.Delta);
		}

		internal void Step(float f)
		{
			step(f);
		}

		/// <summary>
		/// Moves all the bodies to the most recent transform, then simulates and then moves the transforms to the correct position where the body was moved, simulation time
		/// </summary>
		/// <param name="time"></param>
		void step(float f)
		{
			for(int i = 0; i < Bodies.Count; i++)
			{
				Bodies[i].Position = Bodies[i].AttachedScript.Transform.Position;
				Bodies[i].Rotation = Bodies[i].AttachedScript.Transform.Rotation;
			}
			world.Step(f);
			for(int i = 0; i < Bodies.Count; i++)
			{
				Bodies[i].AttachedScript.Transform.Position = Bodies[i].Position;
				Bodies[i].AttachedScript.Transform.Rotation = Bodies[i].Rotation;
			}
		}

		internal void Clear()
		{
			world.Clear();
		}

		public const float DefaultGravity = 9.807f;

		internal QWorldManager(float x = 0, float y = DefaultGravity)
		{
			world = new World(new QVec(x, y));
		}

		internal QWorldManager(QVec gravity)
		{
			world = new World(Gravity);
		}
	}
}