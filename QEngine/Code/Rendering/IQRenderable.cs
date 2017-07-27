namespace QEngine
{
	public interface IQRenderable
	{
		QBehavior Script { get; }
		
		QVec Offset { get; set; }

		QVec Origin { get; set; }

		QRect Source { get; set; }

		QColor Color { get; set; }

		QVec Scale { get; set; }

		float Layer { get; set; }

		bool Visible { get; set; }

		QRenderEffects Effect { get; set; }
	}
}