using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;

namespace ludumdare56;

/// <summary>
/// Extension methods for use with an <see cref="Camera3D.ProjectionType.Orthogonal"/> camera. 
/// </summary>
public static class OrthogonalCameraExtensions
{
    public static float MetersToPixelsRatio(this Camera3D camera3D)
    {
        if (camera3D.Projection is not Camera3D.ProjectionType.Orthogonal)
        {
            throw new ArgumentException("Must be an Orthogonal camera", nameof(camera3D));
        }

        var screenMeters = camera3D.Size;
        var screenPixels = camera3D.KeepAspect switch
        {
            Camera3D.KeepAspectEnum.Height => camera3D.GetWindow().Size.Y,
            Camera3D.KeepAspectEnum.Width => camera3D.GetWindow().Size.X,
            _ => throw new InvalidEnumArgumentException()
        };
        
        // screenPix / screenMet = x / meters
        // x = (screenPix / screenMet) * meters
        return screenPixels / screenMeters;
    }

    public static float PixelsToMetersRatio(this Camera3D camera3D) => 1 / MetersToPixelsRatio(camera3D);
    
    public static Rect2 GetScreenInMeters(this Camera3D camera3D)
    {
        var pixelsToMetersRatio = 1 / MetersToPixelsRatio(camera3D);
        var screenSizeMeters = (Vector2)camera3D.GetWindow().Size * pixelsToMetersRatio;
        var center = camera3D.Position.WithoutZ();
        var bottomLeft = center - (screenSizeMeters / 2);
        return new Rect2(bottomLeft, screenSizeMeters);
    }

    public static Vector2 ScreenToWorldPosition(this Camera3D camera3D, Vector2 pixelsFromTopLeft)
    {
        var pixelsToMeters = camera3D.PixelsToMetersRatio();
        var metersFromTopLeft = pixelsFromTopLeft * pixelsToMeters;
        var screenRect = GetScreenInMeters(camera3D);
        return new Vector2(
            screenRect.Left() + metersFromTopLeft.X,
            screenRect.Top() - metersFromTopLeft.Y
        );
    }
}