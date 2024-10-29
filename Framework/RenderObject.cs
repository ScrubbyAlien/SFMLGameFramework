using SFML.Graphics;

namespace Framework;

public abstract class RenderObject : SceneObject
{
    protected abstract Drawable GetDrawable();
    
    public virtual void Render(RenderTarget target) {
        if (!Hidden) target.Draw(GetDrawable());
    }
    
    public bool Hidden { get; private set; }
    public void Hide() => Hidden = true;
    public void Unhide() => Hidden = false;

    private readonly HashSet<string> _layer = new();
    public void AddLayer(string layer) => this._layer.Add(layer);
    public bool OnLayer(string layer) => this._layer.Contains(layer);

    private int zIndex;
    public void SetZIndex(int index) => zIndex = index;
    public static int CompareByZIndex(RenderObject? r1, RenderObject? r2) {
        if (r1 == null && r2 == null) return 0;
        if (r1 == null) return -1;
        if (r2 == null) return 1;
        return r1.zIndex - r2.zIndex;
    }
}