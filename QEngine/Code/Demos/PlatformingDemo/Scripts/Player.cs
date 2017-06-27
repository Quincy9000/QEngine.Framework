using QEngine.Demos.Physics;
using QEngine.Demos.PlatformingDemo.Scripts.Enemies;
using QEngine.Prefabs;

namespace QEngine.Demos.PlatformingDemo.Scripts
{
    public class Player : QCharacterController
    {
        enum QDirections
        {
            Left,
            Right
        }

        enum PlayerStates
        {
            Jumping,
            Attacking,
            None
        }

        const float PlayerSpeed = 5;

        const float MaxPlayerVelocity = 7;

        const float MaxPlayerSprintVel = 8;

        const float JumpSpeed = 80;

        const float MaxJumpGas = 0.12f;

        const float MaxVel = 5;

        //distance before you cant walk into wall anymore
        const float WalkingIntoWallsDistance = 42;

        int _health;

        QAnimator Animator;

        QRigiBody Body;

        QMusic spaceJam;

        public int HealthMax = 5;

        bool IsTouchingFloor = true;

        bool CanMove = true;

        float JumpGas = MaxJumpGas;

        QRect LeftIdle, RightIdle;

        QDirections PlayerDirection;

        PlayerStates PlayerState;

        QSprite Sprite;

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                if(_health < 0)
                    _health = 0;
            }
        }

        public override void OnLoad(QAddContent add)
        {
            add.Texture(Assets.Bryan + "BryanSpriteSheet");
            add.Texture(Assets.Bryan + "SwordAttack2");
            add.Music("Audio/areYouReadyForThis");
        }

        public override void OnStart(QGetContent get)
        {
            Instantiate(new HealthBar());

            Health = HealthMax;

            Scene.SpriteRenderer.Filter = QFilteringState.Point;
            World.Gravity = new QVec(0, 20);
            var Frames = get.TextureSource("BryanSpriteSheet").Split(32, 32);
            var AttackFrames = get.TextureSource("SwordAttack2").Split(32, 32);
            Sprite = new QSprite(this, Frames[0]);
            Transform.Scale = new QVec(4);
            LeftIdle = Frames[2];
            RightIdle = Frames[0];
            spaceJam = get.Music("areYouReadyForThis");
            //spaceJam.Play();

            Body = World.CreateCapsule(this, Sprite.Height / 3f + 15, Sprite.Width / 6f, 100);
            //Body = World.CreateRectangle(this, Sprite.Width / 3f, Sprite.Height / 3f, 10);
            //Body = World.CreateRoundedRect(this, Sprite.Width /3f + 20, Sprite.Height / 1.2f, 10);

            //Body.FixedRotation = true;
            Body.Friction = 0.1f;
            Body.Restitution = 1f;

            Body.OnCollision += other =>
            {
                switch(QRigiBody.Direction(Body, other))
                {
                    case QCollisionDirection.Top:
                    {
                        if(other.IsStatic || other.Mass > 1)
                        {
                            IsTouchingFloor = true;
                            CanMove = true;
                        }
                        break;
                    }
                    case QCollisionDirection.Left:
                    {
                        if(other.Script is Bat b)
                        {
                            if(Accumulator.CheckAccum("CanDamage", 0.5f))
                            {
                                Body.body.ResetDynamics();
                                Health--;
                                Body.ApplyForce(new QVec(-3, -5) * 4000);
                                CanMove = false;
                            }
                        }
                        break;
                    }
                    case QCollisionDirection.Right:
                    {
                        if(other.Script is Bat b)
                        {
                            if(Accumulator.CheckAccum("CanDamage", 0.5f))
                            {
                                Body.body.ResetDynamics();
                                Health--;
                                Body.ApplyForce(new QVec(3, -5) * 4000);
                                CanMove = false;
                            }
                        }
                        break;
                    }
                }
            };

            Camera.Position = Transform.Position;

            Animator = new QAnimator();
            Animator.AddAnimation("Right", new QAnimation(Frames, 0.1, 4, 8));
            Animator.AddAnimation("Left", new QAnimation(Frames, 0.1, 12, 16));
            Animator.AddAnimation("RightAttack", new QAnimation(AttackFrames, 0.1, 0, 6));
            Animator.AddAnimation("LeftAttack", new QAnimation(AttackFrames, 0.1, 6, 12));
            Animator.Swap("Right");
            PlayerDirection = QDirections.Right;
            PlayerState = PlayerStates.None;
        }

        public override void OnFixedUpdate(float time)
        {
//            Body.Rotation += time * 20;
            if(Health == 0)
                Scene.ResetScene();
            var delta = time;
            var temp = QVec.Zero;
            var sprint = false;
            var right = true;
            var left = true;
            if(World.DidRaycastHit(Transform.Position + new QVec(0, Sprite.Height / 3f - 15), new QVec(0, 25)))
            {
                if(IsTouchingFloor)
                    JumpGas = MaxJumpGas;
            }
            if(World.DidRaycastHit(Transform.Position + new QVec(0, Sprite.Width / 3f - 5), new QVec(WalkingIntoWallsDistance, 0), out QRigiBody b))
            {
                if(b.Script is Platform)
                {
                    Transform.Position += QVec.Left * 2;
                    Sprite.Source = RightIdle;
                    right = false;
                }
            }
            if(World.DidRaycastHit(Transform.Position + new QVec(0, Sprite.Width / 3f - 5), new QVec(-WalkingIntoWallsDistance, 0), out QRigiBody bb))
            {
                if(bb.Script is Platform)
                {
                    Transform.Position += QVec.Right * 2;
                    Sprite.Source = LeftIdle;
                    left = false;
                }
            }
            if(Input.IsLeftMouseButtonHeld() && Accumulator.CheckAccum("Spawner", 0.03f))
            {
                int i = QRandom.Range(0, 1);
                switch(i)
                {
                    case 0:
                        Instantiate(new Block(), Camera.ScreenToWorld(Input.MousePosition()));
                        break;
                    case 1:
                        Instantiate(new Ball(), Camera.ScreenToWorld(Input.MousePosition()));
                        break;
                }
            }
            if(Input.IsMouseScrolledUp())
                Camera.Zoom += Camera.Zoom * 0.1f;
            if(Input.IsMouseScrolledDown())
                Camera.Zoom -= Camera.Zoom * 0.1f;
            if(Input.IsKeyPressed(QKeys.Escape))
                Scene.ResetScene();
            if(CanMove)
            {
                if(Input.IsKeyDown(QKeys.LeftShift) || Input.IsKeyDown(QKeys.RightShift))
                    sprint = true;
                if(Input.IsKeyDown(QKeys.W) || Input.IsKeyDown(QKeys.Space))
                {
                    if(JumpGas > 0)
                    {
                        Body.LinearVelocity += QVec.Up * JumpSpeed * delta;
                        JumpGas -= delta;
                        IsTouchingFloor = false;
                    }
                }
                if(Input.IsKeyDown(QKeys.A) && left)
                {
                    temp += QVec.Left;
                    Animator.Swap("Left");
                }
                if(Input.IsKeyDown(QKeys.D) && right)
                {
                    temp += QVec.Right;
                    Animator.Swap("Right");
                }
                if(temp != QVec.Zero)
                {
                    if(!sprint && Body.LinearVelocity.X < MaxPlayerVelocity && Body.LinearVelocity.X > -MaxPlayerVelocity)
                        Body.LinearVelocity = new QVec(temp.X * PlayerSpeed, Body.LinearVelocity.Y);
                    else
                        Body.LinearVelocity = new QVec(temp.X * PlayerSpeed * 2, Body.LinearVelocity.Y);
//                    Transform.Position += temp.Normalize() * PlayerSpeed * delta;
                    Animator.Play(Sprite, delta);
                }
                if(Input.IsKeyReleased(QKeys.A))
                {
                    Sprite.Source = LeftIdle;
                    Body.LinearVelocity = new QVec(-0.1f, Body.LinearVelocity.Y);
                    //Body.LinearVelocity += QVec.Left * time.Delta * 400;
                }
                if(Input.IsKeyReleased(QKeys.D))
                {
                    Sprite.Source = RightIdle;
                    Body.LinearVelocity = new QVec(0.1f, Body.LinearVelocity.Y);
                    //Body.LinearVelocity += QVec.Right * time.Delta * 400;
                }
            }
        }

        public override void OnLateUpdate(float delta)
        {
            var middle = QVec.Middle(Transform.Position, Camera.ScreenToWorld(Input.MousePosition()));
            Camera.Lerp(middle, 11, delta);
        }

        void LimitVel()
        {
            var lim = Body.LinearVelocity;
            Body.LinearVelocity = new QVec(lim.X > MaxVel ? MaxVel : lim.X, lim.Y > MaxVel ? MaxVel : lim.Y);
        }

        public override void OnDrawSprite(QSpriteRenderer spriteRenderer) { spriteRenderer.Draw(Sprite, Transform); }
    }
}