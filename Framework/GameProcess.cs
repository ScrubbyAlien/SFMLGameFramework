using System.Text.Json;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Framework;

public static class GameProcess
{
    // public static event Action? GlobalEvents;
    
    public static void SetDebugMode(bool on) => Debugging = on;
    
    #region StartGameWindow
    
        private static RenderWindow? _window;
        public static bool Debugging { get; private set; }
        public static ProjectSettings Settings { get; private set; } = new();

        /// <summary>
        /// Start the game using a configuaration file called projectsettings.json
        /// located at configPath relative to the project root directory.
        /// </summary>
        /// <param name="configPath">the relative path to the directory containing projectsettings.json</param>
        public static void StartGame(string configPath = "config") {
            ProjectSettings ps;
            if (configPath != "") {
                string json = File.ReadAllText(configPath + "/projectsettings.json");
                ps = JsonSerializer.Deserialize<ProjectSettings>(json) ?? new ProjectSettings();
            }
            else {
                ps = new ProjectSettings();
            }
            
            StartGame(ps);
        }

        /// <summary>
        /// Start the game using an instance of the ProjectSettings class.
        /// </summary>
        /// <param name="projectSettings">The ProjectSettings instance containing the paramaters to start the game with</param>
        public static void StartGame(ProjectSettings projectSettings) {
            Settings = projectSettings ?? new();
            OpenWindow(Settings.ScreenWidth, Settings.ScreenHeight, Settings.FrameLimit);
        }

        private static void OpenWindow(uint width, uint height, uint frameLimit) {
            _window = new(new VideoMode(width, height), "Invaders");
            // ReSharper disable once AccessToDisposedClosure
            _window.Closed += (_, _) => _window.Close();
            _window.SetFramerateLimit(frameLimit);

            Clock clock = new Clock();
            SceneManager.Instantiate();
            
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

    #endregion

    #region Loop
    
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
                if (r.Enabled || !r.Hidden) r.Render(target);
            }
        }

    #endregion
}