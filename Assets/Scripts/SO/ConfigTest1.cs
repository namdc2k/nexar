using UnityEngine;

namespace SO{
    [CreateAssetMenu(fileName = "ConfigTest1", menuName = "SO/ConfigTest1")]
    public class ConfigTest1 : ScriptableObject{
        [Range(0, 2)] public float speedPlayer;
        [Range(0, 3)] public float distancePlayerToTarget;
        public float RotateSmooth;
    }
}