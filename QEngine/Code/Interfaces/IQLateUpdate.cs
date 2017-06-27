namespace QEngine
{
    public interface IQLateUpdate : IQObject
    {
        void OnLateUpdate(float time);
    }
}