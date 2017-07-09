using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QTransform
	{
		QObject parent;

		QVec position;

		float rotation = 0;
		
		QVec scale;

		/// <summary>
		/// if the transform is dirty we update the body and transform
		/// </summary>
		internal bool IsDirty;

		internal QRigiBody body;

		internal QRigiBody Body
		{
			get => body;
			set
			{
				body = value;
				body.Position = position;
				body.Rotation = rotation;
			}
		}

		public QVec Position
		{
			get => body?.Position ?? position; //
			set
			{
				position = value;
				IsDirty = true;
				if(body != null)
					body.Position = value;
			}
		}

		public QVec Scale
		{
			get => scale;
			set
			{
				scale = value;
				IsDirty = true;
			}
		}

		public float Rotation
		{
			get => body?.Rotation ?? rotation; //
			set
			{
				rotation = value;
				IsDirty = true;
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

		public void Reset(QVec pos, QVec scale, float rot)
		{
			body = null;
			Position = pos;
			Scale = scale;
			Rotation = rot;
			IsDirty = true;
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