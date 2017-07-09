using System.Collections.Generic;
using System.Linq;

namespace QEngine
{
	public class QSceneManager
	{
		Dictionary<string, QScene> Scenes { get; }
		
		internal bool CallToResetScene = false;

		internal bool CallToExitGame = false;

		internal bool CallToChangeScene = false;

		internal string CallToChangeSceneName = "";

		internal QScene CurrentScene { get; set; }

		internal void Init()
		{
			CurrentScene = Scenes.First().Value;
			CurrentScene.OnLoad();
			for(int i = 0; i < 3; i++)
			{
				ResetScene();
			}
		}

		internal void AddScene(QScene scene)
		{
			Scenes[scene.Name] = scene;
		}

		public void ChangeScene(string name)
		{
			CallToChangeScene = false;
			if(Scenes.TryGetValue(name, out QScene s) && CurrentScene != null)
			{
				CurrentScene.OnUnload();
				CurrentScene = s;
				CurrentScene.OnLoad();
			}
		}

		public void ResetScene()
		{
			CallToResetScene = false;
			CurrentScene?.OnUnload();
			CurrentScene?.OnLoad();
		}

		public void Update(QTime time)
		{
			CurrentScene.OnUpdate(time);
			if(CallToChangeScene)
				ChangeScene(CallToChangeSceneName);
			else if(CallToResetScene)
				ResetScene();
			else if(CallToExitGame)
				CurrentScene.ExitGame();
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