using System;
using Microsoft.Xna.Framework.Input;

namespace QEngine
{
	public sealed class QController : QBehavior, IQStart, IQUpdate
	{
		public void OnStart(QGetContent get)
		{
			GetControls();
		}

		void Flush()
		{
			QInput.PreviousKeyState = QInput.CurrentKeyState;
			QInput.PreviousMouseState = QInput.CurrentMouseState;
		}

		void GetControls()
		{
			QInput.CurrentKeyState = Keyboard.GetState();
			QInput.CurrentMouseState = Mouse.GetState();
		}

		void Update()
		{
			Flush();
			GetControls();
		}

		public void OnUpdate(QTime delta)
		{
			Update();
		}
	}
}