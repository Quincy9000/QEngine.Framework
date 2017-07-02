using System;
using Microsoft.Xna.Framework.Input;

namespace QEngine
{
	public class QControls : QBehavior, IQStart, IQUpdate
	{
		KeyboardState PreviousKeyState;
		KeyboardState CurrentKeyState;

		MouseState PreviousMouseState;
		MouseState CurrentMouseState;

		/// <summary>
		/// Checks if the key is down, so aka rapid fire
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyHeld(QKeys key)
		{
			return CurrentKeyState.IsKeyDown((Keys)key);
		}

		/// <summary>
		/// Checks if the key is down, so aka rapid fire
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyHeld(string key)
		{
			if(Enum.TryParse(key, true, out QKeys k))
			{
				return IsKeyHeld(k);
			}
			return false;
		}

		/// <summary>
		/// If the key was pressed and last state was not pressed
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyPressed(QKeys key)
		{
			return CurrentKeyState.IsKeyDown((Keys)key) && PreviousKeyState.IsKeyUp((Keys)key);
		}
		
		/// <summary>
		/// If the key was pressed and last state was not pressed
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyPressed(string key)
		{
			if(Enum.TryParse(key, true, out QKeys k))
			{
				return IsKeyPressed(k);
			}
			return false;
		}

		/// <summary>
		/// Only returns true if they key was released, not when its up
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyReleased(QKeys key)
		{
			return CurrentKeyState.IsKeyUp((Keys)key) && PreviousKeyState.IsKeyDown((Keys)key);
		}
		
		/// <summary>
		/// Only returns true if they key was released, not when its up
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyReleased(string key)
		{
			if(Enum.TryParse(key, true, out QKeys k))
			{
				return IsKeyReleased(k);
			}
			return false;
		}

		public bool IsAnyKeyDown()
		{
			return CurrentKeyState.GetPressedKeys().Length > 0 && PreviousKeyState.GetPressedKeys().Length == 0;
		}

		public bool IsAnyKeyUp()
		{
			return CurrentKeyState.GetPressedKeys().Length == 0 && PreviousKeyState.GetPressedKeys().Length > 0;
		}

		public bool IsAnyKeyHeld()
		{
			return CurrentKeyState.GetPressedKeys().Length > 0 && PreviousKeyState.GetPressedKeys().Length > 0;
		}

		public bool IsNoKey()
		{
			return CurrentKeyState.GetPressedKeys().Length == 0 && PreviousKeyState.GetPressedKeys().Length == 0;
		}

		public bool IsMouseScrolledUp()
		{
			return CurrentMouseState.ScrollWheelValue > PreviousMouseState.ScrollWheelValue;
		}

		public bool IsMouseScrolledDown()
		{
			return CurrentMouseState.ScrollWheelValue < PreviousMouseState.ScrollWheelValue;
		}

		public QVec MousePosition()
		{
			return CurrentMouseState.Position;
		}

		/*LeftMouseButton*/

		public bool IsLeftMouseButtonDown()
		{
			return CurrentMouseState.LeftButton == ButtonState.Pressed &&
			       PreviousMouseState.LeftButton == ButtonState.Released;
		}

		public bool IsLeftMouseButtonHeld()
		{
			return CurrentMouseState.LeftButton == ButtonState.Pressed;
		}

		public bool IsLeftMouseButtonUp()
		{
			return CurrentMouseState.LeftButton == ButtonState.Released &&
			       CurrentMouseState.LeftButton == ButtonState.Pressed;
		}

		/*RightMouseButton*/

		public bool IsRightMouseButtonDown()
		{
			return CurrentMouseState.RightButton == ButtonState.Pressed &&
			       PreviousMouseState.RightButton == ButtonState.Released;
		}

		public bool IsRightMouseButtonHeld()
		{
			return CurrentMouseState.RightButton == ButtonState.Pressed;
		}

		public bool IsRightMouseButtonUp()
		{
			return CurrentMouseState.RightButton == ButtonState.Released &&
			       CurrentMouseState.RightButton == ButtonState.Pressed;
		}

		/*MiddleMouseButton*/

		public bool IsMiddleMouseButtonDown()
		{
			return CurrentMouseState.MiddleButton == ButtonState.Pressed &&
			       PreviousMouseState.MiddleButton == ButtonState.Released;
		}

		public bool IsMiddleMouseButtonHeld()
		{
			return CurrentMouseState.MiddleButton == ButtonState.Pressed;
		}

		public bool IsMiddleMouseButtonUp()
		{
			return CurrentMouseState.MiddleButton == ButtonState.Released &&
			       CurrentMouseState.MiddleButton == ButtonState.Pressed;
		}

		/*BackwardsMouseButton*/

		public bool IsBackwardsMouseButtonDown()
		{
			return CurrentMouseState.XButton1 == ButtonState.Pressed &&
			       PreviousMouseState.XButton1 == ButtonState.Released;
		}

		public bool IsBackwardsMouseButtonHeld()
		{
			return CurrentMouseState.XButton1 == ButtonState.Pressed;
		}

		public bool IsBackwardsMouseButtonUp()
		{
			return CurrentMouseState.XButton1 == ButtonState.Released &&
			       CurrentMouseState.XButton1 == ButtonState.Pressed;
		}

		/*ForwardsMouseButton*/

		public bool IsForwardsMouseButtonDown()
		{
			return CurrentMouseState.XButton2 == ButtonState.Pressed &&
			       PreviousMouseState.XButton2 == ButtonState.Released;
		}

		public bool IsForwardsMouseButtonHeld()
		{
			return CurrentMouseState.XButton2 == ButtonState.Pressed;
		}

		public bool IsForwardsMouseButtonUp()
		{
			return CurrentMouseState.XButton2 == ButtonState.Released &&
			       CurrentMouseState.XButton2 == ButtonState.Pressed;
		}

		/*MouseMoved*/

		public bool IsMouseMoving()
		{
			return CurrentMouseState.Position != PreviousMouseState.Position;
		}

		public void OnStart(QGetContent get)
		{
			GetControls();
		}

		void Flush()
		{
			PreviousKeyState = CurrentKeyState;
			PreviousMouseState = CurrentMouseState;
		}

		void GetControls()
		{
			CurrentKeyState = Keyboard.GetState();
			CurrentMouseState = Mouse.GetState();
		}

		void Update()
		{
			Flush();
			GetControls();
		}

		public void OnUpdate(float time)
		{
			Update();
		}
	}
}