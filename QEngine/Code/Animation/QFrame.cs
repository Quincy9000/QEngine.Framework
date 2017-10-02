using System.ComponentModel;

namespace QEngine
{
	[ImmutableObject(true)]
	struct QFrame
	{
		public bool Equals(QFrame other)
		{
			return SourceRectangle.Equals(other.SourceRectangle) && Duration.Equals(other.Duration);
		}

		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			return obj is QFrame && Equals((QFrame)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (SourceRectangle.GetHashCode() * 397) ^ Duration.GetHashCode();
			}
		}

		public QRectangle SourceRectangle { get; }
		public float Duration { get; }

		public static bool operator ==(QFrame left, QFrame right)
		{
			return left.SourceRectangle == right.SourceRectangle;
		}

		public static bool operator !=(QFrame left, QFrame right)
		{
			return left.SourceRectangle != right.SourceRectangle;
		}

		public QFrame(QRectangle r, float d)
		{
			SourceRectangle = r;
			Duration = d;
		}
	}
}