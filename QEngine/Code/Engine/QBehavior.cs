using System;

namespace QEngine
{
	public abstract class QBehavior
	{
		/*Public*/

		public string Name { get; private set; }

		public QObject Parent { get; internal set; }

		public Guid Id => Parent.Id;

		public QScene Scene => Parent.Scene;

		public QTransform Transform => Parent.Transform;

		public QWindow Window => Scene.Window;

		public QAccum Accumulator => Scene.Accumulator;

		public QPhysics Physics => Scene.Physics;

		public QCamera Camera => Scene.Camera;

		public QDebug Debug => Scene.Debug;

		public QSpriteRenderer SpriteRenderer => Scene.SpriteRenderer;

		public QGuiRenderer GuiRenderer => Scene.GuiRenderer;

		public QCoroutine Coroutine => Scene.Coroutine;

		public QConsole Console => Scene.Console;

		/*Transforms*/

		public QVec Position
		{
			get => Transform.Position;
			set => Transform.Position = value;
		}

		public float Rotation
		{
			get => Transform.Rotation;
			set => Transform.Rotation = value;
		}

		/// <summary>
		/// Find another script that is in the same scene
		/// </summary>
		/// <param name="name"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetBehavior<T>(string name) where T : QBehavior
		{
			return (T)(Scene.GameObjects.Objects.Find(u => u.Script.Name == name).Script);
		}

		/// <summary>
		/// Find another script that is in the same scene
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetBehavior<T>() where T : QBehavior
		{
			return (T)(Scene.GameObjects.Objects.Find(u => u.Script is T).Script);
		}

		/// <summary>
		/// Find another script that is in the same scene
		/// </summary>
		/// <param name="id"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetBehavior<T>(Guid id) where T : QBehavior
		{
			return (T)(Scene.GameObjects.Objects.Find(u => u.Script.Id == id).Script);
		}

		public void Instantiate(QBehavior b, QVec v = default(QVec)) => Scene.Instantiate(b, v);

		/*Internals*/

		internal bool IsDestroyed { get; set; }

		internal event Action OnDestroyEvent;

		internal void DestroyEvent()
		{
			OnDestroyEvent?.Invoke();
		}

		internal void SetName()
		{
			if(string.IsNullOrEmpty(Name))
			{
				Name = GetType().Name;
				//Parent?.Scene?.Console?.WriteLine(Name);
			}
		}

		/*Protected*/

		protected QBehavior(string name)
		{
			Name = name;
		}

		protected QBehavior() { }
	}
}