using SFML.Graphics;

namespace SFMLGameFramework.Collision;

public interface IRectangle : ICollidable
{
    public FloatRect Bounds { get; }
}