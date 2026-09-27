using System.Collections.Generic;
using UnityEngine;

namespace RallyRacing.Racing
{
    [CreateAssetMenu(menuName = "Rally Racing/Track Definition", fileName = "Track_")]
    public sealed class TrackDefinition : ScriptableObject
    {
        public string displayName = "Coastal Circuit";
        public List<Vector3> waypoints = new List<Vector3>();
        public float roadWidth = 12f;
    }
}
