using System.Collections.Generic;
using System.Linq;

namespace QEngine
{
	public class QSceneManager
	{
		Dictionary<string, QScene> Scenes { get; }

		QScene CurrentScene { get; set; }

		internal void Init()
		{
			CurrentScene = Scenes.First().Value;
			CurrentScene.OnLoad();
			ResetScene();
		}

		internal void AddScene(QScene scene)
		{
			Scenes[scene.Name] = scene;
		}

		public void ChangeScene(string name)
		{
			if(Scenes.TryGetValue(name, out QScene s) && CurrentScene != null)
			{
				CurrentScene.OnUnload();
				CurrentScene = s;
				CurrentScene.OnLoad();
			}
		}

		public void ResetScene()
		{
			CurrentScene?.OnUnload();
			CurrentScene?.OnLoad();
		}

		public void Update(QTime time)
		{
			CurrentScene.OnUpdate(time);
		}

		public void Draw()
		{
			CurrentScene.OnDraw(CurrentScene.SpriteRenderer);
		}

		public void Gui()
		{
			CurrentScene.OnGui(CurrentScene.GuiRenderer);
		}
		
		public void Unload()
		{
			CurrentScene.OnUnload();
		}

		internal QSceneManager()
		{
			Scenes = new Dictionary<string, QScene>();
		}
	}
}