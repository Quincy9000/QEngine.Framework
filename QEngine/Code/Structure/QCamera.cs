using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QCamera : QBehavior, IQStart
	{
		bool _isDirty = true;

		QMatrix _transformMatrix;

		float _zoom = 1f;

		public QMatrix TransformMatrix
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
			get => _zoom;
			set
			{
				_zoom = MathHelper.Clamp(value, MinZoom, MaxZoom);
				_isDirty = true;
			}
		}

		/// <summary>
		/// Hide the old position and use this one instead to 
		/// make sure that the view only gets update when it needs to
		/// </summary>
		public new QVector2 Position
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
		public QVector2 WorldToScreen(QVector2 worldPos)
		{
			return QVector2.Transform(worldPos, TransformMatrix);
		}

		/// <summary>
		/// Convert the Vector2 to the world inside the camera view
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		public QVector2 ScreenToWorld(QVector2 screenPos)
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
		public QRectangle Bounds
		{
			get
			{
				//f gives us the proper bounds no matter what the zoom level is
				var f = Zoom / Divider;
				return
					new QRectangle(
						//Cameras is in middle so we - half width and height to get top left
						Position - new QVector2(World.Window.Width + 2, World.Window.Height + 2) / 2f / f,
						//bottom right corner of the camera
						new Vector2(World.Window.Width + 1, World.Window.Height + 1) / f);
			}
		}

		/// <summary>
		/// Smoothly move the camera around, not constant speed
		/// </summary>
		/// <param name="location"></param>
		/// <param name="interp"></param>
		/// <param name="delta"></param>
		public void Lerp(QVector2 location, float interp, float delta)
		{
			Position = QVector2.Lerp(Position, location, interp * delta);
		}

		/// <summary>
		/// Checks if position is in the bounds of the camera
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool IsInCameraView(QVector2 pos)
		{
			return Bounds.Contains(pos);
		}

		/// <summary>
		/// Moves at a constant speed unlike lerp
		/// </summary>
		/// <param name="location"></param>
		/// <param name="speed"></param>
		/// <param name="delta"></param>
		public void MoveTo(QVector2 location, float speed, float delta)
		{
			Position += QVector2.MoveTowards(Position, location) * speed * delta;
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
			                   Matrix.CreateTranslation(World.Window.Width / 2f, World.Window.Height / 2f, 0);
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