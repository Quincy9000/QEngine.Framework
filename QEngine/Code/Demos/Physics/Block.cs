using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
    class Block : QCharacterController
    {
        QRigiBody body;

        public float Width { get; }

        public float Height { get; }

        QSprite sprite;

        public Block(int w, int h)
        {
            Width = w;
            Height = h;
        }

        public override void OnLoad(QAddContent add)
        {
            add.Rectangle(Name, (int)Width, (int)Height, QColor.White);
        }

        public override void OnStart(QGetContent get)
        {
            body = World.CreateRectangle(this, Width, Height);
            body.IsCCD = true;
            sprite = new QSprite(this, get.TextureSource(Name));
            int R() => QRandom.Number(1, 255);
            sprite.Color = new QColor(R(), R(), R());
            //body.FixedRotation = true;
            //body.Restitution = 1f;
        }

        public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
        {
            spriteRenderer.Draw(sprite, Transform);
        }
    }
}