using System;
using Holypastry.Bakery.Cameras;
using UnityEngine;

public static partial class CameraServices
{

    public static Action<bool> ToggleCameraControl = delegate { };
    public static Func<float, Vector3> GetCameraTargetPosition = (distanceInFront) => Vector3.zero;
    public static Action<CameraReference, Transform, Transform> SetCameraWithTarget = delegate { };
    public static Action<CameraReference> SetCamera = delegate { };
    public static Action GoToGameplay = delegate { };
    public static Action<CameraController> RegisterCamera = delegate { };
    public static Action<CameraController> UnregisterCamera = delegate { };
    public static Func<WaitUntil> WaitUntilReady = () => new WaitUntil(() => true);
    public static Action<string> SetCameraByName = (cameraName) => { };
}
