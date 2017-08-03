using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QTransform
	{
		QObject parent;

		internal QVec position;

		internal float rotation;

		/// <summary>
		/// if the transform is dirty we update the body and transform
		/// </summary>
		internal bool IsDirty;

		QRigidBody body;

		internal QRigidBody Body
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
//			get => position;//body?.Position ?? position; //
			get => body?.Position ?? position; //
			set
			{
				IsDirty = true;
				position = value;
				if(body != null)
					body.Position = value;
			}
		}

		internal float Rotation
		{
//			get => rotation;//body?.Rotation ?? rotation; //
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

		public void Reset(QVec pos = default(QVec), float rot = 0)
		{
			body = null;
			Position = pos;
			Rotation = rot;
			IsDirty = true;
		}

		public static bool operator ==(QTransform t, QTransform t2)
		{
			return t.Position == t2.Position && t.Rotation == t2.Rotation;
		}

		public static bool operator !=(QTransform t, QTransform t2)
		{
			return t.Position != t2.Position || t.Rotation != t2.Rotation;
		}

		public override bool Equals(object obj)
		{
			if(!(obj is QTransform t))
				return false;
			return this == t;
		}

		internal QTransform(QObject p)
		{
			parent = p;
			Reset();
		}
	}
}