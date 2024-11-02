using SFML.System;

namespace SFMLGameFramework.System;

public static class Vector2fExtensions
{
    // Vector2f extension methods borrowed from Collision class from lab project breakout
    public static Vector2f Normalized(this Vector2f v) {
        if (v.Length() == 0) return new Vector2f(0, 0);
        return v / v.Length();
    }

    public static float Length(this Vector2f v) {
        return MathF.Sqrt(v.Dot(v));
    }

    public static float Dot(this Vector2f a, Vector2f b) {
        return a.X * b.X + a.Y * b.Y;
    }
}