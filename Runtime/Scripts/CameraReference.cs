using System;
using UnityEngine;

namespace Bakery.Cameras
{
    [CreateAssetMenu(menuName = "Bakery/Cameras/Reference")]
    public class CameraReference : ScriptableObject
    {
        public bool GameplayCamera = false;
    }
}
