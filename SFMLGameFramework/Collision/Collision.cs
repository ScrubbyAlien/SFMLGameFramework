using SFMLGameFramework.System;
using SFML.Graphics;
using SFML.System;

namespace SFMLGameFramework.Collision;

public static class Collision
{
    // RectangleRectangle
    public static bool Intersects(this IRectangle rectangle1, IRectangle rectangle2) {
        return rectangle1.Bounds.Intersects(rectangle2.Bounds);
    }
    public static bool Intersects(this IRectangle rectangle1, IRectangle rectangle2, string layer) {
        return rectangle2.OnLayer(layer) && rectangle1.Bounds.Intersects(rectangle2.Bounds);
    }
    public static bool Intersects(this IRectangle rectangle1, IRectangle rectangle2, out Vector2f diff) {
        bool intersects = rectangle1.Intersects(rectangle2);
        diff = new Vector2f();
        if (intersects) {
            FloatRect rect1 = rectangle1.Bounds;
            FloatRect rect2 = rectangle2.Bounds;
            diff = new Vector2f(rect2.Left - rect1.Left, rect2.Top - rect1.Top);
        }
        return intersects;
    }
    public static bool Intersects(this IRectangle rectangle1, IRectangle rectangle2, string layer, out Vector2f diff) {
        diff = new Vector2f();
        return rectangle2.OnLayer(layer) && rectangle1.Intersects(rectangle2, out diff);
    }
    
    
    // CircleCircle
    public static bool Intersects(this ICircle circle1, ICircle circle2) {
        return (circle1.Origin - circle2.Origin).Length() <= circle1.Radius + circle2.Radius;
    }
    public static bool Intersects(this ICircle circle1, ICircle circle2, string layer) {
        return circle2.OnLayer(layer) && circle1.Intersects(circle2);
    }
    public static bool Intersects(this ICircle circle1, ICircle circle2, out Vector2f diff) {
        bool intersects = circle1.Intersects(circle2);
        diff = new Vector2f();
        if (intersects) {
            Vector2f difference = circle1.Origin - circle2.Origin;
            float length = circle1.Radius + circle2.Radius - difference.Length();
            diff = difference.Normalized() * length;
        }
        return intersects;
    }
    public static bool Intersects(this ICircle circle1, ICircle circle2, string layer, out Vector2f diff) {
        diff = new Vector2f();
        return circle2.OnLayer(layer) && circle1.Intersects(circle2, out diff);
    }
}