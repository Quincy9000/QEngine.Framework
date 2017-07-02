﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using Microsoft.Xna.Framework;
using QEngine.Debug;
using QPhysics.Dynamics;
using QPhysics.Tools.Cutting;
using QPhysics.Utilities;

namespace QEngine
{
	/// <summary>
	/// Base class for all scenes, inherit this to create a scene in qengine
	/// </summary>
	public abstract class QScene
	{
		/*Internal Variables*/

		internal string Name { get; }

		internal QGameObjectManager GameObjects { get; set; }

		internal QEngine Engine { get; set; }

		internal QSpriteRenderer SpriteRenderer { get; set; }

		internal QGuiRenderer GuiRenderer { get; set; }

		internal QContentManager Content { get; set; }

		internal QCoroutine Coroutine { get; private set; }

		internal QControls Input { get; private set; }

		internal QCamera Camera { get; private set; }

		internal QDebug Debug { get; private set; }

		internal QConsole Console { get; private set; }

		internal QAccum Accumulator { get; private set; }

		internal QWorldManager World { get; private set; }

		internal QMegaTexture MegaTexture { get; private set; }

		internal Stopwatch FrameTime { get; set; }

		internal QDebugView DebugView { get; set; }

		/*Privates*/

		List<QObject> CreatorQueue { get; set; }

		Queue<QBehavior> DestroyQueue { get; set; }

		bool CreatorFlag = false;

		/*Public Methods*/

		/// <summary>
		/// Reloads the current scene
		/// </summary>
		public void ResetScene()
		{
			Engine.Manager.ResetScene();
		}

		/// <summary>
		/// Changes to a scene that exists
		/// </summary>
		/// <param name="s"></param>
		public void ChangeScene(string s)
		{
			Engine.Manager.ChangeScene(s);
		}

		public QRect Window => Engine.GraphicsDevice.Viewport.Bounds;

		public void Instantiate(QBehavior script, QVec pos = default(QVec), float rotation = 0)
		{
			script.Parent = QObject.NewObject();
			script.Parent.Scene = this;
			script.Parent.Script = script;
			script.Transform.Reset(pos, QVec.One, rotation);
			script.SetName();
			CreatorQueue.Add(script.Parent);
			CreatorFlag = true;
		}

		/// <summary>
		/// Destroys script
		/// </summary>
		/// <param name="script"></param>
		public void Destroy(QBehavior script)
		{
			if(!DestroyQueue.Contains(script))
				DestroyQueue.Enqueue(script);
		}

		/*Private Methods*/

		/// <summary>
		/// frees up qobject and removes it from all arrays, sounds cool
		/// </summary>
		/// <param name="script"></param>
		void Obliterate(QBehavior script)
		{
			GameObjects.Remove(script);
			QObject.DeleteObject(script.Parent);
		}

		/*Protected Virtuals*/

		/// <summary>
		/// Load all your scripts here, and the scene will call all loads and compile the texture
		/// before it is instantiated so that there is not loading textures during gameplay,
		/// but totally optional
		/// </summary>
		protected virtual void BehaviorScriptLoader(List<IQLoad> scripts) { }

		/// <summary>
		/// Instantiate all your objects here
		/// </summary>
		protected virtual void Load() { }

		/// <summary>
		/// Do some stuff here before unloading the scene like saving etc..
		/// </summary>
		protected virtual void Unload() { }

		/*Internal Methods*/

		/// <summary>
		/// Adds all the items in the queue into the scene on the next frame
		/// </summary>
		internal void ObjectCreator()
		{
			while(CreatorFlag)
			{
				//Add all the queue to the main array
				QGameObjectManager.For(CreatorQueue, o => GameObjects.Add(o.Script));
				//Then make temp array to store the queue
				var temp = CreatorQueue.ToArray();
				//reset the queue for objects that might get added from the OnStart() from the queued objects
				CreatorQueue = new List<QObject>();
				//Set the flag to false again, Instantiate makes it flip, means we need to load more objects
				CreatorFlag = false;
				QGameObjectManager.For(temp, o =>
				{
					if(o.Script is IQLoad l)
						l.OnLoad(new QAddContent(Content));
				});
				MegaTexture = Content.MegaTexture;
				//could potentially set flag to true
				QGameObjectManager.For(temp, o =>
				{
					if(o.Script is IQStart s)
						s.OnStart(new QGetContent(Content));
				});
			}
		}

		/// <summary>
		/// Removes all the items from the destroy queue
		/// </summary>
		internal void ObjectDestroyer()
		{
/*			for(var i = 0; i < DestroyQueue.Count; i++)
				Obliterate(DestroyQueue[i]);*/
			while(DestroyQueue.Count > 0)
				Obliterate(DestroyQueue.Dequeue());
		}

		/// <summary>
		/// When the scene starts it creates all the needed objects that are required by default
		/// </summary>
		internal void OnLoad()
		{
			QPrefs.Load().Wait();
			CreatorQueue = new List<QObject>();
			DestroyQueue = new Queue<QBehavior>();
			SpriteRenderer = new QSpriteRenderer(Engine);
			GuiRenderer = new QGuiRenderer(Engine);
			Content = new QContentManager(Engine);
			GameObjects = new QGameObjectManager();
			World = new QWorldManager();
			List<IQLoad> Loaders = new List<IQLoad>();
			//Use this method to load textures before the scene starts to compile all of them
			//so that the megatexture only has to be created once per scene so that there
			//is no delay when objects spawn, but totally optional if you have a better system
			BehaviorScriptLoader(Loaders);
			foreach(var loader in Loaders)
			{
				((QBehavior)(loader)).SetName();
				((QBehavior)(loader)).Parent = QObject.NewObject();
				loader.OnLoad(new QAddContent(Content));
				QObject.DeleteObject(((QBehavior)(loader)).Parent);
			}
			Instantiate(Input = new QControls());
			Instantiate(Debug = new QDebug());
			Instantiate(Camera = new QCamera());
			Instantiate(Console = new QConsole());
			Instantiate(Coroutine = new QCoroutine());
			Instantiate(Accumulator = new QAccum());
			CheckQueue();
			DebugView = new QDebugView(World.world);
			DebugView.LoadContent(Engine.GraphicsDevice, Engine.Content);
			Load();
		}

		/// <summary>
		/// Checks for objects that have been added to Queue 
		/// and removes ones from that are in the remove queue
		/// </summary>
		void CheckQueue()
		{
			ObjectCreator();
			ObjectDestroyer();
		}

		/// <summary>
		/// uses variable time step for updates, and timestep for physics engine with interpolation
		/// </summary>
		/// <param name="time"></param>
		internal void OnUpdate(QTime time)
		{
			FrameTime = Stopwatch.StartNew();
			World.TryStep(time, GameObjects);
			CheckQueue();
			SpriteRenderer.Matrix = Camera.UpdateMatrix();
		}

		/// <summary>
		/// Renders all thge objects, and debug information
		/// </summary>
		/// <param name="renderer"></param>
		internal void OnDraw(QSpriteRenderer renderer)
		{
			renderer.Begin();
			QGameObjectManager.For(GameObjects.SpriteObjects, s => s.OnDrawSprite(renderer));
			renderer.End();
			//normally ends here, debug renders here, laggy af
			if(Debug.DebugLevel < 2) return;
			var c = Camera.Bounds;
			float ToSim(double v) => ConvertUnits.ToSimUnits(v);
			Matrix a = Matrix.CreateOrthographicOffCenter(ToSim(c.Left), ToSim(c.Right),
				ToSim(c.Bottom), ToSim(c.Top), 0, 1);
			DebugView.RenderDebugData(ref a);
		}

		/// <summary>
		/// Use this after OnDraw to draw things that must be on top but do not interact with the draw objects
		/// </summary>
		/// <param name="renderer"></param>
		internal void OnGui(QGuiRenderer renderer)
		{
			renderer.Begin();
			QGameObjectManager.For(GameObjects.GuiObjects, g => g.OnDrawGui(renderer));
			renderer.End();
			Debug.Lag = (float)FrameTime.Elapsed.TotalMilliseconds;
		}

		/// <summary>
		/// Unloads everything that was used in the scene
		/// </summary>
		internal void OnUnload()
		{
			QGameObjectManager.For(GameObjects.Objects, d => Destroy(d.Script));
			QGameObjectManager.For(GameObjects.UnloadObjects, u => u.OnUnload());
			World.Clear();
			Unload();
			Content.Unload();
			QPrefs.Save().Wait();
		}

		/*ctors*/

		protected QScene(string name)
		{
			Name = name;
		}
	}
}