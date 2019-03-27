using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] Cameras;
    private const float MoveSpeed = 0.125f;
    private const float MoveSpeedR = 40f;
    private Quaternion _defaultCameraRotation;
    private int _currentCameraIndex;// 0 : 2D camera
                                    // 1 : 3D Ortho Camera
    private GameObject _cameraRotator;
    private bool _moveLeft = false;
    private bool _moveTop = false;
    private bool _moveRight = false;
    private bool _moveBottom = false;

    void Start()
    {
        _currentCameraIndex = 0;
        Cameras = Camera.allCameras;
        for (var i = 0; i < Cameras.Length; i++)
            Cameras[i].enabled = i == 0;
        _cameraRotator = GameObject.Find("Camera Rotator");
        _defaultCameraRotation = _cameraRotator.transform.rotation;
    }

    void SwitchCamera()
    {
        if (Cameras.Length - 1 != _currentCameraIndex)
            _currentCameraIndex++;
        else
            _currentCameraIndex = 0;
        for (var i = 0; i < Cameras.Length; i++)
            Cameras[i].enabled = i == _currentCameraIndex;
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
        switch (_currentCameraIndex)
        {
            // 2D Camera
            case 0:
                switch (direction)
                {
                    case "Left":
                        pos.x -= 0.1f;
                        Cameras[_currentCameraIndex].transform.position = pos;
                        break;
                    case "Top":
                        pos.y += 0.1f;
                        Cameras[_currentCameraIndex].transform.position = pos;
                        break;
                    case "Right":
                        pos.x += 0.1f;
                        Cameras[_currentCameraIndex].transform.position = pos;
                        break;
                    case "Bottom":
                        pos.y -= 0.1f;
                        Cameras[_currentCameraIndex].transform.position = pos;
                        break;
                    default:
                        return;
                }
                break;

            // 3D Ortho Camera
            case 1:
                switch (direction)
                {
                    case "CtrlLeft":
                        pos.x -= MoveSpeed;
                        pos.y += MoveSpeed / 10;
                        pos.z -= MoveSpeed * 1.9f;
                        Cameras[_currentCameraIndex].transform.position = pos;
                        break;
                    case "CtrlTop":
                        pos.x -= MoveSpeed / 4;
                        pos.y += MoveSpeed;
                        pos.z += MoveSpeed / 10;
                        Cameras[_currentCameraIndex].transform.position = pos;
                        break;
                    case "CtrlRight":
                        pos.x += MoveSpeed;
                        pos.y -= MoveSpeed / 10;
                        pos.z += MoveSpeed * 1.9f;
                        Cameras[_currentCameraIndex].transform.position = pos;
                        break;
                    case "CtrlBottom":
                        pos.x += MoveSpeed / 4;
                        pos.y -= MoveSpeed;
                        pos.z -= MoveSpeed / 10;
                        Cameras[_currentCameraIndex].transform.position = pos;
                        break;
                    case "Left":
                        _moveLeft = true;
                        break;
                    case "LeftDisable":
                        _moveLeft = false;
                        break;
                    case "Top":
                        _moveTop = true;
                        break;
                    case "TopDisable":
                        _moveTop = false;
                        break;
                    case "Right":
                        _moveRight = true;
                        break;
                    case "RightDisable":
                        _moveRight = false;
                        break;
                    case "Bottom":
                        _moveBottom = true;
                        break;
                    case "BottomDisable":
                        _moveBottom = false;
                        break;
                    case "ResetRot":
                        _cameraRotator.transform.rotation = _defaultCameraRotation;
                        break;
                    default:
                        return;
                }
                break;
            default:
                return;
        }
    }

    private void Move(Vector3 m)
    {
        var pos = Cameras[_currentCameraIndex].transform.position;
        pos.x -= m.x;
        pos.y += m.y;
        pos.z += m.z;
        Cameras[_currentCameraIndex].transform.position = pos;
    }

    void Update()
    {
        if(_moveLeft)
            _cameraRotator.transform.Rotate(0, MoveSpeedR * Time.deltaTime, 0);
        else if (_moveTop)
            _cameraRotator.transform.Rotate(MoveSpeedR * Time.deltaTime, 0, 0);
        else if (_moveRight)
            _cameraRotator.transform.Rotate(0, -MoveSpeedR * Time.deltaTime, 0);
        else if(_moveBottom)
            _cameraRotator.transform.Rotate(-MoveSpeedR * Time.deltaTime, 0, 0);
    }
}