using System.Collections.Generic;

namespace QEngine
{
	/// <summary>
	/// Limit Execution over a period of executeTime using CheckAccumGlobal
	/// </summary>
	public class QAccumulator
	{
		class QAccumulatorTimer
		{
			//actual executeTime that has passed
			public float accum;

			//if accum is greater than executeTime, accumulator can return true then reset
			public float ExecuteTime { get; }

			public QAccumulatorTimer(double t)
			{
				accum = 0;
				ExecuteTime = (float)t;
			}
		}

		Dictionary<string, QAccumulatorTimer> Accumulators { get; } = new Dictionary<string, QAccumulatorTimer>();

		/// <summary>
		/// Returns true only after the amount of executeTime you choose has gone by, always uses real executeTime 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="executeTime"></param>
		/// <returns></returns>
		public bool CheckAccum(string name, double executeTime)
		{
			bool check = Accumulators.TryGetValue(name, out QAccumulatorTimer t);
			if(check)
				t.accum += QTime.Delta;
			else if(!check)
			{
				Accumulators.Add(name, new QAccumulatorTimer(executeTime));
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