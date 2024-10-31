using SFML.System;

namespace Framework.Collision;

public interface ICircle : ICollidable
{
    public Vector2f Origin { get; }
    public float Radius { get; }
}