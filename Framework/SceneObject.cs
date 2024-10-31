namespace Framework;

public abstract class SceneObject
{
    private readonly HashSet<string> _tags = new();
    public bool PersistOnSceneChange { get; protected set; } = false;
    public bool Initialized { get; private set; }
    public bool Paused { get; private set; }
    public bool Enabled { get; private set; }
    
    protected virtual void Initialize() { }

    public void FullInitialize() {
        Initialize();
        Initialized = true;
        Enabled = true;
    }

    public virtual void BeforeDestroy() { }

    public virtual void Update(float deltaTime) { }

    public void AddTag(string tag) => _tags.Add(tag);
    public bool HasTag(string tag) => _tags.Contains(tag);
    public void Pause() => Paused = true;
    public void Unpause() => Paused = false;
    public void Enable() => Enabled = true;
    public void Disable() => Enabled = false;
    public bool ToBeDestroyed() => this.InDestroyQueue();
    public bool ToBeSpawned() => this.InSpawnQueue();
    public void Spawn() => SceneManager.QueueSpawn(this);
    public void Destroy() => SceneManager.QueueDestroy(this);
}