using QEngine.Physics.Collision.ContactSystem;
using QEngine.Physics.Dynamics.Solver;

namespace QEngine.Physics.Dynamics.Handlers
{
    public delegate void PostSolveHandler(Contact contact, ContactVelocityConstraint impulse);
}