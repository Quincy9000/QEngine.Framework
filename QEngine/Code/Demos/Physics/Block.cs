using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
    class Block : QCharacterController
    {
        QRigiBody body;

        public float Width { get; }

        public float Height { get; }
        
        public QColor Color { get; private set; }

        public Block(float w, float h)
        {
            Width = w;
            Height = h;
        }

        public override void OnStart(QGetContent get)
        {
            int R() => QRandom.Range(1, 255);
            body = World.CreateRectangle(this, Width, Height, 10);
            //body.FixedRotation = true;
            Color = new QColor(R(), R(), R());
            //body.Restitution = 1f;
        }

        public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
        {
            spriteRenderer.DrawRect(Transform, Width, Height, Color);
        }
    }
}