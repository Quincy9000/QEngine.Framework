using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using QEngine.Physics.Debug;

namespace QEngine
{
	/// <summary>
	/// Base class for all scenes, inherit this to create a scene in qengine
	/// </summary>
	public class QScene : IQScene
	{
		/*Internal Variables*/

		public string Name { get; }

		internal QContentManager Content { get; set; }

		internal QController Controller { get; private set; }

		internal QCamera Camera { get; private set; }

		internal QDebug Debug { get; private set; }

		internal QConsole Console { get; private set; }

		internal QDebugView DebugView { get; set; }

		internal Stopwatch FrameTime { get; set; }

		internal QGameObjectManager GameObjects { get; set; }

		internal QEngine Engine { get; set; }

		/*Components*/

		public QSpriteRenderer SpriteRenderer { get; internal set; }

		public QGuiRenderer GuiRenderer { get; internal set; }

		public QCoroutine Coroutine { get; internal set; }

		public QAccum Accumulator { get; internal set; }

		public QWindow Window { get; internal set; }

		public QPhysics Physics { get; internal set; }

		public QAtlas Atlas { get; internal set; }

		/*Privates*/

		Queue<QObject> CreatorQueue { get; set; }

		Queue<QBehavior> DestroyQueue { get; set; }

		bool CreatorFlag { get; set; }

		/*Public Methods*/

		public void ResetScene()
		{
			Engine.Manager.CallToResetScene = true;
		}

		public void ChangeScene(string s)
		{
			Engine.Manager.CallToChangeScene = true;
			Engine.Manager.CallToChangeSceneName = s;
		}

		public void ExitGame()
		{
			Engine.Manager.CallToExitGame = true;
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
				//make temp array to store the queue
				var temp = CreatorQueue.ToArray();
				//Add all the queue to the main array
				//QGameObjectManager.For(CreatorQueue, o => GameObjects.Add(o.Script));
				while(CreatorQueue.Count > 0)
					GameObjects.Add(CreatorQueue.Dequeue().Script);
				//reset the queue for objects that might get added from the OnStart() from the queued objects
				CreatorQueue = new Queue<QObject>();
				//Set the flag to false again, Instantiate makes it flip, means we need to load more objects
				CreatorFlag = false;
				QGameObjectManager.For(temp, o =>
				{
					if(o.Script is IQLoad l)
						l.OnLoad(new QLoadContent(Content));
				});
				Atlas = Content.Atlas;
				//OnStart can potentially set flag to true
				QGameObjectManager.For(temp, o =>
				{
					if(o.Script is IQStart s)
						s.OnStart(new QRetrieveContent(Content));
				});
			}
		}

		public void Instantiate(QBehavior script, QVec pos = default(QVec), float rotation = 0)
		{
			script.Parent = QObject.NewObject();
			script.Parent.Scene = this;
			script.Parent.Script = script;
			script.IsDestroyed = false;
			script.Transform.Reset(pos, rotation);
			script.SetName();
			CreatorQueue.Enqueue(script.Parent);
			CreatorFlag = true;
		}

		/// <summary>
		/// Destroys script
		/// </summary>
		/// <param name="script"></param>
		public void Destroy(QBehavior script)
		{
			script.IsDestroyed = true;
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
			script.Transform.Reset();
			QObject.DeleteObject(script.Parent);
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
			CreatorQueue = new Queue<QObject>();
			DestroyQueue = new Queue<QBehavior>();
			Window = new QWindow(Engine);
			SpriteRenderer = new QSpriteRenderer(Engine);
			GuiRenderer = new QGuiRenderer(Engine);
			Content = new QContentManager(Engine);
			GameObjects = new QGameObjectManager(Engine);
			Physics = new QPhysics();
			List<IQLoad> Loaders = new List<IQLoad>();
			//Use this method to load textures before the scene starts to compile all of them
			//so that the megatexture only has to be created once per scene so that there
			//is no delay when objects spawn, but totally optional if you have a better system
			BehaviorScriptLoader(Loaders);
			foreach(var loader in Loaders)
			{
				((QBehavior)loader).SetName();
				((QBehavior)loader).Parent = QObject.NewObject();
				loader.OnLoad(new QLoadContent(Content));
				QObject.DeleteObject(((QBehavior)(loader)).Parent);
			}
			Instantiate(Console = new QConsole());
			Instantiate(Controller = new QController());
			Instantiate(Debug = new QDebug());
			Instantiate(Camera = new QCamera());
			Instantiate(Coroutine = new QCoroutine());
			Instantiate(Accumulator = new QAccum());
			CheckQueue();
			DebugView = new QDebugView(Physics.world);
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
			CheckQueue();
			Physics.TryStep(time, GameObjects);
			SpriteRenderer.Matrix = Camera.TransformMatrix;
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
			Matrix a = Matrix.CreateOrthographicOffCenter(c.Left.ToSim(), c.Right.ToSim(),
				c.Bottom.ToSim(), c.Top.ToSim(), 0, 1);
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
			Physics.Clear();
			Unload();
			Content.Unload();
			QPrefs.Save().Wait();
		}

		/*ctors*/

		/// <summary>
		/// Inherit QScene to make games and add your behavior and scripts.
		/// </summary>
		/// <param name="name"></param>
		protected QScene(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Creates blank scene, do not use this ctor to make games.
		/// </summary>
		internal QScene()
		{
			Name = "DemoScene";
		}
	}
}