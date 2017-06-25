using Microsoft.Xna.Framework.Graphics;

namespace QEngine
{
	/// <summary>
	/// QEffect is the class that loads the shader from monogame pipelien
	/// </summary>
	public class QEffect
	{
		Effect effect;

		public static implicit operator Effect(QEffect e)
		{
			return e.effect;
		}

		public static implicit operator QEffect(Effect e)
		{
			return new QEffect(e);
		}

		internal QEffect(Effect f)
		{
			effect = f;
		}
	}
}