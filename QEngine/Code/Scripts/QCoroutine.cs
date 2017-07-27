using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using QEngine.Prefabs;

namespace QEngine
{
	public class QCoroutine : QCharacterController
	{
		List<IEnumerator> routines { get; }

		internal QCoroutine()
		{
			routines = new List<IEnumerator>();
		}

		/// <summary>
		/// Fire and forget, coroutine function that uses realtime to iterate over a period
		/// </summary>
		/// <param name="routine"></param>
		internal void Start(IEnumerator routine)
		{
			routines.Add(routine);
		}

		internal void StopAll()
		{
			routines.Clear();
		}

		public override void OnUpdate(QTime time)
		{
			for(var i = 0; i < routines.Count; i++)
			{
				if(routines[i].Current is IEnumerator r)
					if(MoveNext(r))
						continue;
				if(!routines[i].MoveNext())
					routines.RemoveAt(i--);
			}
		}

		bool MoveNext(IEnumerator routine)
		{
			if(routine.Current is IEnumerator r)
				if(MoveNext(r))
					return true;
			return routine.MoveNext();
		}

		public static IEnumerator WaitForSeconds(double time)
		{
			var watch = Stopwatch.StartNew();
			while(watch.Elapsed.TotalSeconds < time)
				yield return 0;
		}

		internal int Count => routines.Count;

		internal bool Running => routines.Count > 0;
	}
}