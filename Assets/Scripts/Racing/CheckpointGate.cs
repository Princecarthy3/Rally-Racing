using System;
using UnityEngine;

namespace RallyRacing.Racing
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class CheckpointGate : MonoBehaviour
    {
        public int Index { get; private set; }
        public event Action<int, ArcadeCrossing> Crossed;
        private Vector3 previousLocal;
        private bool hasPrevious;
        private int lastCrossedFrame = -1;
        public void Configure(int index) { Index = index; GetComponent<BoxCollider>().isTrigger = true; }

        private void OnTriggerStay(Collider other)
        {
            var crossing = other.GetComponentInParent<Vehicles.ArcadeCarController>();
            if (crossing == null) return;
            Vector3 local = transform.InverseTransformPoint(crossing.transform.position);
            if (lastCrossedFrame != Time.frameCount && hasPrevious && previousLocal.z < 0f && local.z >= 0f && Vector3.Dot(crossing.Body.velocity, transform.forward) > 0.5f)
            {
                lastCrossedFrame = Time.frameCount;
                Crossed?.Invoke(Index, new ArcadeCrossing(crossing));
            }
            previousLocal = local; hasPrevious = true;
        }
    }

    public readonly struct ArcadeCrossing
    {
        public readonly Vehicles.ArcadeCarController Car;
        public ArcadeCrossing(Vehicles.ArcadeCarController car) { Car = car; }
    }
}
