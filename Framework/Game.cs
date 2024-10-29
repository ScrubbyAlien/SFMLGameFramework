using System.Text.Json;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Framework;

public static class Game
{
    public static event Action? GlobalEvents;

    #region StartGameWindow
    
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

            Clock clock = new Clock();
            
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

    #region SceneObjects
    
        private static void UpdateAll(float deltaTime) {
            // ProcessLoadLevel();
            ProcessSpawnQueue();
            // ProcessDeferredCalls();
            UpdateSceneObjects(deltaTime);
            GlobalEvents?.Invoke();
            ProcessDestroyQueue();
        }
        
        private static readonly HashSet<SceneObject> _sceneObjects = new();
        private static readonly HashSet<SceneObject> _spawnQueue = new();
        private static readonly HashSet<SceneObject> _destroyQueue = new();

        public static void QueueSpawn(SceneObject o) {
            if (o.Initialized) {
                Debug.Exception("Instance of SceneObject already exists in Scene");
            }
            else if (!_spawnQueue.Add(o)) {
                Debug.Warning("Instance of SceneObject is already queued to be spawned.");
            }
        }

        public static void QueueDestroy(SceneObject o) {
            if (!_sceneObjects.Contains(o) && !_spawnQueue.Contains(o)) { // destroyed before it's ever been spawned
                Debug.Exception("Instance of SceneObject does not exist in the scene.");
            } 
            else if (_spawnQueue.Remove(o)) { } // false if it is not queued to spawn
            else if (!_destroyQueue.Add(o)) {
                Debug.Warning("Instance of SceneObject is already queued to be destroyed.");
            }
        }

        private static void ProcessSpawnQueue() {
            foreach (SceneObject q in _spawnQueue) {
                if (!_sceneObjects.Add(q)) {
                    Debug.Exception("Instance of queued SceneObject already exists in scene.");
                }
            }
            _spawnQueue.Clear();

            foreach (SceneObject o in _sceneObjects) {
                if (!o.Initialized) o.FullInitialize();
            }
        }

        private static void UpdateSceneObjects(float deltaTime) {
            foreach (SceneObject o in _sceneObjects) {
                if (o.Enabled) o.Update(deltaTime);
            }
        }

        private static void ProcessDestroyQueue() {
            foreach (SceneObject o in _destroyQueue) {
                if (!_sceneObjects.Remove(o)) {
                    Debug.Warning("Instance of SceneObject to be destroyed does not exist in scene");
                    continue;
                }
                o.Dead = true;
                o.Destroy();
            }
            _destroyQueue.Clear();
        }

    #endregion

    #region RenderObjects

    private static void RenderAll(RenderTarget target) {
        List<RenderObject> renderObjects = _sceneObjects.OfType<RenderObject>().ToList();
        renderObjects.Sort(RenderObject.CompareByZIndex);
        foreach (RenderObject r in renderObjects) {
            if (r.Enabled || !r.Hidden) r.Render(target);
        }
    }

    #endregion
}