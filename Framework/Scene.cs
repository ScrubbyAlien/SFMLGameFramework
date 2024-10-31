namespace Framework;

public abstract class Scene(string name)
{
    public readonly string Name = name;
    private readonly List<SceneObject> _initialObjects = new();

    public List<SceneObject> GetInitialObjects() => _initialObjects;

    protected void AddObject(SceneObject o) => _initialObjects.Add(o);

    public void CreateScene() {
        ClearObjects();
        LoadObjects();
    }

    protected abstract void LoadObjects();

    private void ClearObjects() => _initialObjects.Clear();
}