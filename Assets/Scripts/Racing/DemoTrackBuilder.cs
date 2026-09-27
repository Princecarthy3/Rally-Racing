using RallyRacing.Data;
using RallyRacing.Vehicles;
using UnityEngine;

namespace RallyRacing.Racing
{
    /// <summary>Creates a readable primitive circuit and the gameplay gates for the first prototype.</summary>
    public sealed class DemoTrackBuilder
    {
        private static readonly Vector3[] Points =
        {
            new Vector3(0, 0, 0), new Vector3(0, 0, 110), new Vector3(75, 3, 145),
            new Vector3(150, 7, 100), new Vector3(150, 7, -15), new Vector3(80, 3, -65)
        };
        public readonly List<CheckpointGate> Gates = new List<CheckpointGate>();
        private readonly Vector3 startPosition;
        private readonly Quaternion startRotation;

        private DemoTrackBuilder()
        {
            var root = new GameObject("Coastal Circuit");
            CreatePrimitive("Infield", PrimitiveType.Plane, new Vector3(75, -0.8f, 40), new Vector3(30, 1, 28), new Color(0.19f, 0.38f, 0.2f), root.transform);
            for (int i = 0; i < Points.Length; i++)
            {
                Vector3 a = Points[i]; Vector3 b = Points[(i + 1) % Points.Length];
                Vector3 delta = b - a; Vector3 midpoint = (a + b) * 0.5f;
                var road = CreatePrimitive("Road " + i, PrimitiveType.Cube, midpoint + Vector3.up * 0.05f, new Vector3(12f, 0.5f, delta.magnitude + 2f), new Color(0.12f, 0.13f, 0.15f), root.transform);
                road.transform.rotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
                Vector3 side = Vector3.Cross(Vector3.up, delta.normalized).normalized * 7f;
                CreatePrimitive("Barrier L " + i, PrimitiveType.Cube, midpoint + side + Vector3.up, new Vector3(1, 2, delta.magnitude), new Color(0.82f, 0.82f, 0.78f), root.transform).transform.rotation = road.transform.rotation;
                CreatePrimitive("Barrier R " + i, PrimitiveType.Cube, midpoint - side + Vector3.up, new Vector3(1, 2, delta.magnitude), new Color(0.82f, 0.82f, 0.78f), root.transform).transform.rotation = road.transform.rotation;
            }
            startPosition = Points[0] + new Vector3(0, 1.2f, 8f);
            startRotation = Quaternion.LookRotation((Points[1] - Points[0]).normalized);
            for (int i = 0; i < Points.Length; i++)
            {
                var gateObject = new GameObject(i == 0 ? "Finish Gate" : "Checkpoint " + i);
                gateObject.transform.SetParent(root.transform); gateObject.transform.position = Points[i];
                gateObject.transform.rotation = Quaternion.LookRotation((Points[(i + 1) % Points.Length] - Points[i]).normalized);
                var collider = gateObject.AddComponent<BoxCollider>(); collider.size = new Vector3(12f, 5f, 1f); collider.center = Vector3.up * 2.5f;
                var gate = gateObject.AddComponent<CheckpointGate>(); gate.Configure(i); Gates.Add(gate);
                CreatePrimitive("Gate Marker", PrimitiveType.Cube, Vector3.up * 4f, new Vector3(12f, 0.25f, 0.4f), i == 0 ? new Color(0.15f, 0.9f, 0.46f) : new Color(0.95f, 0.67f, 0.1f), gateObject.transform);
            }
            var lightObject = new GameObject("Demo Sun");
            var light = lightObject.AddComponent<Light>(); light.type = LightType.Directional; light.intensity = 1.3f;
            lightObject.transform.rotation = Quaternion.Euler(48, -32, 0);
            RenderSettings.ambientLight = new Color(0.55f, 0.58f, 0.63f);
        }

        public static DemoTrackBuilder Build() => new DemoTrackBuilder();
        public ArcadeCarController SpawnCar(CarDefinition definition)
        {
            var car = CreatePrimitive(definition.displayName, PrimitiveType.Cube, startPosition, new Vector3(2.1f, 1f, 4f), definition.paint, null);
            car.transform.rotation = startRotation;
            car.AddComponent<Rigidbody>();
            var controller = car.AddComponent<ArcadeCarController>(); controller.Initialize(definition);
            var cameraObject = new GameObject("Race Camera");
            var camera = cameraObject.AddComponent<Camera>(); cameraObject.AddComponent<AudioListener>();
            cameraObject.AddComponent<FollowCamera>().Follow(controller);
            return controller;
        }

        private static GameObject CreatePrimitive(string name, PrimitiveType type, Vector3 position, Vector3 scale, Color color, Transform parent)
        {
            var item = GameObject.CreatePrimitive(type); item.name = name; item.transform.position = position; item.transform.localScale = scale;
            if (parent != null) item.transform.SetParent(parent, true);
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var material = new Material(shader) { color = color }; item.GetComponent<Renderer>().sharedMaterial = material;
            return item;
        }
    }
}
