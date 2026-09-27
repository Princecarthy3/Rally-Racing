using RallyRacing.Data;
using UnityEngine;

namespace RallyRacing.Vehicles
{
    /// <summary>Simple arcade car body driven with physics forces and local keyboard input.</summary>
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ArcadeCarController : MonoBehaviour
    {
        private Rigidbody body;
        private CarDefinition definition;
        private bool controlsEnabled;
        private float throttle;
        private float steering;
        private bool handbrake;
        private Vector3 spawnPosition;
        private Quaternion spawnRotation;
        public float SpeedKph => body == null ? 0 : body.velocity.magnitude * 3.6f;
        public Rigidbody Body => body;

        public void Initialize(CarDefinition car)
        {
            definition = car; body = GetComponent<Rigidbody>(); body.mass = car.mass;
            body.drag = 0.04f; body.angularDrag = 1.2f; body.interpolation = RigidbodyInterpolation.Interpolate;
            spawnPosition = transform.position; spawnRotation = transform.rotation;
        }
        public void SetControlsEnabled(bool enabled) { controlsEnabled = enabled; if (!enabled && body != null) body.velocity = Vector3.zero; }
        public void Respawn()
        {
            body.velocity = Vector3.zero; body.angularVelocity = Vector3.zero;
            transform.SetPositionAndRotation(spawnPosition + Vector3.up, spawnRotation);
        }

        private void Update()
        {
            if (!controlsEnabled) { throttle = 0; steering = 0; handbrake = false; return; }
            throttle = Input.GetAxisRaw("Vertical"); steering = Input.GetAxisRaw("Horizontal");
            handbrake = Input.GetKey(KeyCode.Space);
            if (Input.GetKeyDown(KeyCode.R)) Respawn();
        }

        private void FixedUpdate()
        {
            if (body == null || definition == null || !controlsEnabled) return;
            var localVelocity = transform.InverseTransformDirection(body.velocity);
            var forwardSpeed = localVelocity.z;
            if (throttle >= 0 || forwardSpeed > 1f)
                body.AddForce(transform.forward * (throttle * definition.acceleration), ForceMode.Acceleration);
            else
                body.AddForce(transform.forward * (throttle * definition.braking), ForceMode.Acceleration);
            var normalizedSpeed = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / definition.topSpeed);
            var turn = steering * definition.handling * Mathf.Lerp(0.35f, 1f, normalizedSpeed) * Mathf.Sign(Mathf.Abs(forwardSpeed) < 0.2f ? 1f : forwardSpeed);
            body.MoveRotation(body.rotation * Quaternion.Euler(0, turn * 90f * Time.fixedDeltaTime, 0));
            var lateralDamping = handbrake ? 1.4f / definition.drift : 7f * definition.grip;
            body.AddForce(-transform.right * localVelocity.x * lateralDamping, ForceMode.Acceleration);
            if (body.velocity.magnitude > definition.topSpeed)
                body.velocity = body.velocity.normalized * definition.topSpeed;
        }
    }
}
