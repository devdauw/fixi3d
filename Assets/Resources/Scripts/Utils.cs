using UnityEngine;

namespace Resources.Scripts
{
    public static class Utils
    {
        private static Texture2D _whiteTexture;
        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null) 
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }

        public static void DrawMouseRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
            GUI.color = Color.white;
        }

        public static void DrawMouseRectBorder(Rect rect, float thickness, Color color)
        {
            // Top
            Utils.DrawMouseRect( new Rect( rect.xMin, rect.yMin, rect.width, thickness ), color );
            // Left
            Utils.DrawMouseRect( new Rect( rect.xMin, rect.yMin, thickness, rect.height ), color );
            // Right
            Utils.DrawMouseRect( new Rect( rect.xMax - thickness, rect.yMin, thickness, rect.height ), color);
            // Bottom
            Utils.DrawMouseRect( new Rect( rect.xMin, rect.yMax - thickness, rect.width, thickness ), color );
        }

        public static Rect GetMousePositions(Vector3 screenPos1, Vector3 screenPos2)
        {
            screenPos1.y = Screen.height - screenPos1.y;
            screenPos2.y = Screen.height - screenPos2.y;

            var topLeft = Vector3.Min(screenPos1, screenPos2);
            var bottomRight = Vector3.Max(screenPos1, screenPos2);

            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }
        
        public static Vector3[] ToVector3(this Vector2[] vectors)
        {
            return System.Array.ConvertAll<Vector2, Vector3>(vectors, v => v);
        }
    }
}