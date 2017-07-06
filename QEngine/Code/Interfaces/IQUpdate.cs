namespace QEngine
{
    public interface IQUpdate : IQObject
    {
        void OnUpdate(QTime time);
    }
}