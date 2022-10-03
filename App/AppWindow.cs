using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using ImGuiNET;
using App.Screens;

namespace App;

public class AppWindow
{
#pragma warning disable 8618
	IWindow window;
	IInputContext inputContext;
	GL gl;
	ImGuiController imGuiController;
	ScreenNavigator screenNavigator;
#pragma warning restore 8618

	public void Run()
	{
		var windowOptions = WindowOptions.Default;
		windowOptions.Size = new Vector2D<int>(1280, 720);
		windowOptions.Title = "DartApp";

		window = Window.Create(windowOptions);
		window.Load += OnWindowLoad;
		window.Update += OnWindowUpdate;
		window.Render += OnWindowRender;

		window.Run();

		void OnWindowLoad()
		{
			inputContext = window.CreateInput();
			gl = window.CreateOpenGL();
			imGuiController = new ImGuiController(gl, window, inputContext);
			ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
			ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;

			this.screenNavigator = new ScreenNavigator();

			// var matchFactory = new MatchFactory();
			// var match = matchFactory.CreateDefault();
			// var ruleEngine = new RuleEngine(match);
			// this.screenNavigator.Push(new GameInput(ruleEngine));

			// var playerRepository = new PlayerRepository();

			// var playerA = new Player();
			// playerA.Id = Guid.NewGuid();
			// playerA.FullName = "Jane Doe";
			// playerA.PlayedGames = new List<Guid>();
			// playerA.Lossess = 0;
			// playerA.Wins = 0;

			// playerRepository.Save(playerA);

			// var playerB = new Player();
			// playerB.Id = Guid.NewGuid();
			// playerB.FullName = "John Doe";
			// playerB.PlayedGames = new List<Guid>();
			// playerB.Lossess = 0;
			// playerB.Wins = 0;

			// playerRepository.Save(playerB);

			this.screenNavigator.Push(new MainScreen());
		}

		void OnWindowUpdate(double dt)
		{
			imGuiController.Update((float)dt);

			ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());

			if (ImGui.Begin("Test", ImGuiWindowFlags.NoTitleBar))
			{
				ImGui.Text("Dartin Molema");
				ImGui.Spacing();
				ImGui.Separator();
				ImGui.Spacing();

				var topScreen = this.screenNavigator.GetTopScreen();
				topScreen.Update(this.screenNavigator);
			}
		}


		void OnWindowRender(double dt)
		{
			gl.Clear(ClearBufferMask.ColorBufferBit);

			imGuiController.Render();
		}
	}

	public void Quit()
	{
		window.Close();
	}
}