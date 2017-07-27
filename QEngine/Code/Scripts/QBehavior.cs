using System;

namespace QEngine
{
	public abstract class QBehavior
	{
		/*Public*/

		public Guid Id => Parent.Id;

		public QObject Parent { get; internal set; }

		public string Name { get; private set; }

		public void ExitGame() => Scene.Engine.Exit();

		public QScene Scene => Parent.Scene;

		public QTransform Transform => Parent.Transform;

		public QCamera Camera => Scene.Camera;

		public QConsole Console => Scene.Console;

		public QRect Window => Scene.Window;

		public QCoroutine Coroutine => Scene.Coroutine;

		public QDebug Debug => Scene.Debug;

		public QAccum Accumulator => Scene.Accumulator;

		public QWorld World => Scene.World;

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
		
		/*GetComponents*/

		#region Components

		public T GetComponentFromScripts<T>(string name) where T : QBehavior
		{
			return (T)(Scene.GameObjects.Objects.Find(u => u.Script.Name == name).Script);
		}

		public T GetComponentFromScripts<T>() where T : QBehavior
		{
			return (T)(Scene.GameObjects.Objects.Find(u => u.Script is T).Script);
		}

		public T GetComponentFromScripts<T>(Guid id) where T : QBehavior
		{
			return (T)(Scene.GameObjects.Objects.Find(u => u.Script.Id == id).Script);
		}

//		public T GetComponentFromScene<T>(string name)
//		{
//			return Scene.
//		}

		#endregion

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