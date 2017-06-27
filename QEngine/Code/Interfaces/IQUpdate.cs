namespace QEngine
{
    public interface IQUpdate : IQObject
    {
        void OnUpdate(float time);
    }
}