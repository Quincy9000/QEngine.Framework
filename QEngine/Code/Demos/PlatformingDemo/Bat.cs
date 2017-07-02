using System;
using System.Collections.Generic;
using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo
{
    public class Bat : QCharacterController
    {
        List<QRect> Frames;

        QAnimation BatFlap;

        QVec spawnerPosition;

        Player player;

        QRigiBody body;

        QSprite Sprite;

        public int Speed = 300;

        public int Health = 3;

        public readonly int MaxHealth = 3;

        public bool CanTakeDamage { get; set; } = true;

        double damageAccum;

        public override void OnLoad(QAddContent add)
        {
            add.Texture(Assets.Bryan + "BryanStuff1");
        }

        public override void OnStart(QGetContent get)
        {
            spawnerPosition = Transform.Position;
            
            Health = MaxHealth;

            Frames = Scene.MegaTexture["BryanStuff1"].Split(32, 32);

            player = GetComponent<Player>("Player");

            Sprite = new QSprite(this, Frames[28]);
            Transform.Scale = new QVec(4);

            BatFlap = new QAnimation(Frames, 0.1f, 28, 30);

            body = World.CreateRectangle(this, 13 * Transform.Scale.X, 13 * Transform.Scale.Y);
            body.IgnoreGravity = true;
            body.FixedRotation = true;
            body.LinearDamping = 10f;
        }

        public override void OnUpdate(float time)
        {
            var s = Speed;
            if(Health < 1)
                Scene.Destroy(this);
            damageAccum += time;
            if(damageAccum > 0.5f)
            {
                CanTakeDamage = true;
                damageAccum = 0;
            }
            if(Health < MaxHealth)
            {
                s = Speed * 2;
            }
            if(Health == 1)
            {
                s = Speed * 3;
            }
            var distanceFromPlayer = QVec.Distance(player.Transform.Position, Transform.Position);
            //if the bat sees the player, it goes on the attacks
            if(distanceFromPlayer < 200)
            {
                if(player.Transform.Position.X > Transform.Position.X)
                    Sprite.Effect = QSpriteEffects.FlipHorizontally;
                else
                    Sprite.Effect = QSpriteEffects.None;
                Transform.Position += QVec.MoveTowards(Transform.Position, player.Transform.Position) * time * s;
                Sprite.Source = BatFlap.Play(time);
            }
            //only attacks from above
            else if(distanceFromPlayer < 800 && player.Transform.Position.Y > Transform.Position.Y)
            {
                if(player.Transform.Position.X > Transform.Position.X)
                    Sprite.Effect = QSpriteEffects.FlipHorizontally;
                else
                    Sprite.Effect = QSpriteEffects.None;
                Transform.Position += QVec.MoveTowards(Transform.Position, player.Transform.Position) * time * s;
                Sprite.Source = BatFlap.Play(time);
            }
            else
            {
                //runs away
                if(QVec.Distance(Transform.Position, spawnerPosition) > 1)
                {
                    if(spawnerPosition.X > Transform.Position.X)
                        Sprite.Effect = QSpriteEffects.FlipHorizontally;
                    else
                        Sprite.Effect = QSpriteEffects.None;
                    Transform.Position += QVec.MoveTowards(Transform.Position, spawnerPosition) * time * Speed;
                    Sprite.Source = BatFlap.Play(time);
                }
                else if(distanceFromPlayer > 1000)
                {
                    //closes eyes
                    Sprite.Effect = QSpriteEffects.FlipVertically;
                    Sprite.Source = Frames[30];
                }
                else if(distanceFromPlayer < 1000)
                {
                    //opens eyes
                    Sprite.Effect = QSpriteEffects.FlipVertically;
                    Sprite.Source = Frames[28];
                }
            }
        }

        public override void OnDrawSprite(QSpriteRenderer renderer)
        {
            //if(QVec.Distance(Transform.Position, Camera.Position) < Scene.Window.Width)
            renderer.Draw(Sprite, Transform);
        }

        public override void OnUnload()
        {
        }
    }
}