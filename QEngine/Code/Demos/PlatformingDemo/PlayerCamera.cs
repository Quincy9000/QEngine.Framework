namespace QEngine.Demos.PlatformingDemo
{
	public class PlayerCamera : QBehavior, IQStart, IQLateUpdate
	{
		Player p;
		
		public void OnStart(QGetContent get)
		{
			p = GetComponent<Player>();
		}
		
		public void OnLateUpdate(QTime time)
		{
//			const float cameraSpeed = 5;
//			if(Camera.Bounds.Contains(p.Position))
//			{
//				if(QVec.Distance(Camera.Position, p.Position) > 100)
//					Camera.MoveTo(Position, cameraSpeed, time.Delta);
//			}
//			else
//			{
//				Camera.MoveTo(p.Position, 5000, time.Delta);
//			}
			Camera.Lerp(p.Position, 10, time.Delta);
		}
	}
}