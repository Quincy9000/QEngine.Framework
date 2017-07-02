using System.Collections.Generic;

namespace QEngine
{
	/// <summary>
	/// Limit Execution over a period of time using CheckAccum
	/// </summary>
	public class QAccum : QBehavior, IQUpdate
	{
		class AccumTimer
		{
			//actual time that has passed
			public float accum;

			//if accum is greater than executeTime, accumulator can return true then reset
			public float ExecuteTime { get; }

			public AccumTimer(float t)
			{
				accum = 0;
				ExecuteTime = t;
			}
		}

		Dictionary<string, AccumTimer> Accumulators { get; } = new Dictionary<string, AccumTimer>();

		public void OnUpdate(float delta)
		{
			foreach(var a in Accumulators)
				a.Value.accum += delta;
		}

		/// <summary>
		/// Returns true only after the amount of time you choose has gone by
		/// </summary>
		/// <param name="name"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public bool CheckAccum(string name, float time)
		{
			if(!Accumulators.TryGetValue(name, out AccumTimer t))
			{
				Accumulators.Add(name, new AccumTimer(time));
				return true; //return true if first time so that there is no delay 
			}
			if(t.accum > t.ExecuteTime)
			{
				t.accum = 0;
				return true;
			}
			return false;
		}
	}
}