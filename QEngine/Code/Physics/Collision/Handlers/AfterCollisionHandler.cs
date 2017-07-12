using QEngine.Physics.Collision.ContactSystem;
using QEngine.Physics.Dynamics;
using QEngine.Physics.Dynamics.Solver;

namespace QEngine.Physics.Collision.Handlers
{
    public delegate void AfterCollisionHandler(Fixture fixtureA, Fixture fixtureB, Contact contact, ContactVelocityConstraint impulse);
}