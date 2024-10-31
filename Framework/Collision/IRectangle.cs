using SFML.Graphics;

namespace Framework.Collision;

public interface IRectangle : ICollidable
{
    public FloatRect Bounds { get; }
}