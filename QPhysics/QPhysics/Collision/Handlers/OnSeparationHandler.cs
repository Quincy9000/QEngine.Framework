using QPhysics.Collision.ContactSystem;
using QPhysics.Dynamics;

namespace QPhysics.Collision.Handlers
{
    public delegate void OnSeparationHandler(Fixture fixtureA, Fixture fixtureB, Contact contact);
}