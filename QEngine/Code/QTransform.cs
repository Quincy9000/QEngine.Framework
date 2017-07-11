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

		QRigiBody body;

		internal QRigiBody Body
		{
			get => body;
			set
			{
				body = value;
				body.Position = position;
				body.Rotation = rotation;
				IsDirty = true;
			}
		}

		internal QVec Position
		{
			get => body?.Position ?? position; //
			set
			{
				IsDirty = true;
				position = value;
				if(body != null)
					body.Position = value;
			}
		}

		internal QVec Scale
		{
			get => scale;
			set
			{
				scale = value;
				IsDirty = true;
			}
		}

		internal float Rotation
		{
			get => body?.Rotation ?? rotation; //
			set
			{
				IsDirty = true;
				rotation = value;
				if(body != null)
					body.Rotation = value;
			}
		}

		public static QVec RotateAboutOrigin(QVec point, QVec origin, float rotation)
		{
			return QVec.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
		}

		public void Reset(QVec pos = default(QVec), QVec sca = default (QVec), float rot = 0)
		{
			body = null;
			Position = pos;
			Scale = sca;
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