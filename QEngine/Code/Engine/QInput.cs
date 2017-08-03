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

		static bool IsKeyHeld(QKeyStates keyState)
		{
			return CurrentKeyState.IsKeyDown((Keys)keyState);
		}

		static bool IsKeyHeld(string key)
		{
			if(Enum.TryParse(key, true, out QKeyStates k))
			{
				return IsKeyHeld(k);
			}
			return false;
		}

		static bool IsKeyPressed(QKeyStates keyState)
		{
			return CurrentKeyState.IsKeyDown((Keys)keyState) && PreviousKeyState.IsKeyUp((Keys)keyState);
		}

		static bool IsKeyPressed(string key)
		{
			if(Enum.TryParse(key, true, out QKeyStates k))
			{
				return IsKeyPressed(k);
			}
			return false;
		}

		static bool IsKeyReleased(QKeyStates keyState)
		{
			return CurrentKeyState.IsKeyUp((Keys)keyState) && PreviousKeyState.IsKeyDown((Keys)keyState);
		}

		static bool IsKeyReleased(string key)
		{
			if(Enum.TryParse(key, true, out QKeyStates k))
			{
				return IsKeyReleased(k);
			}
			return false;
		}

		static bool IsAnyKeyDown()
		{
			return CurrentKeyState.GetPressedKeys().Length > 0 && PreviousKeyState.GetPressedKeys().Length == 0;
		}

		static bool IsAnyKeyUp()
		{
			return CurrentKeyState.GetPressedKeys().Length == 0 && PreviousKeyState.GetPressedKeys().Length > 0;
		}

		static bool IsAnyKeyHeld()
		{
			return CurrentKeyState.GetPressedKeys().Length > 0 && PreviousKeyState.GetPressedKeys().Length > 0;
		}

		static bool IsNoKey()
		{
			return CurrentKeyState.GetPressedKeys().Length == 0 && PreviousKeyState.GetPressedKeys().Length == 0;
		}

		static bool IsMouseScrolledUp()
		{
			return CurrentMouseState.ScrollWheelValue > PreviousMouseState.ScrollWheelValue;
		}

		static bool IsMouseScrolledDown()
		{
			return CurrentMouseState.ScrollWheelValue < PreviousMouseState.ScrollWheelValue;
		}

		static QVec MousePosition()
		{
			return CurrentMouseState.Position;
		}

		/*LeftMouseButton*/

		static bool IsLeftMouseButtonPressed()
		{
			return CurrentMouseState.LeftButton == ButtonState.Pressed &&
			       PreviousMouseState.LeftButton == ButtonState.Released;
		}

		static bool IsLeftMouseButtonHeld()
		{
			return CurrentMouseState.LeftButton == ButtonState.Pressed;
		}

		static bool IsLeftMouseButtonReleased()
		{
			return CurrentMouseState.LeftButton == ButtonState.Released &&
			       CurrentMouseState.LeftButton == ButtonState.Pressed;
		}

		/*RightMouseButton*/

		static bool IsRightMouseButtonPressed()
		{
			return CurrentMouseState.RightButton == ButtonState.Pressed &&
			       PreviousMouseState.RightButton == ButtonState.Released;
		}

		static bool IsRightMouseButtonHeld()
		{
			return CurrentMouseState.RightButton == ButtonState.Pressed;
		}

		static bool IsRightMouseButtonReleased()
		{
			return CurrentMouseState.RightButton == ButtonState.Released &&
			       CurrentMouseState.RightButton == ButtonState.Pressed;
		}

		/*MiddleMouseButton*/

		static bool IsMiddleMouseButtonPressed()
		{
			return CurrentMouseState.MiddleButton == ButtonState.Pressed &&
			       PreviousMouseState.MiddleButton == ButtonState.Released;
		}

		static bool IsMiddleMouseButtonHeld()
		{
			return CurrentMouseState.MiddleButton == ButtonState.Pressed;
		}

		static bool IsMiddleMouseButtonReleased()
		{
			return CurrentMouseState.MiddleButton == ButtonState.Released &&
			       CurrentMouseState.MiddleButton == ButtonState.Pressed;
		}

		/*BackwardsMouseButton*/

		static bool IsBackwardsMouseButtonPressed()
		{
			return CurrentMouseState.XButton1 == ButtonState.Pressed &&
			       PreviousMouseState.XButton1 == ButtonState.Released;
		}

		static bool IsBackwardsMouseButtonHeld()
		{
			return CurrentMouseState.XButton1 == ButtonState.Pressed;
		}

		static bool IsBackwardsMouseButtonReleased()
		{
			return CurrentMouseState.XButton1 == ButtonState.Released &&
			       CurrentMouseState.XButton1 == ButtonState.Pressed;
		}

		/*ForwardsMouseButton*/

		static bool IsForwardsMouseButtonPressed()
		{
			return CurrentMouseState.XButton2 == ButtonState.Pressed &&
			       PreviousMouseState.XButton2 == ButtonState.Released;
		}

		static bool IsForwardsMouseButtonHeld()
		{
			return CurrentMouseState.XButton2 == ButtonState.Pressed;
		}

		static bool IsForwardsMouseButtonReleased()
		{
			return CurrentMouseState.XButton2 == ButtonState.Released &&
			       CurrentMouseState.XButton2 == ButtonState.Pressed;
		}

		/*MouseMoved*/

		static bool IsMouseMoving()
		{
			return CurrentMouseState.Position != PreviousMouseState.Position;
		}
	}
}