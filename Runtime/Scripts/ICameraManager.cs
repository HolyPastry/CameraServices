using UnityEngine;

namespace Bakery
{
    public interface ICameraManager
    {
        WaitUntil WaitUntilReady { get; }
        void SetCamera(CameraReference reference);
        void SetCamera(string name);
        void SetCamera(CameraReference reference, Transform follow, Transform aim);
        void GoToGameplay();

        void RegisterCamera(CameraController controller);
        void UnregisterCamera(CameraController controller);

        CameraReference CurrentCamera { get; }
    }
}
