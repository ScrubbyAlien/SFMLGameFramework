using System.Reflection;
using static Framework.GameProcess;

namespace Framework;

public static class SceneManager
{
    private static string _nextScene = "";
    private static Scene _currentScene = null!;
    private static readonly List<Scene> _scenes = new();
    
    private static readonly HashSet<SceneObject> _sceneObjects = new();
    private static readonly HashSet<SceneObject> _spawnQueue = new();
    private static readonly HashSet<SceneObject> _destroyQueue = new();
    
    public static HashSet<T> AllObjects<T>() where T : SceneObject {
        return _sceneObjects.OfType<T>().ToHashSet();
    }
    internal static bool InDestroyQueue(this SceneObject o) => _destroyQueue.Contains(o);
    internal static bool InSpawnQueue(this SceneObject o) => _spawnQueue.Contains(o);
    
    internal static void Instantiate() {
        
        // get all types in assembly 
        IEnumerable<Type> sceneTypes = Assembly.GetEntryAssembly()!.GetTypes()
                                               .Where(t => t.Namespace != null && t.Namespace.Contains(GameProcess.Settings.SceneNamespace));
        // dynamically call each constructor
        foreach (Type type in sceneTypes) {
            // <>c appears when lambda expressions exist in the class definition
            // it crashes if we try to call it's constructor so we ignore it
            if (type.Name.Contains('<')) continue;
            
            // get parameterless constructor and invoke it
            Scene? scene = (Scene?)type.GetConstructor(Type.EmptyTypes)?.Invoke([]);
            if (scene != null) {
                List<string> sceneNames = _scenes.Select(s => s.Name).ToList();
                if (sceneNames.Contains(scene.Name)) { // scenes may not have dulicate names
                    Debug.Exception($"Scene with name {scene.Name} already exists.");
                }
                _scenes.Add(scene);
            }
        }

        ChangeScene(Settings.MainScene);
    }

    private static void ClearSceneForChange() {
        foreach (SceneObject sceneObject in _sceneObjects) {
            if (!sceneObject.PersistOnSceneChange) sceneObject.Destroy();
        }
        _sceneObjects.RemoveWhere(o => !o.PersistOnSceneChange);
        
        foreach (SceneObject queued in _spawnQueue) {
            if (!queued.PersistOnSceneChange) queued.Destroy();
        }
        _spawnQueue.RemoveWhere(o => !o.PersistOnSceneChange);
    }


    public static void ChangeScene(string name) {
        _nextScene = name;
    }

    private static void LoadScene(string name) {
        _currentScene = _scenes.First(scene => scene.Name == name);
        _currentScene.CreateScene();
        QueueSpawn(_currentScene.GetInitialObjects());
    }
    
    internal static void ProcessLoadScene() {
        if (_nextScene == "") return;
        ClearSceneForChange();
        LoadScene(_nextScene);
        _nextScene = "";
    }
    
    public static void QueueSpawn(SceneObject o) {
        if (o.Initialized) {
            Debug.Exception($"Instance of SceneObject {nameof(o)} already exists in Scene");
        }
        else if (!_spawnQueue.Add(o)) {
            Debug.Warning($"Instance of SceneObject {nameof(o)} is already queued to be spawned.");
        }
    }

    public static void QueueSpawn(IEnumerable<SceneObject> sceneObjects) {
        foreach (SceneObject o in sceneObjects) {
            QueueSpawn(o);
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

    internal static void ProcessSpawnQueue() {
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

    internal static void UpdateSceneObjects(float deltaTime) {
        foreach (SceneObject o in _sceneObjects) {
            if (o.Enabled) o.Update(deltaTime);
        }
    }

    internal static void ProcessDestroyQueue() {
        foreach (SceneObject o in _destroyQueue) {
            if (!_sceneObjects.Remove(o)) {
                Debug.Warning("Instance of SceneObject to be destroyed does not exist in scene");
                continue;
            }
            o.Destroy();
        }
        _destroyQueue.Clear();
    }
}