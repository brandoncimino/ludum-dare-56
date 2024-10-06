using Godot;

namespace ludumdare56;

/// <summary>
/// Some helper methods for dealing with Godot's mathy types like <see cref="Vector2"/> and <see cref="Rect2"/>.
/// </summary>
public static class GodotExtensions
{
    public static Vector2 WithoutZ(this Vector3 vector3) => new(vector3.X, vector3.Y);
    public static Vector3 WithZ(this Vector2 vector2, float z) => new(vector2.X, vector2.Y, z);

    public static float Left(this Rect2 rect2) => rect2.Position.X;
    public static float Right(this Rect2 rect2) => rect2.End.X;
    public static float Top(this Rect2 rect2) => rect2.End.Y;
    public static float Bottom(this Rect2 rect2) => rect2.Position.Y;

    public static float Height(this Rect2 rect2) => rect2.Size.Y;
    public static float Width(this Rect2 rect2) => rect2.Size.X;

    public static Vector2 TopLeft(this Rect2 rect2) => new(rect2.Left(), rect2.Top());
    public static Vector2 BottomLeft(this Rect2 rect2) => rect2.Position;
    public static Vector2 BottomRight(this Rect2 rect2) => new(rect2.Right(), rect2.Bottom());
    public static Vector2 TopRight(this Rect2 rect2) => rect2.End;
}