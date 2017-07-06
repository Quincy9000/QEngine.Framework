namespace QEngine
{
    public interface IQLateUpdate : IQObject
    {
        void OnLateUpdate(QTime time);
    }
}