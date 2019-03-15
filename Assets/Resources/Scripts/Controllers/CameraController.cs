using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] Cameras;
    private int _currentCameraIndex;// 0 : 2D camera
                                    // 1 : 3D Ortho Camera

    void Start()
    {
        _currentCameraIndex = 0;
        Cameras = Camera.allCameras;
        for (var i = 0; i < Cameras.Length; i++)
        {
            if (i != 0)
                Cameras[i].enabled = false;
            else
                Cameras[i].enabled = true;
        }
    }

    void SwitchCamera()
    {
        if (Cameras.Length - 1 != _currentCameraIndex)
            _currentCameraIndex++;
        else
            _currentCameraIndex = 0;
        for (int i = 0; i < Cameras.Length; i++)
        {
            if (i != _currentCameraIndex)
                Cameras[i].enabled = false;
            else
                Cameras[i].enabled = true;
        }
    }

    void ZoomCamera(int n)
    {
        // If camera 3D Ortho is selected
        if (_currentCameraIndex != 1) return;
        switch (n)
        {
            // Mouse Wheel Down
            case 1:
                var sizeD = Cameras[_currentCameraIndex].orthographicSize;
                sizeD += (float)0.2;
                Cameras[_currentCameraIndex].orthographicSize = sizeD;
                break;
            // Mouse Wheel Up
            case -1:
                var sizeU = Cameras[_currentCameraIndex].orthographicSize;
                sizeU -= (float)0.2;
                Cameras[_currentCameraIndex].orthographicSize = sizeU;
                break;
        }
    }
}
