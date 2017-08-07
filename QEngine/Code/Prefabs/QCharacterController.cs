﻿namespace QEngine.Prefabs
{
    public class QCharacterController : QBehavior, IQLoad, IQStart, IQFixedUpdate, IQUpdate, IQLateUpdate, IQDrawSprite, IQDrawGui, IQDestroy, IQUnload
    {
        public virtual void OnLoad(QLoadContent load) { }

        public virtual void OnStart(QRetrieveContent retrieve) { }

        public virtual void OnFixedUpdate(float delta) { }

        public virtual void OnUpdate(QTime delta) { }

        public virtual void OnLateUpdate(QTime delta) { }

        public virtual void OnDrawSprite(QSpriteRenderer spriteRenderer) { }

        public virtual void OnDrawGui(QGuiRenderer spriteRenderer) { }

        public virtual void OnDestroy() { }

        public virtual void OnUnload() { }

        public QCharacterController(string name) : base(name) { }

        public QCharacterController() { }
    }
}