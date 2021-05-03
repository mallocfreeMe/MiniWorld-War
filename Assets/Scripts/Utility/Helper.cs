using UnityEngine;

namespace Utility
{
    public class Helper : MonoBehaviour
    {
        public static Vector3 GetMouseWorldPos(Camera camera)
        {
            var pos = new Vector3();
            if (camera != null)
            {
                pos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                    camera.ScreenToWorldPoint(Input.mousePosition).y,
                    camera.ScreenToWorldPoint(Input.mousePosition).z);
            }
            return pos;
        }
    }
}