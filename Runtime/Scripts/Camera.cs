using System;
using UnityEngine;

namespace Bakery
{
    public static class Cameras
    {
        public static class Events
        {
            internal static Action CallForRegistration = delegate { };
            public static Action<bool> OnCameraLocked = delegate { };

            public static Action<CameraReference> OnCameraChanged = delegate { };
            public static Action<CameraTransition> OnCameraTransitionStarted = delegate { };
        }

        public static Func<ICameraManager> Manager = UnregisterManager;

        private static ICameraManager _cachedMockManager;

        internal static ICameraManager UnregisterManager()
        {
            Debug.LogWarning("No Camera Manager registered. using a mock.");
            _cachedMockManager ??= new MockManager();
            Manager = () => _cachedMockManager;
            return _cachedMockManager;
        }

        public class MockManager : ICameraManager
        {
            public WaitUntil WaitUntilReady => new WaitUntil(() => true);

            public CameraReference CurrentCamera => null;

            public void GoToGameplay() { }

            public void RegisterCamera(CameraController controller) { }

            public void SetCamera(CameraReference reference) { }

            public void SetCamera(string name) { }

            public void SetCamera(CameraReference reference, Transform follow, Transform aim) { }

            public void UnregisterCamera(CameraController controller)
            { }
        }

        //Cleaning stuff in case cowboys are fast reloading in the editor
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            Events.CallForRegistration = delegate { };
            Events.OnCameraChanged = delegate { };
            Events.OnCameraLocked = delegate { };
            Events.OnCameraTransitionStarted = delegate { };

            Manager = UnregisterManager;

#if UNITY_EDITOR
            Debug.Log("[Flow] Static fields reset (domain reload skipped)");
#endif
        }

    }
}
