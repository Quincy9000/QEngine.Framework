using Microsoft.Xna.Framework;
using QPhysics.Collision.ContactSystem;

namespace QPhysics.Collision.Narrowphase
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