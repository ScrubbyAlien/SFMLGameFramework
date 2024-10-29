namespace Framework;

public abstract class SceneObject
{
    private readonly HashSet<string> _tags = new();
    public bool Dead = false;
    public bool DontDestroyOnClear = false;
    public bool Initialized { get; private set; }
    public bool Paused { get; private set; }

    /// <summary>
    /// Any functionality that requires references to other entities should be called from Initialize,
    /// such as FindByType calls, or event handlers/listerners
    /// </summary>
    protected virtual void Initialize() { }

    public void FullInitialize() {
        Initialize();
        Initialized = true;
    }

    public virtual void Destroy() { }

    public virtual void Update(float deltaTime) { }

    public void AddTag(string tag) => _tags.Add(tag);
    public bool HasTag(string tag) => _tags.Contains(tag);
    public virtual void Pause() => Paused = true;
    public virtual void Unpause() => Paused = false;
}