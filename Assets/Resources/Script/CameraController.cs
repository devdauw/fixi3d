using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] Cameras;
    private int _currentCameraIndex;

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

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
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
    }
}
