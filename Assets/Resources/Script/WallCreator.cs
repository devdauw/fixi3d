using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class WallCreator : MonoBehaviour
{
    public List<float> Size { get; set; } = new List<float>();
    [DllImport("__Internal")]
    private static extern float GetLengthFromPage();
    [DllImport("__Internal")]
    private static extern float GetHeightFromPage();
    [DllImport("__Internal")]
    private static extern float GetWidthFromPage();
    private static int _nbWall = 1;
    private Vector2 _box_start_pos;
    private Vector2 _box_end_pos;

    public List<GameObject> ListGo = new List<GameObject>();
    private float posZ = 0;

    private Rect position = new Rect(193, 148, 249 - 193, 148 - 104);
    public Color color = Color.green;
    private Vector3[] mousePositions = new Vector3[2];
    private bool _draggingMouse = false;
    private bool _drawRect = false;
    public float Timer = 1.2f;

    void Start()
    {
        /*#if !UNITY_EDITOR && UNITY_WEBGL
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;
        #endif*/
    }

    void CreateGo(float width, float height, float x, float y)
    {
        /*Size.Add(GetLengthFromPage());
        Size.Add(GetHeightFromPage());
        Size.Add(GetWidthFromPage());*/
        Model3D model = new Model3D();
        ListGo.Add(model.CreateModel(x, y, posZ, width, height, 2, "Wall" + _nbWall, "Green"));
        //go = model.CreateModel(0, 0, 0, Size[0], Size[1], Size[2], "Wall" + _nbWall, "Green");
        _nbWall++;
        posZ += 10;
    }

    void EditWall()
    {
        ListGo.First().transform.localScale = new Vector3(GetLengthFromPage() / Size[0], GetHeightFromPage() / Size[1], GetWidthFromPage() / Size[2]);
    }

    void DestroyWall()
    {
        //Destroy(go);
        ListGo.Remove(ListGo.First());
    }

    void reset()
    {
        _drawRect = false;
        mousePositions[0] = new Vector3();
        mousePositions[1] = new Vector3();
        Timer = 1.2f;
        _draggingMouse = false;
    }
    private void Update()
    {
        if (_drawRect)
        {
            if (Timer > 0.1)
            {
                Timer -= 1 * Time.deltaTime;
            }
            else
            {
                reset();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!_draggingMouse)
            {
                mousePositions[0] = Input.mousePosition;
            }
            _draggingMouse = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_draggingMouse)
            {
                mousePositions[1] = Input.mousePosition;
                _drawRect = true;
                Vector3 beginPos = Camera.main.ScreenToWorldPoint(mousePositions[0]);
                Vector3 endPos = Camera.main.ScreenToWorldPoint(mousePositions[1]);
                float x = Math.Min(beginPos.x, endPos.x);
                float y = Math.Min(beginPos.y, endPos.y);
                float width = Math.Max(beginPos.x, endPos.x) - x;
                float height = Math.Max(beginPos.y, endPos.y) - y;
                CreateGo(width, height, x, y);
            }
        }
    }
}
