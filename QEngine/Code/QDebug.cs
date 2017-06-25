namespace QEngine
{
	/// <summary>
	/// Debuger that displays fps
	/// </summary>
	public sealed class QDebug : QBehavior, IQLoad, IQStart, IQUpdate
	{
		public QFrameCounter fps { get; private set; }

		QFont font;

		/// <summary>
		/// Frames per second the screen updates
		/// </summary>
		public float Fps => fps.CurrentFramesPerSecond;

		public float TotalFrames => fps.TotalFrames;

		public float TotalSeconds => fps.TotalSeconds;

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
		}

		public void OnUpdate(QTime time)
		{
			if(Input.IsKeyPressed(QKeys.F12))
				IsDebugMode = !IsDebugMode;
			fps.Update(time.Delta);
			if(IsDebugMode)
			{
				Console.WriteLine($"fps: {Fps}");
			}
		}

		public QDebug() : base("QDebug") { }
	}
}