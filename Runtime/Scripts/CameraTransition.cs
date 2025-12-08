using Cinemachine;
using UnityEngine;

namespace Bakery
{
    [CreateAssetMenu(menuName = "Bakery/Cameras/Transition")]
    public class CameraTransition : ScriptableObject
    {
        public CameraReference CameraIn;
        public CameraReference CameraOut;
        public CinemachineBlendDefinition Blend;
    }
}
