using System;
using System.Collections.Generic;

namespace QEngine
{
	public class QEntity
	{
		public Guid Id { get; }

		public QTransform Transform { get; }

		public QWorld World { get; internal set; }

		public QBehavior Script { get; internal set; }

		internal QEntity()
		{
			Id = Guid.NewGuid();
			Transform = new QTransform();
		}

		/*Statics*/

		public static int TotalEntities => Pool.Count;

		internal const int StartingAllocatedPoolSize = 1000;

		internal static QEntity GetEntity() => Pool.Count > 0 ? Pool.Pop() : new QEntity();

		internal static void FreeEntity(QEntity obj) => Pool.Push(obj);

		static Stack<QEntity> Pool { get; } = new Stack<QEntity>(StartingAllocatedPoolSize);
	}
}