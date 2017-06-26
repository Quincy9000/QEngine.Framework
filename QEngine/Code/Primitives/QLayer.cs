namespace QEngine
{
	/// <summary>
	/// Keeps the layer of the sprite hidden but can use any value to make it easier to use, its just a QTime
	/// </summary>
	public struct QLayer
	{
		internal float value { get; }

		internal float condensedValue { get; }

		/// <summary>
		/// number used to lower the number the user enters into all layers
		/// </summary>
		static int Divider = 10000;

		public static implicit operator QLayer(float f)
		{
			return new QLayer(f);
		}

		public static implicit operator float(QLayer f)
		{
			return f.value;
		}

		public QLayer(float f)
		{
			value = f;
			condensedValue = value / Divider;
		}

		public QLayer(int i)
		{
			value = i;
			condensedValue = value / Divider;
		}
	}
}