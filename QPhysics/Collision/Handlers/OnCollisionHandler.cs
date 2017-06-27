using QPhysics.Collision.ContactSystem;
using QPhysics.Dynamics;

namespace QPhysics.Collision.Handlers
{
    public delegate void OnCollisionHandler(Fixture fixtureA, Fixture fixtureB, Contact contact);
}