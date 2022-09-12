using UnityEngine;

namespace CameraController
{
    /// <summary>
    /// Controls distance of camera to look at target
    /// </summary>
    public class CameraZoom : MonoBehaviour
    {
        private const float MinDistance = 3;
        private const float MaxDistance = 30;
        private const int DistanceSteps = 5;
        private const float DistanceInterpolation = 0.95f;

        private static float StepSize => (MaxDistance - MinDistance) / DistanceSteps;

        private float _currentDistance;
        private float _distanceTarget;

        private void Awake()
        {
            _currentDistance = -transform.position.z;
            _distanceTarget = (MaxDistance + MaxDistance) / 2.0f;
        }

        private void Update()
        {
            var scroll = Input.mouseScrollDelta.y;

            var delta = scroll switch
            {
                > 0 => -StepSize,
                < 0 => +StepSize,
                _ => 0
            };

            // calculate new distance target
            _distanceTarget += delta;
            _distanceTarget = Mathf.Clamp(_distanceTarget, MinDistance, MaxDistance);

            _currentDistance = Mathf.Lerp(_distanceTarget, _currentDistance, DistanceInterpolation);

            transform.localPosition = _currentDistance * Vector3.back;
        }
    }
}