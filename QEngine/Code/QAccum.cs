using System.Collections.Generic;
using System.Security.Policy;
using QPhysics.Collision.TOI;

namespace QEngine
{
	/// <summary>
	/// Limit Execution over a period of executeTime using CheckAccumGlobal
	/// </summary>
	public class QAccum : QBehavior
	{
		class AccumTimer
		{
			//actual executeTime that has passed
			public double accum;

			//if accum is greater than executeTime, accumulator can return true then reset
			public double ExecuteTime { get; }

			public AccumTimer(double t)
			{
				accum = 0;
				ExecuteTime = t;
			}
		}

		Dictionary<string, AccumTimer> Accumulators { get; } = new Dictionary<string, AccumTimer>();

		/// <summary>
		/// Returns true only after the amount of executeTime you choose has gone by, always uses real executeTime 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="executeTime"></param>
		/// <returns></returns>
		public bool CheckAccum(string name, double executeTime, QTime? time = null)
		{
			bool check = Accumulators.TryGetValue(name, out AccumTimer t);
			if(check && time != null)
				t.accum += time.Value.Delta;
			else if(!check)
			{
				Accumulators.Add(name, new AccumTimer(executeTime));
				return true;
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