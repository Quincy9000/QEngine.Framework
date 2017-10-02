namespace QEngine
{
	public enum QSortOrders
	{
		/// <summary>
		/// Starts from Zero, works way up to One
		/// </summary>
		StartAtZero,

		/// <summary>
		/// Starts from One and works down to Zero
		/// </summary>
		StartAtOne,

		/// <summary>
		/// Dont care what order the sprites are rendered in, more efficient but layer issues
		/// </summary>
		DontCare
	}
}