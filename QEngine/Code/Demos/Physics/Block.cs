using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
    class Block : QCharacterController
    {
        QSprite sprite;

        QRigiBody body;

        public override void OnLoad(QAddContent add)
        {
            add.Rectangle(Name, 22, 22, QColor.YellowGreen);
        }

        public override void OnStart(QGetContent get)
        {
            int R() => QRandom.Range(1, 255);
            sprite = new QSprite(this, Name)
            {
                Color = new QColor(R(), R(), R())
            };
            body = World.CreateRectangle(this, 22, 22, 10);
            body.Restitution = 0.1f;
        }

        public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
        {
            spriteRenderer.Draw(sprite, Transform);
        }
    }
}