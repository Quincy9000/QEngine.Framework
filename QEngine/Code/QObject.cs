using System;
using System.Collections.Generic;

namespace QEngine
{
	public sealed class QObject : IQObject
	{
		public Guid Id { get; }

		public QTransform Transform { get; }

		public QScene Scene { get; internal set; }

		public QBehavior Script { get; internal set; }

		internal QObject(Guid uniqueId)
		{
			if(Guid.TryParse(uniqueId.ToString(), out Guid x))
			{
				Id = x;
			}
			Transform = new QTransform(this);
		}

		/*Statics*/

		internal static Stack<QObject> Pool { get; } = new Stack<QObject>();

		public static int PoolCount => Pool.Count;

		internal static QObject NewObject() => Pool.Count > 0 ? Pool.Pop() : new QObject(Guid.NewGuid());

		internal static void DeleteObject(QObject obj) => Pool.Push(obj);
	}
}