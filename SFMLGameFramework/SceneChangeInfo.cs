namespace SFMLGameFramework;

public sealed class SceneChangeInfo<T> : SceneObject
{
    private bool _sceneChanged;
    private readonly T _info;
    public string Name { get; }

    public SceneChangeInfo(T info, string name) {
        _info = info;
        Name = name;
        PersistOnSceneChange = true;
    }

    public T Extract() => _info;
    
    protected override void Initialize() => SceneManager.SceneChanged += OnSceneChange;
    public override void BeforeDestroy() => SceneManager.SceneChanged -= OnSceneChange;
    private void OnSceneChange(string lastScene, string nextScene) => _sceneChanged = true;

    public override void Update(float deltaTime) {
        // destroy this instance on the first frame after a new scene has been loaded
        if (_sceneChanged) Destroy();   
    }

    public static T Catch(string name, T defaultValue) {
        SceneChangeInfo<T>? info = SceneManager.FindByType<SceneChangeInfo<T>>();
        if (info == null) return defaultValue;
        if (info.Name != name) return defaultValue;
        return info.Extract();
    }
}