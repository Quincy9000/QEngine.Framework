namespace QEngine
{
    /// <summary>
    /// Debuger that displays Fps
    /// </summary>
    public sealed class QDebug : QBehavior, IQLoad, IQStart, IQLateUpdate, IQDrawGui
    {
        public QFrameCounter Fps { get; private set; }

        public QFont Font { get; private set; }

        public QLabel Label { get; private set; }

        public float FramesPerSecond => Fps.CurrentFramesPerSecond;

        public float TotalFrames => Fps.TotalFrames;

        public float TotalSeconds => Fps.TotalSeconds;

        public float Lag { get; internal set; }

        public int DebugLevel { get; set; }

        public void AppendText(string msg)
        {
            Label.Text += msg + "\n";
        }

        public void OnLoad(QAddContent add)
        {
            add.Font("Fonts/arial");
            DebugLevel = 0;
            Fps = new QFrameCounter();
        }

        public void OnStart(QGetContent get)
        {
            Font = get.Font("arial");
            Console.Label = new QLabel(Font);
            Label = new QLabel(Font);
        }
        
        public void OnLateUpdate(float delta)
        {
            Fps.Update(delta);
            if(Accumulator.CheckAccum("Debugger", 1 / 10f))
            {
                if(DebugLevel > 0)
                {
                    Label.Visible = true;
                    AppendText($"FrameDelay: {Lag}ms\nFPS: {FramesPerSecond}\nTotalFrames: {TotalFrames}\nTime: {TotalSeconds} seconds");
                    Transform.Position = new QVec(Window.Left, Window.Bottom - Label.Measure(Label.Text).Y);
                }
                else
                    Label.Visible = false;
            }
            if(Input.IsKeyPressed(QKeys.F12))
            {
                DebugLevel++;
                if(DebugLevel == 3)
                    DebugLevel = 0;
            }
        }

        public void OnDrawGui(QGuiRenderer renderer)
        {
            renderer.DrawString(Label, Transform);
            Label.Text = "";
        }
    }
}