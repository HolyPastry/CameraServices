using Cinemachine;
using UnityEngine;

namespace Bakery.Cameras
{
    [CreateAssetMenu(menuName = "Bakery/Cameras/Transition")]
    public class CameraTransition : ScriptableObject
    {
        public CameraReference CameraIn;
        public CameraReference CameraOut;
        public CinemachineBlendDefinition Blend;
    }
}
