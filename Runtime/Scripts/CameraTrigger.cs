
using UnityEngine;

namespace Bakery
{
    [RequireComponent(typeof(Collider))]
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private CameraReference _cameraReference;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            Cameras.Manager().SetCamera(_cameraReference, null, null);
        }
    }
}
