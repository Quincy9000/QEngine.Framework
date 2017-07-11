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
			if(Scenes.TryGetValue(name, out QScene s) && CurrentScene != null)
			{
				CurrentScene.OnUnload();
				CurrentScene = s;
				CurrentScene.OnLoad();
			}
			CallToChangeScene = false;
			CallToChangeSceneName = "";
		}

		public void ResetScene()
		{
			CurrentScene?.OnUnload();
			CurrentScene?.OnLoad();
			CallToResetScene = false;
		}

		public void Update(QTime time)
		{
			if(CallToChangeScene)
				ChangeScene(CallToChangeSceneName);
			else if(CallToResetScene)
				ResetScene();
			else if(CallToExitGame)
				CurrentScene.Engine.Exit();
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