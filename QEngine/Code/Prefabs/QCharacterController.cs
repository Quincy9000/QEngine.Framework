namespace QEngine.Prefabs
{
    public class QCharacterController 
        : QBehavior, IQLoad, IQStart, IQFixedUpdate, IQUpdate, IQLateUpdate, IQSprite, IQGui, IQDestroy, IQUnload
    {
        public virtual void OnLoad(QLoadContent load) { }

        public virtual void OnStart(QGetContent get) { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnUpdate() { }

        public virtual void OnLateUpdate() { }

        public virtual void OnDrawSprite(QSpriteRenderer render) { }

        public virtual void OnDrawGui(QGuiRenderer spriteRenderer) { }

        public virtual void OnDestroy() { }

        public virtual void OnUnload() { }

        public QCharacterController() { }
    }
}