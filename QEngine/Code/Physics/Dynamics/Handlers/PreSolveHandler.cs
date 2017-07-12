using QEngine.Physics.Collision.ContactSystem;
using QEngine.Physics.Collision.Narrowphase;

namespace QEngine.Physics.Dynamics.Handlers
{
    public delegate void PreSolveHandler(Contact contact, ref Manifold oldManifold);
}