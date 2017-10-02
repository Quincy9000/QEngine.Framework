using System.Collections.Generic;

namespace QEngine
{
	class QEntityManager
	{
		internal List<QEntity> Objects { get; }

		internal List<IQLoad> LoadObjects { get; set; }

		internal List<IQStart> StartObjects { get; set; }

		internal List<IQFixedUpdate> FixedUpdateObjects { get; set; }

		internal List<IQUpdate> UpdateObjects { get; set; }

		internal List<IQLateUpdate> LateUpdateObjects { get; set; }

		internal List<IQSprite> SpriteObjects { get; set; }

		internal List<IQGui> GuiObjects { get; set; }

		internal List<IQDestroy> DestroyObjects { get; set; }

		internal List<IQUnload> UnloadObjects { get; set; }

		public void Add(QBehavior script)
		{
			Objects.Add(script.Parent);
			if(script is IQLoad l)
			{
				LoadObjects.Add(l);
			}
			if(script is IQStart s)
			{
				StartObjects.Add(s);
			}
			if(script is IQFixedUpdate u)
			{
				FixedUpdateObjects.Add(u);
			}
			if(script is IQUpdate up)
			{
				UpdateObjects.Add(up);
			}
			if(script is IQLateUpdate late)
			{
				LateUpdateObjects.Add(late);
			}
			if(script is IQSprite ds)
			{
				SpriteObjects.Add(ds);
			}
			if(script is IQGui dg)
			{
				GuiObjects.Add(dg);
			}
			if(script is IQDestroy des)
			{
				DestroyObjects.Add(des);
			}
			if(script is IQUnload un)
			{
				UnloadObjects.Add(un);
			}
		}

		public void Remove(QBehavior script)
		{
			Objects.Remove(script.Parent);
			if(script is IQLoad l)
			{
				LoadObjects.Remove(l);
			}
			if(script is IQStart s)
			{
				StartObjects.Remove(s);
			}
			if(script is IQFixedUpdate u)
			{
				FixedUpdateObjects.Remove(u);
			}
			if(script is IQUpdate up)
			{
				UpdateObjects.Remove(up);
			}
			if(script is IQLateUpdate late)
			{
				LateUpdateObjects.Remove(late);
			}
			if(script is IQSprite ds)
			{
				SpriteObjects.Remove(ds);
			}
			if(script is IQGui dg)
			{
				GuiObjects.Remove(dg);
			}
			if(script is IQDestroy des)
			{
				des.OnDestroy();
				DestroyObjects.Remove(des);
			}
			if(script is IQUnload un)
			{
				UnloadObjects.Remove(un);
			}
			//invokes event even if does not inherit OnDestroy
			script.DestroyEvent();
		}

//		public QObject this[Type t, int i]
//		{
//			get
//			{
//				switch(t)
//				{
//					case null:
//						return null;
//					case IQLoad l:
//						return (QObject)LoadObjects[i];
//					case IQStart s:
//						return (QObject)StartObjects[i];
//					case IQFixedUpdate u:
//						return (QObject)FixedUpdateObjects[i];
//					case IQDrawSprite ds:
//						return (QObject)SpriteObjects[i];
//					case IQDestroy d:
//						return (QObject)DestroyObjects[i];
//					case IQUnload un:
//						return (QObject)UnloadObjects[i];
//					default:
//						return null;
//				}
//			}
//		}

		public QEntityManager(QEngine e)
		{
			Objects = new List<QEntity>();
			LoadObjects = new List<IQLoad>();
			StartObjects = new List<IQStart>();
			FixedUpdateObjects = new List<IQFixedUpdate>();
			UpdateObjects = new List<IQUpdate>();
			LateUpdateObjects = new List<IQLateUpdate>();
			SpriteObjects = new List<IQSprite>();
			GuiObjects = new List<IQGui>();
			DestroyObjects = new List<IQDestroy>();
			UnloadObjects = new List<IQUnload>();
		}
	}
}