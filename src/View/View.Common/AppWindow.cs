using System.Diagnostics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using ImGuiNET;
using App.Models;

namespace App.View;

public class AppWindow
{
#pragma warning disable 8618
    IWindow window;
    IInputContext inputContext;
    GL gl;
    ImGuiController imGuiController;
    ScreenNavigator screenNavigator;

    ImFontPtr FontRoboto { get; set; }
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

        void OnWindowLoad()
        {
            inputContext = window.CreateInput();
            gl = window.CreateOpenGL();
            imGuiController = new ImGuiController(gl, window, inputContext);
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;

            var dependencyContainer = new DependencyContainer();
            this.screenNavigator = dependencyContainer.GetScreenNavigator();

            // var matchFactory = new MatchFactory();
            // var match = matchFactory.CreateDefault();
            // var ruleEngine = new RuleEngine(match);
            // this.screenNavigator.Push(new GameInput(ruleEngine));

            var playerRepository = dependencyContainer.GetPlayerRepository();

            // var names = new List<string>() {
            //     "Dennis Doosjes",
            //     "Bart Bakker"
            // };

            // var names = new List<string>() {
            //     "Jan de barman",
            //     "Martin Makkermaat"
            // };
            // foreach (var name in names)
            // {
            //     var playerA = new Player();
            //     playerA.Id = Guid.NewGuid();
            //     playerA.FullName = name;
            //     playerA.PlayedGames = new List<Guid>();
            //     playerA.WonGames = new List<Guid>();
            //     playerA.LostGames = new List<Guid>();
            //     playerA.Statistic = new PlayerStatistic()
            //     {
            //         AverageTurnScore = 0,
            //         Ninedarters = 0,
            //         OneEighties = 0,
            //     };

            //     playerRepository.Save(playerA);
            // }
            // var playerA = new Player();
            // playerA.Id = Guid.NewGuid();
            // playerA.FullName = "Jane Doe";
            // playerA.PlayedGames = new List<Guid>();
            // playerA.Lossess = 0;
            // playerA.Wins = 0;


            // var playerB = new Player();
            // playerB.Id = Guid.NewGuid();
            // playerB.FullName = "Dennis Dropjes";
            // playerB.PlayedGames = new List<Guid>();
            // playerB.Lossess = 0;
            // playerB.Wins = 0;

            // dependencyContainer.GetPlayerRepository().Save(playerB);

            this.screenNavigator.Push(dependencyContainer.MakeMainScreen());
        }

        void OnWindowUpdate(double dt)
        {
            imGuiController.Update((float)dt);

            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());

            if (ImGui.Begin("Test", ImGuiWindowFlags.NoTitleBar))
            {
                var topScreen = this.screenNavigator.GetTopScreen();
                topScreen.Update();
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