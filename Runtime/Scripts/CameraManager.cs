using System;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;
using Holypastry.Bakery.Flow;
using UnityEngine;
using UnityEngine.Rendering;

namespace Holypastry.Bakery.Cameras
{

    public class CameraManager : Service
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private DataCollection<CameraTransition> _cameraTransitions;
        [SerializeField] private CameraReference _gameplayCamera;
        [SerializeField] private CameraReference _startCamera;

        [SerializeField] private bool _debugMode = false;

        private readonly List<CameraController> _cameraControllers = new();

        private Camera _camera;
        private CameraController _currentController;
        private CameraReference _currentReference;
        private CinemachineBlendDefinition _defaultBlend;

        internal static int HIGH_PRIORITY = 11;
        internal static int LOW_PRIORITY = 9;
        private bool _cameraIsLocked;

        void Awake()
        {
            _camera = Camera.main;
            _cameraTransitions = new DataCollection<CameraTransition>("CameraTransitions");
            _defaultBlend = _cinemachineBrain.m_DefaultBlend;
        }

        void OnDisable()
        {
            CameraServices.WaitUntilReady = () => WaitUntilReady;

            CameraServices.ToggleCameraControl = delegate { };
            CameraServices.GetCameraTargetPosition = delegate { return Vector3.zero; };

            CameraServices.SetCameraWithTarget = delegate { };
            CameraServices.SetCameraByName = delegate { };
            CameraServices.SetCamera = delegate { };
            CameraServices.GoToGameplay = delegate { };

            CameraServices.RegisterCamera = (cameraName) => { };
            CameraServices.UnregisterCamera = delegate { };

            CameraServices.GetCurrentCamera = () => null;

        }

        void OnEnable()
        {
            CameraServices.WaitUntilReady = () => new WaitUntil(() => _isReady);

            CameraServices.ToggleCameraControl = ToggleCameraControl;
            CameraServices.GetCameraTargetPosition = (distanceInFront) => _camera.transform.position + _camera.transform.forward * distanceInFront;

            CameraServices.SetCameraWithTarget = SetCamera;
            CameraServices.SetCameraByName = SetCamera;
            CameraServices.SetCamera = SetCamera;
            CameraServices.GoToGameplay = GoToGameplay;

            CameraServices.RegisterCamera = RegisterCamera;
            CameraServices.UnregisterCamera = UnregisterCamera;

            CameraServices.GetCurrentCamera = () => _currentReference;

        }

        protected override IEnumerator Start()
        {
            yield return FlowServices.WaitUntilReady();

            if (_startCamera != null)
                SetCamera(_startCamera, null, null);
            _isReady = true;

        }

        private void DebugLog(string message)
        {
            if (_debugMode) Debug.Log(message);
        }


        private void UnregisterCamera(CameraController controller)
        {
            _cameraControllers.Remove(controller);
        }

        private void RegisterCamera(CameraController controller)
        {
            if (!_cameraControllers.Contains(controller))
                _cameraControllers.Add(controller);
            if (controller != _currentController)
                controller.gameObject.SetActive(false);
        }

        private void SetCamera(string name)
        {
            var controller = _cameraControllers.Find(c => c.CameraReference.name == name);
            if (controller == null)
            {
                DebugLog("No camera found with name " + name);
                return;
            }
            SetCamera(controller.CameraReference, null, null);

        }
        private void GoToGameplay() => SetCamera(_gameplayCamera, null, null);

        private void SetCamera(CameraReference reference) => SetCamera(reference, null, null);
        private void SetCamera(CameraReference newReference, Transform follow, Transform aim)
        {
            if (newReference == null)
            {
                DebugLog("No camera reference provided");
                return;
            }

            if (_cameraIsLocked)
            {
                DebugLog("Camera is locked " + newReference.name);
                return;
            }
            if (_currentReference == newReference)
            {
                DebugLog("Camera is already set to " + newReference.name);
                return;
            }

            if (!TryAndGetController(newReference, out var nextCamera))
            {
                DebugLog("No controller found with name " + newReference.name);
                return;
            }

            DebugLog("Switching to camera: " + newReference.name);

            CameraEvents.OnCameraChanged.Invoke(newReference);
            if (newReference.GameplayCamera)
                RecenterCamera(nextCamera);


            if (_currentController != null)
                _currentController.Deactivate();
            SetCameraTransition(newReference);
            _currentReference = newReference;

            nextCamera.Activate(follow, aim);

            _currentController = nextCamera;
            SetCameraLock();
        }

        private void SetCameraTransition(CameraReference newReference)
        {
            var transition = _cameraTransitions.Find(transition =>
                 transition.CameraIn == _currentReference &&
                transition.CameraOut == newReference);

            if (transition == null)
                _cinemachineBrain.m_DefaultBlend = _defaultBlend;
            else
                _cinemachineBrain.m_DefaultBlend = transition.Blend;
            CameraEvents.OnCameraTransitionStarted.Invoke(transition);
        }


        private void RecenterCamera(CameraController nextCamera)
        {
            nextCamera.SetBrainActive(false);
            nextCamera.transform.SetPositionAndRotation(
                _camera.transform.position,
                 _camera.transform.rotation);
            nextCamera.SetBrainActive(true);
        }



        private bool TryAndGetController(CameraReference reference, out CameraController cameraController)
        {
            cameraController = _cameraControllers.Find(controller => controller.CameraReference == reference);
            if (cameraController == null)
                Debug.LogWarning("No Camera Controller found for " + reference.name);
            return cameraController != null;
        }

        private void ToggleCameraControl(bool isOn)
        {
            //_cinemachineBrain.enabled = IsOn;
            _cameraIsLocked = !isOn;
            SetCameraLock();

            CameraEvents.OnCameraLocked.Invoke(!isOn);
        }

        private void SetCameraLock()
        {
            if (_currentController == null) return;
            if (_cameraIsLocked)
                _currentController.DisableControls();
            else
                _currentController.EnableControls();
        }
    }
}
