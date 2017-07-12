using QEngine.Physics.Collision.ContactSystem;

namespace QEngine.Physics.Collision.Handlers
{
    /// <summary>
    /// This delegate is called when a contact is created
    /// </summary>
    public delegate bool BeginContactHandler(Contact contact);
}