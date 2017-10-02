using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace QEngine
{
	public static class QTime
	{
		const int MaximumSamples = 100;

		static Queue<float> SampleBuffer { get; } = new Queue<float>();
		static Stopwatch LatencyTimer { get; } = Stopwatch.StartNew();
		
		/// <summary>
		/// Total frames that have passed since the scene started
		/// </summary>
		public static long TotalFrames { get; private set; } = 0;
		/// <summary>
		/// Total seconds that have passed since the scene started
		/// </summary>
		public static float TotalSeconds { get; private set; } = 0;
		
		public static float AverageFramesPerSecond { get; private set; } = 0;
		public static float CurrentFramesPerSecond { get; private set; } = 0;
		public static float Delta { get; private set; } = 0;
		public static float FixedDelta { get; set; } = (float)(1 / 60.0);
		public static float Latency { get; private set; } = 0;

		internal static void Update(GameTime gt)
		{
			Latency = (float)LatencyTimer.Elapsed.TotalMilliseconds;
			LatencyTimer.Restart();
			Delta = (float)gt.ElapsedGameTime.TotalSeconds;
			if(Math.Abs(Delta) < float.Epsilon)
				Delta = 0.1f;
			CurrentFramesPerSecond = 1.0f / Delta;

			SampleBuffer.Enqueue(CurrentFramesPerSecond);

			if(SampleBuffer.Count > MaximumSamples)
			{
				SampleBuffer.Dequeue();
				AverageFramesPerSecond = SampleBuffer.Average(i => i);
			}
			else
			{
				AverageFramesPerSecond = CurrentFramesPerSecond;
			}

			TotalFrames++;
			TotalSeconds += Delta;
		}
	}
}