using System;
using Holypastry.Bakery.Cameras;


public static class CameraEvents
{
    public static Action<bool> OnCameraLocked = delegate { };

    public static Action<CameraReference> OnCameraChanged = delegate { };
    public static Action<CameraTransition> OnCameraTransitionStarted = delegate { };
}

