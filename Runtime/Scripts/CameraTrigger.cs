
using UnityEngine;

namespace Holypastry.Bakery.Cameras
{
    [RequireComponent(typeof(Collider))]
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private bool _lockCamera = false;
        [SerializeField] private CameraReference _cameraReference;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            CameraServices.ToggleCameraControl(true);
            CameraServices.SetCameraWithTarget(_cameraReference, null, null);
            CameraServices.ToggleCameraControl(!_lockCamera);
        }
    }
}
