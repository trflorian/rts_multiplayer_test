using UnityEngine;

namespace CameraController
{
    /// <summary>
    /// Positions and aligns the camera smoothly to a point
    /// </summary>
    public class CameraRig : MonoBehaviour
    {
        [SerializeField] private Transform cameraPoint;
        [SerializeField] private Transform cameraLookAtTarget;

        private void Update()
        {
            transform.position = cameraPoint.position;
            transform.LookAt(cameraLookAtTarget);
        }
    }
}