using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenGL;

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

		/*Privates*/

		List<QObject> ObjectQueue { get; set; }

		List<QBehavior> DestroyQueue { get; set; }

		bool CreatorFlag = false;

		public void ResetScene()
		{
			Engine.Manager.ResetScene();
		}

		public void ChangeScene(string s)
		{
			Engine.Manager.ChangeScene(s);
		}

		public QRect Window => Engine.GraphicsDevice.Viewport.Bounds;

		/*Public Methods*/

		public void Instantiate(QBehavior script, QVec pos = default(QVec))
		{
			script.SetName();
			script.Parent = QObject.NewObject();
			script.Parent.Scene = this;
			script.Parent.Script = script;
			script.Transform.Reset();
			script.Transform.Position = pos;
			ObjectQueue.Add(script.Parent);
			CreatorFlag = true;
		}

		public void AddToDestroyList(QBehavior script)
		{
			if(!DestroyQueue.Contains(script))
				DestroyQueue.Add(script);
		}

		/*Private Methods*/

		void Destroy(QBehavior script)
		{
			GameObjects.Remove(script);
			DestroyQueue.Remove(script);
			QObject.DeleteObject(script.Parent);
		}

		/*Protected Virtuals*/

		protected virtual void Load() { }

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
				QGameObjectManager.For(ObjectQueue, o => GameObjects.Add(o.Script));
				//Then make temp array to store the queue
				var temp = ObjectQueue.ToArray();
				//reset the queue for objects that might get added from the OnStart() from the queued objects
				ObjectQueue = new List<QObject>();
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

		internal void ObjectDestroyer()
		{
			for(var i = 0; i < DestroyQueue.Count; i++)
				Destroy(DestroyQueue[i]);
		}

		internal void OnLoad()
		{
			QPrefs.Load().Wait();
			DestroyQueue = new List<QBehavior>();
			ObjectQueue = new List<QObject>();
			SpriteRenderer = new QSpriteRenderer(Engine);
			GuiRenderer = new QGuiRenderer(Engine);
			GameObjects = new QGameObjectManager();
			Content = new QContentManager(Engine);
			World = new QWorldManager();
			Instantiate(Input = new QControls());
			Instantiate(Debug = new QDebug());
			Instantiate(Camera = new QCamera());
			Instantiate(Console = new QConsole(40, 10));
			Instantiate(Coroutine = new QCoroutine());
			Instantiate(Accumulator = new QAccum());
			Load();
		}

		internal void OnUpdate(QTime time)
		{
			FrameTime = Stopwatch.StartNew();
			ObjectCreator();
			ObjectDestroyer();
			Accumulator.Physics += time.Delta;
			const float simulation = 1 / 60f;
			while(Accumulator.Physics >= simulation)
			{
				World.Step(simulation);
				Accumulator.Physics -= simulation;
			}
			QGameObjectManager.For(GameObjects.UpdateObjects, u => u.OnUpdate(time.Delta));
		}

		internal void OnDraw(QSpriteRenderer renderer)
		{
			SpriteRenderer.Matrix = Camera.UpdateMatrix();
			renderer.Begin();
			QGameObjectManager.For(GameObjects.SpriteObjects, s => s.OnDrawSprite(renderer));
			renderer.End();
		}

		internal void OnGui(QGuiRenderer renderer)
		{
			renderer.Begin();
			QGameObjectManager.For(GameObjects.GuiObjects, g => g.OnDrawGui(renderer));
			renderer.End();
			Debug.FrameTime = (float)FrameTime.Elapsed.TotalMilliseconds;
		}

		internal void OnUnload()
		{
			QGameObjectManager.For(GameObjects.Objects, d => AddToDestroyList(d.Script));
			QGameObjectManager.For(GameObjects.UnloadObjects, u => u.OnUnload());
			World.Clear();
			Unload();
			Content.Unload();
			QPrefs.Save().Wait();
		}

		/*ctors*/

		public QScene(string name)
		{
			Name = name;
		}
	}
}