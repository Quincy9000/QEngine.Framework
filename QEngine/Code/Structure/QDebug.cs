namespace QEngine
{
	/// <summary>
	/// Debugger to display all kinds of information
	/// </summary>
	public sealed class QDebug : QBehavior, IQLoad, IQStart, IQLateUpdate, IQGui
	{
		public QFont Font { get; private set; }

		public QLabel Label { get; set; }

		public int DebugLevel { get; set; }

		public void Append(string msg)
		{
			Label.Append(msg);
		}

		public void AppendLine(string msg)
		{
			Label.AppendLine(msg);
		}

		public void OnLoad(QLoadContent load)
		{
			load.Font("Fonts/arial");
			DebugLevel = 0;
		}

		public void OnStart(QGetContent get)
		{
			Font = get.Font("arial");
			Label = new QLabel(Font);
			Label.Visible = false;
		}

		public void OnLateUpdate()
		{
			if(DebugLevel > 0)
			{
				Label.Visible = true;
				Label.Append($"FrameDelay: {QTime.Latency}ms\nFPS: {QTime.CurrentFramesPerSecond}\n" +
				             $"TotalFrames: {QTime.TotalFrames}\nTime: {QTime.TotalSeconds} seconds");
				Transform.Position =
					new QVector2(Window.Bounds.Left, Window.Bounds.Bottom - Label.Measure(Label.Text).Y);
			}
			else
				Label.Visible = false;
			if(QInput.Pressed(QKeyStates.F12))
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