using UnityEngine;

namespace SO{
    public static class SOExtensions{
        private static ConfigTest1 cacheConfigTest1;

        public static ConfigTest1 GetConfigTest1() {
            if (cacheConfigTest1) return cacheConfigTest1;
            cacheConfigTest1 = Resources.Load<ConfigTest1>("ConfigTest1");
            return cacheConfigTest1;
        }
    }
}