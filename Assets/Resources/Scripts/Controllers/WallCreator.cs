using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WallCreator : MonoBehaviour
{
    #region UnityWebGLCom
    //We import our methods from js_cross. Somes are pure js calls to grab data from the page. Others are calls sent from our C# to get data back in our page
    [DllImport("__Internal")]
    private static extern float GetLengthFromPage();
    [DllImport("__Internal")]
    private static extern float GetHeightFromPage();
    [DllImport("__Internal")]
    private static extern float GetWidthFromPage();
    //This method is used to get data back in our page, in this case we pass back the list of objects we created
    [DllImport("__Internal")]
    private static extern void GetModelsList(string cSharpList);
    #endregion

    #region MouseCreation
    private float posZ = 0;
    private Vector3[] mousePositions = new Vector3[2];
    private bool _draggingMouse = false;
    private bool _drawRect = false;
    public float Timer = 1.2f;
    #endregion

    public List<Model3D> modelsList = new List<Model3D>();
    public static int wallNum = 0;

    void Start() {
        //We disable the capture keyboard function from the WebGL plugin, otherwise we would not be able to communicate with our webpage using JS (our inputs would not take keyboard)
        #if !UNITY_EDITOR && UNITY_WEBGL
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;
        #endif
    }

    //Creation use by the Html button
    void CreateWall()
    {
        Model3D model = new Model3D();
        model.CreateModel(0, 0, 0, GetLengthFromPage(), GetHeightFromPage(), GetWidthFromPage(), "Wall" + wallNum, "Green");
        modelsList.Add(model);
        wallNum++;
        GetWallsList();
    }

    //Creation use by drawing with mouse
    void CreateWall(float width, float height, float x, float y)
    {
        Model3D model = new Model3D();
        model.CreateModel(x, y, posZ, width, height, 2, "Wall" + wallNum, "Green");
        modelsList.Add(model);
        wallNum++;
        posZ += 10;
        GetWallsList();
    }

    //Method that takes our C# walls list and send it back to our webpage using pointers to the adress of the list
    public void GetWallsList()
    {
        //We need to have a simple serializable object
        List<SzModel> szModelList = new List<SzModel>();
        foreach (var item in modelsList)
        {
            SzModel newWall = new SzModel();
            newWall.modelName = item.Name;
            newWall.modelSize = item.Size;
            szModelList.Add(newWall);
        }

        //We serialize our list of simple objects and pass it back to our html
        GetModelsList(JsonHelper.ToJson<SzModel>(szModelList.ToArray(), true));
    }

    //Copy Paste function
    public void CopyPaste(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            Model3D wallSelected = modelsList.Find(x => x.Name == value);
            Model3D copyWall = new Model3D();
            copyWall.CreateModel(wallSelected.Position.x + wallSelected.Size.x, wallSelected.Position.y, wallSelected.Position.z,
                wallSelected.Size.x, wallSelected.Size.y, wallSelected.Size.z, "Wall" + wallNum, "Green");
            wallNum++;
            modelsList.Add(copyWall);
            GetWallsList();
        }
    }

    public void GetSelectedWall()
    {
        SzModel selectedWall = new SzModel();
    }

    // Reset the event for drawing with mouse after a certain amount of time
    void resetMouseClickTimer()
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
                resetMouseClickTimer();
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
                if (mousePositions[0] != mousePositions[1])
                {
                    _drawRect = true;
                    Vector3 beginPos = Camera.main.ScreenToWorldPoint(mousePositions[0]);
                    Vector3 endPos = Camera.main.ScreenToWorldPoint(mousePositions[1]);
                    float x = Math.Min(beginPos.x, endPos.x);
                    float y = Math.Min(beginPos.y, endPos.y);
                    float width = Math.Max(beginPos.x, endPos.x) - x;
                    float height = Math.Max(beginPos.y, endPos.y) - y;
                    CreateWall(width, height, x, y);
                } 
            }
        }
    }

    /*
    void EditWall()
    {
        go.transform.localScale = new Vector3(GetLengthFromPage()/Size[0], GetHeightFromPage()/Size[1], GetWidthFromPage()/Size[2]);
    }
    */
 
    //Destroy the Wall selected
    void RemoveWall(string selectedWall)
    {
        //Debug.Log("access + model: " + selectedWall);
        SzModel model = JsonUtility.FromJson<SzModel>(selectedWall);
        foreach (var item in modelsList)
        {
            if (item.Name == model.modelName)
            {
                GameObject hiddenWall = item.Model;
                hiddenWall.SetActive(false);
                Debug.Log("Wall hidden");
                var itemToRemove = modelsList.Find(r => r.Name == item.Name);
                Debug.Log(itemToRemove);
                if (itemToRemove != null)
                {
                    modelsList.Remove(itemToRemove);
                    GetWallsList();
                }

            }
        }
    }
}
