using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QCamera : QBehavior, IQStart
	{
		bool _isDirty = true;

		QMat _transformMatrix;

		public QMat TransformMatrix
		{
			get
			{
				if(_isDirty)
					UpdateMatrix();
				return _transformMatrix;
			}
		}

		public float MinZoom = 1f;

		public float MaxZoom = 1000f;

		/// <summary>
		/// Default zoom level when the scene starts
		/// </summary>
		public static float DefaultZoom = 100f;

		/// <summary>
		/// Just lets us use higher numbers for the camera so that its easier to manage
		/// but we need to divide the zoom whenever we want to interact with anything
		/// </summary>
		const float Divider = 100f;

		/// <summary>
		/// Using the scale as the zoom for the camera
		/// </summary>
		public float Zoom
		{
			get => Transform.Scale.X;
			set
			{
				Transform.Scale = new QVec(MathHelper.Clamp(value, MinZoom, MaxZoom), 1);
				_isDirty = true;
			}
		}

		/// <summary>
		/// Hide the old position and use this one instead to 
		/// make sure that the view only gets update when it needs to
		/// </summary>
		public new QVec Position
		{
			get => Transform.Position;
			set
			{
				Transform.Position = value;
				_isDirty = true;
			}
		}

		/// <summary>
		/// Hide the old rotation from script and use this one instead
		/// </summary>
		public new float Rotation
		{
			get => Transform.Rotation;
			set
			{
				Transform.Rotation = value; //MathHelper.ToRadians(value);
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

		/// <summary>
		/// Set the default zoom value
		/// </summary>
		/// <param name="get"></param>
		public void OnStart(QGetContent get)
		{
			Zoom = DefaultZoom;
		}

		/// <summary>
		/// Gets the window around the camera in world units
		/// </summary>
		public QRect Bounds
		{
			get
			{
				//f gives us the proper bounds no matter what the zoom level is
				var f = Zoom / Divider;
				return
					new QRect(
						//Cameras is in middle so we - half width and height to get top left
						Position - new QVec(Window.Width + 2, Window.Height + 2) / 2f / f,
						//bottom right corner of the camera
						new Vector2(Window.Width + 1, Window.Height + 1) / f);
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

		/// <summary>
		/// Checks if position is in the bounds of the camera
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
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

		/// <summary>
		/// returns matrix for the spritebatch
		/// </summary>
		/// <returns></returns>
		void UpdateMatrix()
		{
			var f = Zoom / Divider;
			_transformMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
			                   Matrix.CreateRotationZ(Rotation) *
			                   Matrix.CreateScale(f, f, 1) *
			                   Matrix.CreateTranslation((Scene.Window.Width / 2f), (Scene.Window.Height / 2f), 0);
			_isDirty = false;
		}
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