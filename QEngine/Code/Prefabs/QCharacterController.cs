﻿namespace QEngine
{
	public class QCharacterController : QBehavior, IQLoad, IQStart, IQUpdate, IQDrawSprite, IQDrawGui, IQDestroy, IQUnload
	{
		public virtual void OnLoad(QAddContent add) { }

		public virtual void OnStart(QGetContent get) { }

		public virtual void OnUpdate(float delta) { }

		public virtual void OnDrawSprite(QSpriteRenderer spriteRenderer) { }

		public virtual void OnDrawGui(QGuiRenderer spriteRenderer) { }

		public virtual void OnDestroy() { }

		public virtual void OnUnload() { }

		public QCharacterController(string name) : base(name) { }

		public QCharacterController() { }
	}
}