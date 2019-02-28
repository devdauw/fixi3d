using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrawing : MonoBehaviour
{
    private Rect position = new Rect(193, 148, 249 - 193, 148 - 104);
    public Color color = Color.green;
    private Vector3[] mousePositions = new Vector3[2];
    private bool draggingMouse = false;
    private bool drawRect = false;
    public float timer = 1.2f;

    void OnGUI()
    {
        if (drawRect)
        {
            DrawRectangle(position, 1, color);
        }
    }

    void DrawRectangle(Rect area, int frameWidth, Color color)
    {
        //Create a one pixel texture with the right color
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        Rect lineArea = area;
        lineArea.height = frameWidth; //Top line
        GUI.DrawTexture(lineArea, texture);
        lineArea.y = area.yMax - frameWidth; //Bottom
        GUI.DrawTexture(lineArea, texture);
        lineArea = area;
        lineArea.width = frameWidth; //Left
        GUI.DrawTexture(lineArea, texture);
        lineArea.x = area.xMax - frameWidth;//Right
        GUI.DrawTexture(lineArea, texture);
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
                print("x start:" + mousePositions[0].x);
                print("y start:" + mousePositions[0].y);
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
                float y2 = Math.Min(mousePositions[0].y, mousePositions[1].y);
                float width = Math.Max(mousePositions[0].x, mousePositions[1].x) - x;
                float height = Math.Max(Screen.height - mousePositions[0].y, Screen.height - mousePositions[1].y) - y;
                print("width:" + width);
                print("height:" + height);
                print("x end:" + mousePositions[1].x);
                print("y end:" + mousePositions[1].y);
                print("X" + x);
                print("Y2" + y2);
                print("X-335/30 : " + ((x-335)/30));
                print("Y2-72/30 : " + ((y2-72)/30));
                /*print("Width/30:" + (width/30));
                print("Height/30:" + (height/30));
                print("Width+335/30 : " + (width-335)/30);
                print("Height+72/30 : " + (height-72)/30);*/
                position = new Rect(x, y, width, height);
                print("Got last mouse position!");
                drawRect = true;
            }
        }
    }
}
