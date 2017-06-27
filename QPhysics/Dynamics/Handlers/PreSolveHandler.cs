using QPhysics.Collision.ContactSystem;
using QPhysics.Collision.Narrowphase;

namespace QPhysics.Dynamics.Handlers
{
    public delegate void PreSolveHandler(Contact contact, ref Manifold oldManifold);
}