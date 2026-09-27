using System.Collections.Generic;
using UnityEngine;

namespace RallyRacing.Data
{
    [CreateAssetMenu(menuName = "Rally Racing/Car Definition", fileName = "Car_")]
    public sealed class CarDefinition : ScriptableObject
    {
        public string displayName = "Falcon";
        public Color paint = new Color(0.9f, 0.2f, 0.12f);
        [Range(10, 70)] public float topSpeed = 34;
        [Range(5, 40)] public float acceleration = 18;
        [Range(0.2f, 2)] public float handling = 1;
        [Range(5, 30)] public float braking = 15;
        [Range(0.2f, 2)] public float grip = 1;
        [Range(0.2f, 2)] public float drift = 1;
        [Range(600, 1800)] public float mass = 950;

        public static List<CarDefinition> CreateDefaults()
        {
            return new List<CarDefinition>
            {
                Make("Falcon", new Color(0.92f, 0.18f, 0.1f), 34f, 18f, 1.15f, 16f, 1.1f, 1.15f, 950f),
                Make("Viper", new Color(0.12f, 0.55f, 0.94f), 39f, 16f, 0.95f, 15f, 0.95f, 0.9f, 1050f),
                Make("Titan", new Color(0.95f, 0.65f, 0.08f), 31f, 21f, 0.85f, 19f, 1.25f, 1.3f, 1180f)
            };
        }

        private static CarDefinition Make(string label, Color color, float speed, float accel, float steer, float brake, float gripValue, float driftValue, float weight)
        {
            var item = CreateInstance<CarDefinition>();
            item.displayName = label; item.paint = color; item.topSpeed = speed; item.acceleration = accel;
            item.handling = steer; item.braking = brake; item.grip = gripValue; item.drift = driftValue; item.mass = weight;
            return item;
        }
    }
}
