using UnityEngine;

namespace CameraController
{
    /// <summary>
    /// Movement component for the camera
    /// </summary>
    public class CameraMovement : MonoBehaviour
    {
        private const float MovementSpeed = 40;
        private const float MovementInterpolation = 0.9f;

        private Vector3 _movementDeltaInterpolated;

        private void Awake()
        {
            _movementDeltaInterpolated = Vector3.zero;
        }

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var deltaFactor = MovementSpeed * Time.deltaTime;
            var cameraMovementDeltaLocal = Vector3.right * horizontal +
                                           Vector3.forward * vertical;
            var cameraMovementDelta = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * cameraMovementDeltaLocal;

            _movementDeltaInterpolated =
                Vector3.Lerp(deltaFactor * cameraMovementDelta, _movementDeltaInterpolated, MovementInterpolation);

            transform.position += _movementDeltaInterpolated;
        }
    }
}