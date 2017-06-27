using QEngine.Prefabs;

namespace QEngine.Demos.Physics
{
    class Ball : QCharacterController
    {
        QSprite sprite;

        QRigiBody body;

        public override void OnLoad(QAddContent add)
        {
            add.Circle(Name, 22, QColor.White);
        }

        public override void OnStart(QGetContent get)
        {
            int R() => QRandom.Range(1, 255);
            sprite = new QSprite(this, Name)
            {
                Color = new QColor(R(), R(), R())
            };
            body = World.CreateCircle(this, 10, 100);
            body.Restitution = 1f;
        }

        public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
        {
            spriteRenderer.Draw(sprite, Transform);
        }
    }
}