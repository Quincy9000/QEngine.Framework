using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using QEngine.Debug;
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

		public void ResetScene()
		{
			Engine.Manager.ResetScene();
		}

		public void ChangeScene(string s)
		{
			Engine.Manager.ChangeScene(s);
		}

		public QRect Window => Engine.GraphicsDevice.Viewport.Bounds;

		public void Instantiate(QBehavior script, QVec pos = default(QVec))
		{
			script.SetName();
			script.Parent = QObject.NewObject();
			script.Parent.Scene = this;
			script.Parent.Script = script;
			script.Transform.Reset();
			script.Transform.Position = pos;
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
			Instantiate(Input = new QControls());
			Instantiate(Debug = new QDebug());
			Instantiate(Camera = new QCamera());
			Instantiate(Console = new QConsole(40, 10));
			Instantiate(Coroutine = new QCoroutine());
			Instantiate(Accumulator = new QAccum());
			DebugView = new QDebugView(World.world);
			DebugView.LoadContent(Engine.GraphicsDevice, Engine.Content);
			Load();
		}

		internal void OnUpdate(QTime time)
		{
			FrameTime = Stopwatch.StartNew();
			ObjectCreator();
			ObjectDestroyer();
			World.TryStep(time, GameObjects);
			SpriteRenderer.Matrix = Camera.UpdateMatrix();
		}

		internal void OnDraw(QSpriteRenderer renderer)
		{
			renderer.Begin();
			QGameObjectManager.For(GameObjects.SpriteObjects, s => s.OnDrawSprite(renderer));
			renderer.End();
			//normally ends here
			if(Debug.DebugLevel < 2) return;
			var c = Camera;
			float ToSim(double v) => ConvertUnits.ToSimUnits(v);
			Matrix a = Matrix.CreateOrthographicOffCenter(ToSim(c.Bounds.Left), ToSim(c.Bounds.Right), ToSim(c.Bounds.Bottom), ToSim(c.Bounds.Top), -1, 1);
			DebugView.RenderDebugData(ref a);
		}

		internal void OnGui(QGuiRenderer renderer)
		{
			renderer.Begin();
			QGameObjectManager.For(GameObjects.GuiObjects, g => g.OnDrawGui(renderer));
			renderer.End();
			Debug.Lag = (float)FrameTime.Elapsed.TotalMilliseconds;
		}

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