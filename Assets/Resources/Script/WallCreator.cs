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
    private bool draggingMouse = false;
    private bool drawRect = false;
    public float timer = 1.2f;

    void Start()
    {
        /*#if !UNITY_EDITOR && UNITY_WEBGL
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;
        #endif*/
    }

    void CreateGo(float width, float height)
    {
        /*Size.Add(GetLengthFromPage());
        Size.Add(GetHeightFromPage());
        Size.Add(GetWidthFromPage());*/
        Model3D model = new Model3D();
        ListGo.Add(model.CreateModel(0,0, posZ, width, height, 2, "Wall" + _nbWall, "Green"));
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
        drawRect = false;
        mousePositions[0] = new Vector3();
        mousePositions[1] = new Vector3();
        timer = 1.2f;
        draggingMouse = false;
    }
    private void Update()
    {
        if (drawRect)
        {
            if (timer > 0.1)
            {
                timer -= 1 * Time.deltaTime;
            }
            else
            {
                reset();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!draggingMouse)
            {
                mousePositions[0] = Input.mousePosition;
                //print("x start:" + mousePositions[0].x);
                //print("y start:" + mousePositions[0].y);
            }
            draggingMouse = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (draggingMouse)
            {
                mousePositions[1] = Input.mousePosition;
                float x = Math.Min(mousePositions[0].x, mousePositions[1].x);
                float y = Math.Min(Screen.height - mousePositions[0].y, Screen.height - mousePositions[1].y);
                float width = Math.Max(mousePositions[0].x, mousePositions[1].x) - x;
                float height = Math.Max(Screen.height - mousePositions[0].y, Screen.height - mousePositions[1].y) - y;
                //print("width:" + width);
                //print("height:" + height);
                //print("x end:" + mousePositions[1].x);
                //print("y end:" + mousePositions[1].y);
                //print("Got last mouse position!");
                drawRect = true;
                CreateGo(width, height);
            }
        }
    }
}
