namespace QEngine
{
	/// <summary>
	/// Just a wrapper for a float that we use for the physics state
	/// </summary>
	public struct QPhysicsState
	{
		float _value;

		public static implicit operator QPhysicsState(float f) => new QPhysicsState(f);
		public static implicit operator QPhysicsState(double f) => new QPhysicsState(f);
		
		public static implicit operator float(QPhysicsState f) => f._value;

		public QPhysicsState(float f)
		{
			_value = f;
		}		public QPhysicsState(double f)
		{
			_value = (float)f;
		}
	}
}