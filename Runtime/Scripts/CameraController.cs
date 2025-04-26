
using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Holypastry.Bakery.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraController : ValidatedMonoBehaviour
    {
        [SerializeField] private CameraReference _cameraReference;
        [SerializeField, Self] private CinemachineVirtualCamera _camera;
        [SerializeField] private bool _generateOwnReference = false;


        public CameraReference CameraReference
        {
            get => _cameraReference;
            set => _cameraReference = value;
        }

        private CinemachineInputProvider _inputProvider;

        void Awake()
        {
            _camera.Priority = CameraManager.LOW_PRIORITY;
            if (!_generateOwnReference) return;
            GenerateSelfReference(name);
        }

        void Start()
        {
            CameraServices.RegisterCamera(this);
        }

        void OnDestroy()
        {
            CameraServices.UnregisterCamera(this);
        }

        public void DisableControls()
        {
            if (_inputProvider == null)
                _inputProvider = GetComponent<CinemachineInputProvider>();
            if (_inputProvider != null)
                _inputProvider.enabled = false;
        }

        public void EnableControls()
        {
            if (_inputProvider == null)
                _inputProvider = GetComponent<CinemachineInputProvider>();
            if (_inputProvider != null)
                _inputProvider.enabled = true;
        }

        public void Activate()
        {
            _camera.gameObject.SetActive(true);
            _camera.Priority = CameraManager.HIGH_PRIORITY;
        }

        public bool Activate(Transform target, Transform aimTarget)
        {

            if (target != null)
                _camera.Follow = target;

            if (aimTarget != null)
                _camera.LookAt = aimTarget;
            Activate();

            return true;
        }

        public void Deactivate()
        {
            _camera.Priority = CameraManager.LOW_PRIORITY;
            _camera.gameObject.SetActive(false);
        }

        public void SetBrainActive(bool isOn)
        {
            _camera.enabled = isOn;
        }

        public void GenerateSelfReference(string referenceName)
        {
            _cameraReference = ScriptableObject.CreateInstance<CameraReference>();
            _cameraReference.name = referenceName;
        }
    }
}
