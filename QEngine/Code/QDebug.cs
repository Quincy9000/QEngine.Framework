namespace QEngine
{
    /// <summary>
    /// Debuger that displays fps
    /// </summary>
    public sealed class QDebug : QBehavior, IQLoad, IQStart, IQFixedUpdate, IQUpdate, IQDrawGui
    {
        public QFrameCounter fps { get; private set; }

        QFont font;

        QLabel label;

        /// <summary>
        /// Frames per second the screen updates
        /// </summary>
        public float Fps => fps.CurrentFramesPerSecond;

        public float TotalFrames => fps.TotalFrames;

        public float TotalSeconds => fps.TotalSeconds;

        public float Lag { get; set; }

        public int DebugLevel { get; set; }

        public void OnLoad(QAddContent add)
        {
            add.Font("Fonts/arial");
            DebugLevel = 0;
            fps = new QFrameCounter();
        }

        public void OnStart(QGetContent get)
        {
            font = get.Font("arial");
            Console.Label = new QLabel(font);
            label = new QLabel(font);
        }

        public void OnFixedUpdate(float time)
        {
            if(DebugLevel > 0)
            {
                label.Visible = true;
                label.Text = $"FrameDelay: {Lag}ms\nFPS: {Fps}\nTotalFrames: {TotalFrames}\nTime: {TotalSeconds} seconds";
                Transform.Position = new QVec(Window.Left, Window.Bottom - label.Measure(label.Text).Y);
            }
            else
                label.Visible = false;
            if(Input.IsKeyPressed(QKeys.F12))
            {
                DebugLevel++;
                if(DebugLevel == 3)
                    DebugLevel = 0;
            }
        }

        public void OnDrawGui(QGuiRenderer renderer)
        {
            renderer.DrawString(label, Transform);
        }

        public void OnUpdate(float delta)
        {
            fps.Update(delta);
        }

        public QDebug() : base("QDebug")
        {
        }
    }
}