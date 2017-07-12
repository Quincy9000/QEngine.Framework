using QEngine.Physics.Collision.ContactSystem;
using QEngine.Physics.Dynamics;

namespace QEngine.Physics.Collision.Handlers
{
    public delegate void OnCollisionHandler(Fixture fixtureA, Fixture fixtureB, Contact contact);
}