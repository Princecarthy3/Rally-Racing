using UnityEngine;

namespace RallyRacing.Vehicles
{
    public sealed class FollowCamera : MonoBehaviour
    {
        private Transform target;
        private ArcadeCarController car;
        private Camera cameraComponent;
        public void Follow(ArcadeCarController targetCar)
        {
            car = targetCar; target = targetCar.transform; cameraComponent = GetComponent<Camera>();
            transform.position = target.position - target.forward * 7f + Vector3.up * 4f;
        }
        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 lookAhead = target.forward * Mathf.Clamp(car.SpeedKph / 45f, 2f, 8f);
            Vector3 desired = target.position - target.forward * 7f + Vector3.up * 4f;
            transform.position = Vector3.Lerp(transform.position, desired, 1f - Mathf.Exp(-5f * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position + lookAhead + Vector3.up - transform.position), 1f - Mathf.Exp(-6f * Time.deltaTime));
            cameraComponent.fieldOfView = Mathf.Lerp(62f, 76f, Mathf.Clamp01(car.SpeedKph / 150f));
        }
    }
}
