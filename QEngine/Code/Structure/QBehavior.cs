using System;
using System.Reflection.Emit;

namespace QEngine
{
	public abstract class QBehavior
	{
		/*Public*/

		public string Name { get; }

		public Guid Id => Parent.Id;

		public QEntity Parent { get; internal set; }

		public QTransform Transform => Parent.Transform;

		public QWorld World => Parent.World;

		public QWindow Window => World.Window;

		public QAccumulator Accumulator => World.Accumulator;

		public QPhysics Physics => World.Physics;

		public QCamera Camera => World.Camera;

		public QDebug Debug => World.Debug;

		public QSpriteRenderer SpriteRenderer => World.SpriteRenderer;

		public QGuiRenderer GuiRenderer => World.GuiRenderer;

		public QCoroutine Coroutine => World.Coroutine;

		public QConsole Console => World.Console;

		/*Transforms*/

		public QVector2 Position
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
			return (T)(World.Entities.Objects.Find(u => u.Script.Name == name).Script);
		}

		/// <summary>
		/// Find another script that is in the same scene
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetBehavior<T>() where T : QBehavior
		{
			return (T)(World.Entities.Objects.Find(u => u.Script is T).Script);
		}

		/// <summary>
		/// Find another script that is in the same scene
		/// </summary>
		/// <param name="id"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetBehavior<T>(Guid id) where T : QBehavior
		{
			return (T)(World.Entities.Objects.Find(u => u.Script.Id == id).Script);
		}

		public void Instantiate(QBehavior b, QVector2 v = default(QVector2)) => World.Instantiate(b, v);

		/*Internals*/

		internal bool IsDestroyed { get; set; }

		internal event Action OnDestroyEvent;

		internal void DestroyEvent()
		{
			OnDestroyEvent?.Invoke();
		}

		internal void SetName()
		{

		}

		protected QBehavior(string name = "")
		{
			Name = name;
			if(string.IsNullOrEmpty(Name))
			{
				Name = GetType().FullName;
				//Parent?.Scene?.Console?.WriteLine(Name);
			}
		}
	}
}