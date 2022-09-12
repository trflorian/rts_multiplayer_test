using UnityEngine;

namespace CameraController
{
    /// <summary>
    /// Handle rotation of camera pivot point
    /// </summary>
    public class CameraRotation : MonoBehaviour
    {
        private const int RotationMouseButton = 2;
        private const KeyCode RotateRightKey = KeyCode.E;
        private const KeyCode RotateLeftKey = KeyCode.Q;
        
        private const float RotationInterpolation = 0.5f;
        
        private const float RotationSpeedMouse = 0.5f;
        private const float RotationSpeedKey = 200f;
        private const float RotationAngleLimitVerticalMax = 70;
        private const float RotationAngleLimitVerticalMin = 10;

        private Vector3 _mousePositionPrevious;
        private Vector2 _rotationDeltaInterpolated;

        private void Awake()
        {
            _mousePositionPrevious = Vector3.zero;
            _rotationDeltaInterpolated = Vector2.zero;
        }

        private void Update()
        {
            var horizontal = 0f;
            var vertical = 0f;
            
            if (Input.GetMouseButton(RotationMouseButton))
            {
                // drag
                var delta = Input.mousePosition - _mousePositionPrevious;
                horizontal -= RotationSpeedMouse * delta.x;
                vertical += RotationSpeedMouse * delta.y;
            }

            if (Input.GetKey(RotateRightKey))
            {
                horizontal += RotationSpeedKey * Time.deltaTime;
            }

            if (Input.GetKey(RotateLeftKey))
            {
                horizontal -= RotationSpeedKey * Time.deltaTime;
            }

            Rotate(horizontal, vertical);
        }

        private void LateUpdate()
        {
            _mousePositionPrevious = Input.mousePosition;
        }

        private void Rotate(float horizontal, float vertical)
        {
            var eulerRot = transform.rotation.eulerAngles;

            if (eulerRot.x > 180)
            {
                eulerRot.x -= 360;
            }

            _rotationDeltaInterpolated = Vector2.Lerp(new Vector2(horizontal, vertical), _rotationDeltaInterpolated,
                RotationInterpolation);

            eulerRot.x -= _rotationDeltaInterpolated.y;
            eulerRot.y -= _rotationDeltaInterpolated.x;
                
            eulerRot.x = Mathf.Clamp(eulerRot.x, RotationAngleLimitVerticalMin, RotationAngleLimitVerticalMax);

            transform.rotation = Quaternion.Euler(eulerRot);
        }
    }
}