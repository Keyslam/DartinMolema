using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using ImGuiNET;

namespace App.View;

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
		windowOptions.Size = new Vector2D<int>(1920, 1080);
		windowOptions.Title = "Dartin Molema";

		window = Window.Create(windowOptions);
		window.Load += OnWindowLoad;
		window.Update += OnWindowUpdate;
		window.Render += OnWindowRender;

		window.Run();
	}

	private void OnWindowLoad()
	{
		inputContext = window.CreateInput();
		gl = window.CreateOpenGL();
		imGuiController = new ImGuiController(gl, window, inputContext);
		ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
		ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;

		var dependencyContainer = new DependencyContainer();
		this.screenNavigator = dependencyContainer.GetScreenNavigator();

		this.screenNavigator.Push(dependencyContainer.MakeMainScreen());
	}

	private void OnWindowUpdate(double dt)
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
			topScreen.Update();
		}
	}

	private void OnWindowRender(double dt)
	{
		gl.Clear(ClearBufferMask.ColorBufferBit);

		imGuiController.Render();
	}

	private void Quit()
	{
		window.Close();
	}
}