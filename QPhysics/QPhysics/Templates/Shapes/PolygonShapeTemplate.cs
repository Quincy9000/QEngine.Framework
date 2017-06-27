using QPhysics.Collision.Shapes;
using QPhysics.Shared;

namespace QPhysics.Templates.Shapes
{
    public class PolygonShapeTemplate : ShapeTemplate
    {
        public PolygonShapeTemplate() : base(ShapeType.Polygon) { }

        public Vertices Vertices { get; set; }
    }
}