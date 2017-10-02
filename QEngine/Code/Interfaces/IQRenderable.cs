namespace QEngine
{
	public interface IQRenderable
	{
		QBehavior Script { get; }
		
		QVector2 Offset { get; set; }

		QVector2 Origin { get; set; }

		QRectangle Source { get; set; }

		QColor Color { get; set; }

		QVector2 Scale { get; set; }

		float Layer { get; set; }

		bool Visible { get; set; }

		QRenderEffects Effect { get; set; }
	}
}