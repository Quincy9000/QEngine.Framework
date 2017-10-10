using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using QEngine.Physics.Debug;

namespace QEngine
{
	/// <summary>
	/// Base class for all scenes, inherit this to create a scene in qengine
	/// </summary>
	public class QWorld
	{
		/*Internal Variables*/

		public string Name { get; }

		internal QEngine Engine { get; set; }

		internal QContentManager Content { get; set; }

		public QSpriteRenderer SpriteRenderer { get; internal set; }

		public QGuiRenderer GuiRenderer { get; internal set; }

		internal QDebugView DebugView { get; set; }

		internal QEntityManager Entities { get; set; }

		/*Components*/

		internal QCamera Camera { get; private set; }

		internal QDebug Debug { get; private set; }

		internal QConsole Console { get; private set; }

		public QCoroutine Coroutine { get; private set; }

		public QAccumulator Accumulator { get; private set; }

		public QWindow Window { get; internal set; }

		public QPhysics Physics { get; internal set; }

		/*Privates*/

		Queue<QEntity> CreatorQueue { get; set; }

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
				QEntity[] temp = CreatorQueue.ToArray();
				//Add all the queue to the main array
				//QGameObjectManager.For(CreatorQueue, o => GameObjects.Add(o.Script));
				while(CreatorQueue.Count > 0)
					Entities.Add(CreatorQueue.Dequeue().Script);
				//reset the queue for objects that might get added from the OnStart() from the queued objects
				CreatorQueue = new Queue<QEntity>();
				//Set the flag to false again, Instantiate makes it flip, means we need to load more objects
				CreatorFlag = false;
				temp.For(o =>
				{
					if(o.Script is IQLoad l)
						l.OnLoad(new QLoadContent(Engine, Content));
				});
				//TODO check this
				Content.Atlases = QTextureAtlas.CreateAtlases(this);
				//Like if the textureAtlas is too big for one image, we create multiple atlases 
				//and then pretend they are all on one array so the user doesnt have to worry about them
				//OnStart can potentially set flag to true
				temp.For(o =>
				{
					if(o.Script is IQStart s)
						s.OnStart(new QGetContent(Content));
				});
			}
		}

		public void Instantiate(QBehavior script, QVector2 pos = default(QVector2), float rotation = 0)
		{
			script.Parent = QEntity.GetEntity();
			script.Parent.World = this;
			script.Parent.Script = script;
			script.IsDestroyed = false;
			script.Transform.Reset();
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
			Entities.Remove(script);
			script.Transform.Reset();
			QEntity.FreeEntity(script.Parent);
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
			CreatorQueue = new Queue<QEntity>();
			DestroyQueue = new Queue<QBehavior>();
			Window = new QWindow(Engine);
			SpriteRenderer = new QSpriteRenderer(Engine);
			GuiRenderer = new QGuiRenderer(Engine);
			Content = new QContentManager(Engine);
			Entities = new QEntityManager(Engine);
			Physics = new QPhysics();
			List<IQLoad> Loaders = new List<IQLoad>();
			//Use this method to load textures before the scene starts to compile all of them
			//so that the megatexture only has to be created once per scene so that there
			//is no delay when objects spawn, but totally optional if you have a better system
			BehaviorScriptLoader(Loaders);
			foreach(var loader in Loaders)
			{
				((QBehavior)loader).SetName();
				((QBehavior)loader).Parent = QEntity.GetEntity();
				loader.OnLoad(new QLoadContent(Engine, Content));
				QEntity.FreeEntity(((QBehavior)(loader)).Parent);
			}
			Accumulator = new QAccumulator();
			Coroutine = new QCoroutine();
			Instantiate(Console = new QConsole());
			Instantiate(Debug = new QDebug());
			Instantiate(Camera = new QCamera());
			CheckQueue();
			DebugView = new QDebugView(Physics.PhysicsWorld);
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
		internal void OnUpdate()
		{
			CheckQueue();
			Physics.TryStep(QTime.FixedDelta, Entities);
			Entities.UpdateObjects.For(u => u.OnUpdate());
			Entities.LateUpdateObjects.For(late => late.OnLateUpdate());
			SpriteRenderer.Matrix = Camera.TransformMatrix;
		}

		/// <summary>
		/// Renders all thge objects, and debug information
		/// </summary>
		/// <param name="renderer"></param>
		internal void OnDraw(QSpriteRenderer renderer)
		{
			renderer.Begin();
			Entities.SpriteObjects.For(s => s.OnDrawSprite(renderer));
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
			Entities.GuiObjects.For(g => g.OnDrawGui(renderer));
			renderer.End();
		}

		/// <summary>
		/// Unloads everything that was used in the scene
		/// </summary>
		internal void OnUnload()
		{
			Entities.Objects.For(d => Destroy(d.Script));
			Entities.UnloadObjects.For(un => un.OnUnload());
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
		protected QWorld(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Creates blank scene, do not use this ctor to make games.
		/// </summary>
		internal QWorld()
		{
			Name = "DemoScene";
		}
	}
}