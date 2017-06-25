using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QTransform
	{
		public QVec Position { get; set; } = QVec.Zero;

		public QVec Scale { get; set; } = QVec.One;

		float rotation = 0;

		public float Rotation
		{
			get { return rotation; }
			set { rotation = MathHelper.ToRadians(value); }
		}

		public static QVec RotateAboutOrigin(QVec point, QVec origin, float rotation)
		{
			return QVec.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
		}

		public void Reset()
		{
			Position = QVec.Zero;
			Scale = QVec.One;
			Rotation = 0;
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode() + Scale.GetHashCode() + rotation.GetHashCode();
		}

		public static bool operator ==(QTransform t, QTransform t2)
		{
			return t.Position == t2.Position && t.Scale == t2.Scale && t.Rotation == t2.Rotation;
		}

		public static bool operator !=(QTransform t, QTransform t2)
		{
			return t.Position != t2.Position || t.Scale != t2.Scale || t.Rotation != t2.Rotation;
		}

		public override bool Equals(object obj)
		{
			if(!(obj is QTransform t)) return false;
			return this == t;
		}

		internal QTransform(QObject parent) { }

		internal QTransform() { }
	}
}