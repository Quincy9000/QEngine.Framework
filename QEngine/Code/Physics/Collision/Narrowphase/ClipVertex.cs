using Microsoft.Xna.Framework;
using QEngine.Physics.Collision.ContactSystem;

namespace QEngine.Physics.Collision.Narrowphase
{
    /// <summary>
    /// Used for computing contact manifolds.
    /// </summary>
    internal struct ClipVertex
    {
        public ContactID ID;
        public Vector2 V;
    }
}