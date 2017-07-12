using System;

namespace QEngine.Physics.Shared.Optimization
{
    public interface IPoolable<T> : IDisposable where T : IPoolable<T>
    {
        void Reset();

        Pool<T> Pool { set; }
    }
}
