namespace QEngine
{
	/// <summary>
	/// Debuger that displays fps
	/// </summary>
	public sealed class QDebug : QBehavior, IQLoad, IQStart, IQUpdate, IQDrawGui
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

		public float FrameTime { get; set; }

		public bool IsDebugMode { get; private set; }

		public void OnLoad(QAddContent add)
		{
			add.Font("Fonts/arial");
			IsDebugMode = false;
			fps = new QFrameCounter();
		}

		public void OnStart(QGetContent get)
		{
			font = get.Font("arial");
			Console.Label = new QLabel(font);
			label = new QLabel(font);
		}

		public void OnUpdate(float time)
		{
			if(IsDebugMode)
			{
				label.Visible = IsDebugMode;
				label.Text = $"FrameDelay: {FrameTime}ms\nFPS: {Fps}\nTotalFrames: {TotalFrames}\nTime: {TotalSeconds} seconds";
				Transform.Position = new QVec(Window.Left, Window.Bottom - label.Measure(label.Text).Y);
			}
			else
				label.Visible = IsDebugMode;
			if(Input.IsKeyPressed(QKeys.F12))
				IsDebugMode = !IsDebugMode;
			fps.Update(time);
		}

		public void OnDrawGui(QGuiRenderer renderer)
		{
			renderer.DrawString(label, Transform);
		}

		//		public void Update(float delta)
		//		{
		//			if(Input.IsKeyPressed(QKeys.F12))
		//				IsDebugMode = !IsDebugMode;
		//			fps.Update(delta);
		//			if(IsDebugMode)
		//				Console.WriteLine($"fps: {Fps}");
		//		}

		public QDebug() : base("QDebug") { }
	}
}