using System;
using System.Collections.Generic;
using System.Linq;

namespace QEngine
{
	/// <summary>
	/// Counts the frames and calculates Fps
	/// </summary>
	public class QFrameCounter
	{
		public long TotalFrames { get; private set; } = 0;
		public float TotalSeconds { get; private set; } = 0;
		public float AverageFramesPerSecond { get; private set; } = 0;
		public float CurrentFramesPerSecond { get; private set; } = 0;

		public const int MaximumSamples = 100;

		Queue<float> _sampleBuffer { get; } = new Queue<float>();

		public void Update(float deltaTime)
		{
			if(Math.Abs(deltaTime) < float.Epsilon)
				deltaTime = 0.1f;

			CurrentFramesPerSecond = 1.0f / deltaTime;

			_sampleBuffer.Enqueue(CurrentFramesPerSecond);

			if(_sampleBuffer.Count > MaximumSamples)
			{
				_sampleBuffer.Dequeue();
				AverageFramesPerSecond = _sampleBuffer.Average(i => i);
			}
			else
			{
				AverageFramesPerSecond = CurrentFramesPerSecond;
			}

			TotalFrames++;
			TotalSeconds += deltaTime;
		}
	}
}