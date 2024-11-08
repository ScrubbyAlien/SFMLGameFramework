using System.Text.Json;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLGameFramework;

public static class Game
{
    // public static event Action? GlobalEvents;
    private static RenderWindow? _window;
    public static bool Debugging { get; private set; }
    internal static ProjectSettings ProjectSettings { get; private set; } = new();

    public static void SetDebugMode(bool on) {
        Debugging = on;
    }

    /// <summary>
    /// Start the game using a configuaration file called projectsettings.json
    /// located at configPath relative to the project root directory.
    /// </summary>
    /// <param name="configPath">the relative path to the directory containing projectsettings.json</param>
    public static void Start(string configPath = "config") {
        ProjectSettings ps;
        if (configPath != "") {
            string json = File.ReadAllText(configPath + "/projectsettings.json");
            ps = JsonSerializer.Deserialize<ProjectSettings>(json) ?? new ProjectSettings();
        }
        else {
            ps = new ProjectSettings();
        }
        Start(ps);
    }

    /// <summary>
    /// Start the game using an instance of the ProjectSettings class.
    /// </summary>
    /// <param name="projectSettings">The ProjectSettings instance containing the paramaters to start the game with</param>
    public static void Start(ProjectSettings projectSettings) {
        ProjectSettings = projectSettings;
        OpenWindow(ProjectSettings.ScreenWidth, ProjectSettings.ScreenHeight, ProjectSettings.FrameLimit);
    }

    private static void OpenWindow(uint width, uint height, uint frameLimit) {
        _window = new RenderWindow(new VideoMode(width, height), "Invaders");
        // ReSharper disable once AccessToDisposedClosure
        _window.Closed += (_, _) => _window.Close();
        _window.SetFramerateLimit(frameLimit);

        Clock clock = new();
        SceneManager.Instantiate();
        AssetManager.PreLoadAssets(ProjectSettings.PreLoadedAssets);

        while (_window.IsOpen) {
            _window.DispatchEvents();
            float deltaTime = clock.Restart().AsSeconds();
            UpdateAll(deltaTime);

            _window.Clear();
            RenderAll(_window);

            _window.Display();
        }

        _window.Dispose();
    }

    private static void UpdateAll(float deltaTime) {
        SceneManager.ProcessLoadScene();
        SceneManager.ProcessDestroyQueue();
        SceneManager.ProcessSpawnQueue();
        // ProcessDeferredCalls();
        SceneManager.UpdateSceneObjects(deltaTime);
        // GlobalEvents?.Invoke();
    }
    private static void RenderAll(RenderTarget target) {
        List<RenderObject> renderObjects = SceneManager.AllObjects<RenderObject>().ToList();
        renderObjects.Sort(RenderObject.CompareByZIndex);
        foreach (RenderObject r in renderObjects) {
            if (r.Enabled || !r.Visible) r.Render(target);
        }
    }
}