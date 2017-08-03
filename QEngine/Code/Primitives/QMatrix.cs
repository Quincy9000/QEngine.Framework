using Microsoft.Xna.Framework;

namespace QEngine
{
	/// <summary>
	/// Matrix
	/// </summary>
	public class QMatrix
	{
		readonly Matrix _mat;

		public QMatrix()
		{
			_mat = Matrix.Identity;
		}

		public static QMatrix Identity => Matrix.Identity;

		/// <summary>
		/// Moves the matrix on the 2D plane
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static QMatrix Translate(QVec v) => Matrix.CreateTranslation(v.X, v.Y, 1);

		public static QMatrix Rotate(float r) => Matrix.CreateRotationZ(r);
		
		public static QMatrix operator+ (QMatrix left, QMatrix right) => Matrix.Add(left, right);

		public static QMatrix operator -(QMatrix left, QMatrix right) => Matrix.Subtract(left, right);

		public static QMatrix operator *(QMatrix left, QMatrix right) => Matrix.Multiply(left, right);
		
		public static QMatrix operator /(QMatrix left, QMatrix right) => Matrix.Divide(left, right);

		public QMatrix(Matrix m)
		{
			_mat = m;
		}

		public static implicit operator Matrix(QMatrix m)
		{
			return m._mat;
		}

		public static implicit operator QMatrix(Matrix m)
		{
			return new QMatrix(m);
		}
	}
}