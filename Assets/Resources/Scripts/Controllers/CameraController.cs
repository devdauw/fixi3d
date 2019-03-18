using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] Cameras;
    private float moveSpeed = 0.125f;
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
                sizeD += 0.2f;
                Cameras[_currentCameraIndex].orthographicSize = sizeD;
                break;
            // Mouse Wheel Up
            case -1:
                var sizeU = Cameras[_currentCameraIndex].orthographicSize;
                sizeU -= 0.2f;
                Cameras[_currentCameraIndex].orthographicSize = sizeU;
                break;
            default:
                return;
        }
    }

    void MoveCamera(string direction)
    {
        var pos = Cameras[_currentCameraIndex].transform.position;
        var rot = Cameras[_currentCameraIndex].transform.eulerAngles;
        switch (_currentCameraIndex)
        {
            // 2D Camera
            case 0:
                switch (direction)
                {
                    case "Left":
                        pos.x -= 0.1f;
                        break;
                    case "Top":
                        pos.y += 0.1f;
                        break;
                    case "Right":
                        pos.x += 0.1f;
                        break;
                    case "Bottom":
                        pos.y -= 0.1f;
                        break;
                    default:
                        return;
                }
                break;

            // 3D Ortho Camera
            case 1:
                switch (direction)
                {
                    case "Left":
                        pos.x -= moveSpeed;
                        pos.y += moveSpeed / 10;
                        pos.z -= moveSpeed * 1.9f;
                        break;
                    case "Top":
                        pos.x -= moveSpeed / 4;
                        pos.y += moveSpeed;
                        pos.z += moveSpeed / 10;
                        break;
                    case "Right":
                        pos.x += moveSpeed;
                        pos.y -= moveSpeed / 10;
                        pos.z += moveSpeed * 1.9f;
                        break;
                    case "Bottom":
                        pos.x += moveSpeed / 4;
                        pos.y -= moveSpeed;
                        pos.z -= moveSpeed / 10;
                        break;
                    case "CtrlLeft":
                        rot.y -= 0.1f;
                        Cameras[1].transform.eulerAngles = rot;
                        break;
                    case "CtrlTop":
                        rot.x -= 0.1f;
                        Cameras[1].transform.eulerAngles = rot;
                        break;
                    case "CtrlRight":
                        rot.y += 0.1f;
                        Cameras[1].transform.eulerAngles = rot;
                        break;
                    case "CtrlBottom":
                        rot.x += 0.1f;
                        Cameras[1].transform.eulerAngles = rot;
                        break;
                    default:
                        return;
                }
                break;

            default:
                return;
        }
        Cameras[_currentCameraIndex].transform.position = pos;
    }

    private void Move(Vector3 m)
    {
        var pos = Cameras[_currentCameraIndex].transform.position;
        pos.x -= m.x;
        pos.y += m.y;
        pos.z += m.z;
        Cameras[_currentCameraIndex].transform.position = pos;
    }
}