using QPhysics.Collision.ContactSystem;
using QPhysics.Dynamics;
using QPhysics.Dynamics.Solver;

namespace QPhysics.Collision.Handlers
{
    public delegate void AfterCollisionHandler(Fixture fixtureA, Fixture fixtureB, Contact contact, ContactVelocityConstraint impulse);
}