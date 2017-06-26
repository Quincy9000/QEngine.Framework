using System;
using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QCamera : QBehavior, IQStart
	{
		bool _isDirty = true;

		public QMat TransformMatrix { get; private set; } = Matrix.Identity;

		public float MinZoom = 1f;

		public float MaxZoom = 1000f;

		const float DividerZoom = 100f;

		public float Zoom
		{
			//changing this
			get => Transform.Scale.X;
			set
			{
				Transform.Scale = new QVec(MathHelper.Clamp(value, MinZoom, MaxZoom), 0);
				_isDirty = true;
			}
		}

		public QVec Position
		{
			get => Transform.Position;
			set
			{
				Transform.Position = value;
				_isDirty = true;
			}
		}

		public float Rotation
		{
			get => Transform.Rotation;
			set
			{
				Transform.Rotation = MathHelper.ToRadians(value);
				_isDirty = true;
			}
		}

		/// <summary>
		/// Convert the Vector2 to the screen position
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public QVec WorldToScreen(QVec worldPos)
		{
			return QVec.Transform(worldPos, TransformMatrix);
		}

		/// <summary>
		/// Convert the Vector2 to the world inside the camera view
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		public QVec ScreenToWorld(QVec screenPos)
		{
			return Vector2.Transform(screenPos, Matrix.Invert(TransformMatrix));
		}

		public void OnStart(QGetContent get)
		{
			Zoom = 100f;
			UpdateMatrix();
		}

		public QRect Bounds
		{
			get
			{
				var f = Zoom / DividerZoom;
				return
					new QRect(((Position - new QVec(Window.Width, Window.Height) / 2f)) / (f), new Vector2(Window.Width, Window.Height) / (f));
			}
		}

		/// <summary>
		/// Smoothly move the camera around, not constant speed
		/// </summary>
		/// <param name="location"></param>
		/// <param name="interp"></param>
		/// <param name="delta"></param>
		public void Lerp(QVec location, float interp, float delta)
		{
			Position = QVec.Lerp(Position, location, interp * delta);
		}

		public bool IsInCameraView(QVec pos)
		{
			return Bounds.Contains(pos);
		}

		/// <summary>
		/// Moves at a constant speed unlike lerp
		/// </summary>
		/// <param name="location"></param>
		/// <param name="speed"></param>
		/// <param name="delta"></param>
		public void MoveTo(QVec location, float speed, float delta)
		{
			Position += QVec.MoveTowards(Position, location) * speed * delta;
		}

		internal QMat UpdateMatrix()
		{
			if(_isDirty)
			{
				var f = Zoom / DividerZoom;
				TransformMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
				                  Matrix.CreateRotationZ(Rotation) *
				                  Matrix.CreateScale(f, f, 1) *
				                  Matrix.CreateTranslation((Scene.Window.Width / 2f), (Scene.Window.Height / 2f), 0);
				_isDirty = false;
			}
			return TransformMatrix;
		}

		internal QCamera() : base("QCamera") { }
	}
}

//		internal void UpdateMatrix()
//		{
//			if(_isDirty)
//			{
//				var f = (QTime)Math.Round(Zoom / 100f, 2, MidpointRounding.ToEven);
//				TransformMatrix = Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
//				                  Matrix.CreateRotationZ(Rotation) *
//				                  Matrix.CreateScale(f, f, 1) *
//				                  Matrix.CreateTranslation((int)(Scene.Window.Width / 2f), (int)(Scene.Window.Height / 2f), 0);
//				_isDirty = false;
//			}
//		}