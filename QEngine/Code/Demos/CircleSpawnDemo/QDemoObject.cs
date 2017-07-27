using System.Collections;
using QEngine.Prefabs;

namespace QEngine.Demos.CircleSpawnDemo
{
	public class QDemoObject : QCharacterController
	{
		QSprite sprite;

		QTexture euro;

		QFont font;

		QLabel Arial;

		QTexture Square;

		const float speed = 500f;

		public override void OnLoad(QAddContent add)
		{
			add.Texture("asdf");
			add.Font("arial");
			add.Rectangle("penis", 100, 100, QColor.DarkRed);
			//add.Circle("penis", 100, QColor.DarkRed);
		}

		public override void OnStart(QGetContent get)
		{
			Square = get.Texture("penis");
			font = get.Font("arial");
			euro = get.Texture("asdf");

			Arial = new QLabel(font);
			sprite = new QSprite(this, "penis");
			Instantiate(new QDemoObject2());

			Console.Label = Arial;
			Console.Color = QColor.DarkGoldenrod;

			Coroutine.Start(Thing());
		}

		public override void OnUpdate(QTime time)
		{
			if(QInput.Held(QKeyStates.W))
				Transform.Position += QVec.Up * speed * time.Delta;
			if(QInput.Held(QKeyStates.A))
				Transform.Position += QVec.Left * speed * time.Delta;
			if(QInput.Held(QKeyStates.S))
				Transform.Position += QVec.Down * speed * time.Delta;
			if(QInput.Held(QKeyStates.D))
				Transform.Position += QVec.Right * speed * time.Delta;
			if(QInput.Pressed(QKeyStates.Tab))
				Scene.ResetScene();
			if(QInput.Held(QKeyStates.Space) && Accumulator.CheckAccum("ball", 0.06f))
				Instantiate(new QDemoCircle());
			if(QInput.IsMouseScrolledUp())
				Camera.Zoom += Camera.Zoom * 0.1f;
			if(QInput.IsMouseScrolledDown())
				Camera.Zoom -= Camera.Zoom * 0.1f;
			if(QInput.IsForwardsMouseButtonPressed())
				Console.WriteLine("forwardsMouseButtonDown");
			if(QInput.IsBackwardsMouseButtonPressed())
				Console.WriteLine("backwardsMouseButton");
			if(QInput.IsMiddleMouseButtonPressed())
				Console.WriteLine("middleMouseButton");
			Camera.Lerp(Transform.Position, 5, time.Delta);
		}

		IEnumerator Thing()
		{
			Instantiate(new QDemoCircle());
			yield return QCoroutine.WaitForSeconds(1);
			Coroutine.Start(Thing()); //probably bad
		}

		public override void OnDrawSprite(QSpriteRenderer spriteRenderer)
		{
			spriteRenderer.Draw(sprite, Transform);
		}

		public QDemoObject() : base("QDemoObject") { }
	}
}