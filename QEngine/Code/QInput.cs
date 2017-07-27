using System;
using Microsoft.Xna.Framework.Input;

namespace QEngine
{
	public static class QInput
	{
		internal static KeyboardState PreviousKeyState { get; set; }
		internal static KeyboardState CurrentKeyState { get; set; }

		internal static MouseState PreviousMouseState { get; set; }
		internal static MouseState CurrentMouseState { get; set; }

		public static bool Held(QKeyStates button)
		{
			return IsKeyHeld(button);
		}

		/// <summary>
		/// Checks the string if its a valid button and then checks if its held down
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public static bool Held(string button)
		{
			return IsKeyHeld(button);
		}

		/// <summary>
		/// Check if a mouse button is held down, does not include scroll wheel
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static bool Held(QMouseStates button)
		{
			switch(button)
			{
				case QMouseStates.Left:
					return IsLeftMouseButtonHeld();
				case QMouseStates.Right:
					return IsRightMouseButtonHeld();
				case QMouseStates.Middle:
					return IsMiddleMouseButtonHeld();
				case QMouseStates.Forward:
					return IsForwardsMouseButtonHeld();
				case QMouseStates.Backward:
					return IsBackwardsMouseButtonHeld();
				//You can't hold down the scroll wheel so we don't check that
				default:
					throw new ArgumentOutOfRangeException(nameof(button), button, null);
			}
		}

		public static bool Pressed(QKeyStates button)
		{
			return IsKeyPressed(button);
		}

		public static bool Pressed(string button)
		{
			return IsKeyPressed(button);
		}

		/// <summary>
		/// Checks if mouse button was pressed, this includes the scroll wheel
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static bool Pressed(QMouseStates button)
		{
			switch(button)
			{
				case QMouseStates.Left:
					return IsLeftMouseButtonPressed();
				case QMouseStates.Right:
					return IsRightMouseButtonPressed();
				case QMouseStates.Middle:
					return IsMiddleMouseButtonPressed();
				case QMouseStates.Forward:
					return IsForwardsMouseButtonPressed();
				case QMouseStates.Backward:
					return IsBackwardsMouseButtonPressed();
				//TODO Make sure this works in Pressed and Held and Released
				case QMouseStates.Up:
					return IsMouseScrolledUp();
				case QMouseStates.Down:
					return IsMouseScrolledDown();
				default:
					throw new ArgumentOutOfRangeException(nameof(button), button, null);
			}
		}

		public static bool Released(QKeyStates button)
		{
			return IsKeyReleased(button);
		}

		public static bool Released(string button)
		{
			return IsKeyReleased(button);
		}

		public static bool Released(QMouseStates button)
		{
			switch(button)
			{
				case QMouseStates.Left:
					return IsLeftMouseButtonReleased();
				case QMouseStates.Right:
					return IsRightMouseButtonReleased();
				case QMouseStates.Middle:
					return IsMiddleMouseButtonReleased();
				case QMouseStates.Forward:
					return IsForwardsMouseButtonReleased();
				case QMouseStates.Backward:
					return IsBackwardsMouseButtonReleased();
				//You can't release the scroll wheel up and down so we don't include those here
				default:
					throw new ArgumentOutOfRangeException(nameof(button), button, null);
			}
		}

		public static bool IsKeyHeld(QKeyStates keyState)
		{
			return CurrentKeyState.IsKeyDown((Keys)keyState);
		}

		public static bool IsKeyHeld(string key)
		{
			if(Enum.TryParse(key, true, out QKeyStates k))
			{
				return IsKeyHeld(k);
			}
			return false;
		}

		public static bool IsKeyPressed(QKeyStates keyState)
		{
			return CurrentKeyState.IsKeyDown((Keys)keyState) && PreviousKeyState.IsKeyUp((Keys)keyState);
		}

		public static bool IsKeyPressed(string key)
		{
			if(Enum.TryParse(key, true, out QKeyStates k))
			{
				return IsKeyPressed(k);
			}
			return false;
		}

		public static bool IsKeyReleased(QKeyStates keyState)
		{
			return CurrentKeyState.IsKeyUp((Keys)keyState) && PreviousKeyState.IsKeyDown((Keys)keyState);
		}

		public static bool IsKeyReleased(string key)
		{
			if(Enum.TryParse(key, true, out QKeyStates k))
			{
				return IsKeyReleased(k);
			}
			return false;
		}

		public static bool IsAnyKeyDown()
		{
			return CurrentKeyState.GetPressedKeys().Length > 0 && PreviousKeyState.GetPressedKeys().Length == 0;
		}

		public static bool IsAnyKeyUp()
		{
			return CurrentKeyState.GetPressedKeys().Length == 0 && PreviousKeyState.GetPressedKeys().Length > 0;
		}

		public static bool IsAnyKeyHeld()
		{
			return CurrentKeyState.GetPressedKeys().Length > 0 && PreviousKeyState.GetPressedKeys().Length > 0;
		}

		public static bool IsNoKey()
		{
			return CurrentKeyState.GetPressedKeys().Length == 0 && PreviousKeyState.GetPressedKeys().Length == 0;
		}

		public static bool IsMouseScrolledUp()
		{
			return CurrentMouseState.ScrollWheelValue > PreviousMouseState.ScrollWheelValue;
		}

		public static bool IsMouseScrolledDown()
		{
			return CurrentMouseState.ScrollWheelValue < PreviousMouseState.ScrollWheelValue;
		}

		public static QVec MousePosition()
		{
			return CurrentMouseState.Position;
		}

		/*LeftMouseButton*/

		public static bool IsLeftMouseButtonPressed()
		{
			return CurrentMouseState.LeftButton == ButtonState.Pressed &&
			       PreviousMouseState.LeftButton == ButtonState.Released;
		}

		public static bool IsLeftMouseButtonHeld()
		{
			return CurrentMouseState.LeftButton == ButtonState.Pressed;
		}

		public static bool IsLeftMouseButtonReleased()
		{
			return CurrentMouseState.LeftButton == ButtonState.Released &&
			       CurrentMouseState.LeftButton == ButtonState.Pressed;
		}

		/*RightMouseButton*/

		public static bool IsRightMouseButtonPressed()
		{
			return CurrentMouseState.RightButton == ButtonState.Pressed &&
			       PreviousMouseState.RightButton == ButtonState.Released;
		}

		public static bool IsRightMouseButtonHeld()
		{
			return CurrentMouseState.RightButton == ButtonState.Pressed;
		}

		public static bool IsRightMouseButtonReleased()
		{
			return CurrentMouseState.RightButton == ButtonState.Released &&
			       CurrentMouseState.RightButton == ButtonState.Pressed;
		}

		/*MiddleMouseButton*/

		public static bool IsMiddleMouseButtonPressed()
		{
			return CurrentMouseState.MiddleButton == ButtonState.Pressed &&
			       PreviousMouseState.MiddleButton == ButtonState.Released;
		}

		public static bool IsMiddleMouseButtonHeld()
		{
			return CurrentMouseState.MiddleButton == ButtonState.Pressed;
		}

		public static bool IsMiddleMouseButtonReleased()
		{
			return CurrentMouseState.MiddleButton == ButtonState.Released &&
			       CurrentMouseState.MiddleButton == ButtonState.Pressed;
		}

		/*BackwardsMouseButton*/

		public static bool IsBackwardsMouseButtonPressed()
		{
			return CurrentMouseState.XButton1 == ButtonState.Pressed &&
			       PreviousMouseState.XButton1 == ButtonState.Released;
		}

		public static bool IsBackwardsMouseButtonHeld()
		{
			return CurrentMouseState.XButton1 == ButtonState.Pressed;
		}

		public static bool IsBackwardsMouseButtonReleased()
		{
			return CurrentMouseState.XButton1 == ButtonState.Released &&
			       CurrentMouseState.XButton1 == ButtonState.Pressed;
		}

		/*ForwardsMouseButton*/

		public static bool IsForwardsMouseButtonPressed()
		{
			return CurrentMouseState.XButton2 == ButtonState.Pressed &&
			       PreviousMouseState.XButton2 == ButtonState.Released;
		}

		public static bool IsForwardsMouseButtonHeld()
		{
			return CurrentMouseState.XButton2 == ButtonState.Pressed;
		}

		public static bool IsForwardsMouseButtonReleased()
		{
			return CurrentMouseState.XButton2 == ButtonState.Released &&
			       CurrentMouseState.XButton2 == ButtonState.Pressed;
		}

		/*MouseMoved*/

		public static bool IsMouseMoving()
		{
			return CurrentMouseState.Position != PreviousMouseState.Position;
		}
	}
}