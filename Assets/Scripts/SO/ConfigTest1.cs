using UnityEngine;

namespace SO{
    [CreateAssetMenu(fileName = "ConfigTest1", menuName = "SO/ConfigTest1")]
    public class ConfigTest1 : ScriptableObject{
        public float speedPlayer;
        public float distancePlayerToTarget;
    }
}