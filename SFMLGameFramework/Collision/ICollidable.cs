namespace SFMLGameFramework.Collision;

public interface ICollidable
{
    public List<string> Layers { get; }
    public bool OnLayer(string layer) {
        return Layers.Contains(layer);
    }
}