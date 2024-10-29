using System.Text.Json;
using SFML.Graphics;
using SFML.Window;

namespace Framework;

public static class Game
{
    private static readonly List<SceneObject> _sceneObjects = new();
    private static readonly List<SceneObject> _spawnQueue = new();
    private static readonly List<SceneObject> _destroyQueue = new();
    
    private static RenderWindow? _window;
    public static bool Debugging { get; private set; }

    public static void StartGame(string configPath, bool debug = false) {
        Debugging = debug;
        
        ProjectSettings gs;
        if (configPath != "") {
            string json = File.ReadAllText(configPath + "/projectsettings.json");
            gs = JsonSerializer.Deserialize<ProjectSettings>(json) ?? new ProjectSettings();
        }
        else {
            gs = new ProjectSettings();
        }
        
        Debug.Write("debugging");
        
        OpenWindow(gs.ScreenWidth, gs.ScreenHeight, gs.FrameLimit);
    }

    public static void StartGame(bool debug = false) {
        StartGame("config", debug);
    }

    private static void OpenWindow(uint width, uint height, uint frameLimit) {
        _window = new(new VideoMode(width, height), "Invaders");
        // ReSharper disable once AccessToDisposedClosure
        _window.Closed += (_, _) => _window.Close();
        _window.SetFramerateLimit(frameLimit);

        while (_window.IsOpen) {
            _window.DispatchEvents();
            // float deltaTime = clock.Restart().AsSeconds();
            // Scene.UpdateAll(deltaTime);
            //
            // window.Clear();
            // Scene.RenderAll(window);
            //
            // window.Display();
        }
        _window.Dispose();
    }
}