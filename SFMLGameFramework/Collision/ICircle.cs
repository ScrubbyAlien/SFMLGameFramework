using SFML.System;

namespace SFMLGameFramework.Collision;

public interface ICircle : ICollidable
{
    public Vector2f Origin { get; }
    public float Radius { get; }
}