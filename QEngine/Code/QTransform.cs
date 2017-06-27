using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QTransform
	{
		QObject parent;

		QVec pos;

		float rotation = 0;

		internal QRigiBody body;

		public QVec Position
		{
			get => body?.Position ?? pos;
			set
			{
				pos = value;
				if(body != null)
					body.Position = value;
			}
		}

		public QVec Scale { get; set; }

		public float Rotation
		{
			get => body?.Rotation ?? rotation;
			set
			{
				rotation = value;
				if(body != null)
					body.Rotation = value;
			}
		}

		public static QVec RotateAboutOrigin(QVec point, QVec origin, float rotation)
		{
			return QVec.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
		}

		public void Reset()
		{
			body = null;
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
			if(!(obj is QTransform t))
				return false;
			return this == t;
		}

		internal QTransform(QObject parent)
		{
			this.parent = parent;
			Reset();
		}
	}
}