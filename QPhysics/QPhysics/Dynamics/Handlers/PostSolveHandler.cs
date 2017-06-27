using QPhysics.Collision.ContactSystem;
using QPhysics.Dynamics.Solver;

namespace QPhysics.Dynamics.Handlers
{
    public delegate void PostSolveHandler(Contact contact, ContactVelocityConstraint impulse);
}