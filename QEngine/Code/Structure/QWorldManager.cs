using System.Collections.Generic;
using System.Linq;

namespace QEngine
{
	public class QWorldManager
	{
		Dictionary<string, QWorld> Scenes { get; }

		internal bool CallToResetScene = false;

		internal bool CallToExitGame = false;

		internal bool CallToChangeScene = false;

		internal string CallToChangeSceneName = "";

		internal QWorld CurrentWorld { get; set; }

		internal void Init()
		{
			CurrentWorld = Scenes.First().Value;
			CurrentWorld.OnLoad();
		}

		internal void AddScene(QWorld world)
		{
			Scenes[world.Name] = world;
		}

		public void ChangeScene(string name)
		{
			if(Scenes.TryGetValue(name, out QWorld s) && CurrentWorld != null)
			{
				CurrentWorld.OnUnload();
				CurrentWorld = s;
				CurrentWorld.OnLoad();
			}
			CallToChangeScene = false;
			CallToChangeSceneName = "";
		}

		public void ResetScene()
		{
			CurrentWorld?.OnUnload();
			CurrentWorld?.OnLoad();
			CallToResetScene = false;
		}

		internal void Update()
		{
			if(CallToChangeScene)
				ChangeScene(CallToChangeSceneName);
			else if(CallToResetScene)
				ResetScene();
			else if(CallToExitGame)
				CurrentWorld.Engine.Exit();
			CurrentWorld.OnUpdate();
		}

		internal void Draw()
		{
			CurrentWorld.OnDraw(CurrentWorld.SpriteRenderer);
		}

		internal void Gui()
		{
			CurrentWorld.OnGui(CurrentWorld.GuiRenderer);
		}

		internal void Unload()
		{
			CurrentWorld.OnUnload();
		}

		internal QWorldManager()
		{
			Scenes = new Dictionary<string, QWorld>();
		}
	}
}