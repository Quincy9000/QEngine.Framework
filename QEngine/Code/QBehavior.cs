using System;

namespace QEngine
{
	public abstract class QBehavior
	{
		internal bool IsDestroyed { get; set; }

		internal event Action OnDestroy;

		internal void OnDestroyEvent()
		{
			OnDestroy?.Invoke();
		}

		/*Publics*/

		public Guid Id => Parent.Id;

		public QObject Parent { get; internal set; }

		public string Name { get; private set; }

		public void ExitGame() => Scene.Engine.Exit();

		public QControls Input => Scene.Input;

		public QScene Scene => Parent.Scene;

		public QTransform Transform => Parent.Transform;

		public QCamera Camera => Scene.Camera;

		public QConsole Console => Scene.Console;

		public QRect Window => Scene.Window;

		public QCoroutine Coroutine => Scene.Coroutine;

		public QDebug Debug => Scene.Debug;

		public QAccum Accumulator => Scene.Accumulator;

		public QWorldManager World => Scene.World;

		public T GetComponent<T>(string name) where T : QBehavior
		{
			return (T)(Scene.GameObjects.Objects.Find(u => u.Script.Name == name).Script);
		}

		public T GetComponent<T>(Guid id) where T : QBehavior
		{
			return (T)(Scene.GameObjects.Objects.Find(u => u.Script.Id == id).Script);
		}

		public void Instantiate(QBehavior b, QVec v = default(QVec)) => Scene.Instantiate(b, v);

		public QBehavior(string name)
		{
			Name = name;
		}

		internal void SetName()
		{
			if(string.IsNullOrEmpty(Name))
			{
				Name = GetType().Name;
			}
		}

		public QBehavior()
		{
			
		}
	}
}