namespace Framework;

public abstract class SceneObject
{
    private readonly HashSet<string> _tags = new();
    public bool Dead = false;
    public bool PersistOnClear = false;
    public bool Initialized { get; private set; }
    public bool Paused { get; private set; }
    public bool Enabled { get; private set; }

    /// <summary>
    /// Any functionality that requires references to other entities should be called from Initialize,
    /// such as FindByType calls, or event handlers/listerners
    /// </summary>
    protected virtual void Initialize() { }

    public void FullInitialize() {
        Initialize();
        Initialized = true;
        Enabled = true;
    }

    public virtual void Destroy() { }

    public virtual void Update(float deltaTime) { }

    public void AddTag(string tag) => _tags.Add(tag);
    public bool HasTag(string tag) => _tags.Contains(tag);
    public void Pause() => Paused = true;
    public void Unpause() => Paused = false;
    public void Enable() => Enabled = true;
    public void Disable() => Enabled = false;
}