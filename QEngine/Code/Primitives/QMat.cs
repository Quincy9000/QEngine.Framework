using Microsoft.Xna.Framework;

namespace QEngine
{
	/// <summary>
	/// Matrix
	/// </summary>
	public class QMat
	{
		Matrix mat;

		public QMat()
		{
			mat = Matrix.Identity;
		}

		public static QMat Identity => Matrix.Identity;

		public QMat(Matrix m)
		{
			mat = m;
		}

		public static implicit operator Matrix(QMat m)
		{
			return m.mat;
		}

		public static implicit operator QMat(Matrix m)
		{
			return new QMat(m);
		}
	}
}