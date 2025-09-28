
using System;
using Cinemachine;
using UnityEngine;

namespace Holypastry.Bakery.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CameraReference _cameraReference;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private bool _generateOwnReference = false;

        public CinemachineVirtualCamera Camera
        {
            get
            {
                if (_camera == null)
                    _camera = GetComponent<CinemachineVirtualCamera>();
                return _camera;
            }
        }

        public CameraReference CameraReference
        {
            get => _cameraReference;
            set => _cameraReference = value;
        }

        private CinemachineInputProvider _inputProvider;

        void Awake()
        {

            Camera.Priority = CameraManager.LOW_PRIORITY;
            if (!_generateOwnReference) return;
            GenerateSelfReference();
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
            Camera.gameObject.SetActive(true);
            Camera.Priority = CameraManager.HIGH_PRIORITY;
        }

        public bool Activate(Transform target, Transform aimTarget)
        {

            if (target != null)
                Camera.Follow = target;

            if (aimTarget != null)
                Camera.LookAt = aimTarget;
            Activate();

            return true;
        }

        public void Deactivate()
        {
            Camera.Priority = CameraManager.LOW_PRIORITY;
            Camera.gameObject.SetActive(false);
        }

        public void SetBrainActive(bool isOn)
        {
            Camera.enabled = isOn;
        }

        public void GenerateSelfReference()
        {
            _cameraReference = ScriptableObject.CreateInstance<CameraReference>();
            _cameraReference.name = $"{Guid.NewGuid()}_CameraReference";
        }
    }
}
