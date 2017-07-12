using QEngine.Physics.Dynamics;

namespace QEngine.Physics.Collision.Handlers
{
    public delegate void BroadphaseHandler(ref FixtureProxy proxyA, ref FixtureProxy proxyB);
}