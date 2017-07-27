namespace QEngine.Demos.PlatformingDemo
{
	public class PlayerCamera : QBehavior, IQStart, IQLateUpdate
	{
		Player p;
		
		public void OnStart(QGetContent get)
		{
			p = GetComponentFromScripts<Player>();
		}
		
		public void OnLateUpdate(QTime time)
		{
			const float cameraSpeed = 5;
			if(Camera.Bounds.Contains(p.Position))
			{
				Camera.Lerp(p.Position, cameraSpeed, time.Delta);
			}
			else
			{
				Camera.Lerp(p.Position, 20, time.Delta);
			}
		}
	}
}