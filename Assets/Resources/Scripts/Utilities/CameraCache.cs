using UnityEngine;

namespace Hakobio.Utility
{
    public static class CameraCache
    {
        private static Camera cachedCamera;

        public static Camera Main
        {
            get
            {
                if (cachedCamera == null || !cachedCamera.gameObject.activeInHierarchy)
                {
                    return Refresh(Camera.main);
                }
                return cachedCamera;
            }
        }

        public static Camera Refresh(Camera newMain)
        {
            return cachedCamera = newMain;
        }
    }
}
