using QEngine.Physics.Collision.Shapes;
using QEngine.Physics.Shared;

namespace QEngine.Physics.Templates.Shapes
{
    public class PolygonShapeTemplate : ShapeTemplate
    {
        public PolygonShapeTemplate() : base(ShapeType.Polygon) { }

        public Vertices Vertices { get; set; }
    }
}