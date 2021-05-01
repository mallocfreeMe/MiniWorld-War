using System;
using UnityEngine;

namespace UI
{
    public class CameraControl : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            // positive -> scroll up -> zoom out camera
            // negative -> scroll down -> zoom in camera
            if (Input.mouseScrollDelta.y < 0 && _camera.orthographicSize < 20)
            {
                _camera.orthographicSize++;
            } else if (Input.mouseScrollDelta.y > 0 && _camera.orthographicSize > 5)
            {
                _camera.orthographicSize--;
            }
        }
    }
}
