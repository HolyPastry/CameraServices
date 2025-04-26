using System;
using UnityEngine;

namespace Holypastry.Bakery.Cameras
{
    [CreateAssetMenu(menuName = "Bakery/Cameras/Reference")]
    public class CameraReference : ScriptableObject
    {
        public bool GameplayCamera = false;
    }
}
