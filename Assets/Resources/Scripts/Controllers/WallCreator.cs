using System;
using System.Collections.Generic;
using System.Linq;
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
    private float _posZ = 0;
    private Vector3[] mousePositions = new Vector3[2];
    private bool _draggingMouse = false;
    private bool _drawRect = false;
    public float Timer = 1.2f;
    #endregion

    public List<Model3D> modelSList = new List<Model3D>();
    public static int wallNum = 0;

    void Start() {
        //We disable the capture keyboard function from the WebGL plugin, otherwise we would not be able to communicate with our webpage using JS (our inputs would not take keyboard)
        #if !UNITY_EDITOR && UNITY_WEBGL
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;
        #endif
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
                ResetMouseClickTimer();
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

        if (!Input.GetMouseButtonUp(0)) return;
        if (!_draggingMouse) return;
        mousePositions[1] = Input.mousePosition;
        if (mousePositions[0] == mousePositions[1]) return;
        _drawRect = true;
        var beginPos = Camera.main.ScreenToWorldPoint(mousePositions[0]);
        var endPos = Camera.main.ScreenToWorldPoint(mousePositions[1]);
        var x = Math.Min(beginPos.x, endPos.x);
        var y = Math.Min(beginPos.y, endPos.y);
        var width = Math.Max(beginPos.x, endPos.x) - x;
        var height = Math.Max(beginPos.y, endPos.y) - y;
        CreateWall(width, height, x, y);
    }

    //Creation use by the Html button
    void CreateWall()
    {
        var model = new Model3D();
        model.CreateModel(0, 0, 0, GetLengthFromPage(), GetHeightFromPage(), GetWidthFromPage(), "Wall" + wallNum, "Green");
        modelSList.Add(model);
        wallNum++;
        #if !UNITY_EDITOR && UNITY_WEBGL
            GetWallsList();
        #endif
    }

    //Creation use by drawing with mouse
    void CreateWall(float width, float height, float x, float y)
    {
        var model = new Model3D();
        model.CreateModel(x, y, _posZ, width, height, 2, "Wall" + wallNum, "Green");
        modelSList.Add(model);
        wallNum++;
        _posZ += 10;
        #if !UNITY_EDITOR && UNITY_WEBGL
            GetWallsList();
        #endif
    }

    //Copy Paste function
    public void CopyWall(string selectedWall)
    {
        var model = JsonUtility.FromJson<SzModel>(selectedWall);
        foreach (var item in modelSList.Where(x => x.Name == model.modelName))
        {
            var copyWall = new Model3D();
            copyWall.CreateModel(item.Position.x + item.Size.x, item.Position.y, item.Position.z,
                item.Size.x, item.Size.y, item.Size.z, "Wall" + wallNum, "Green");
            wallNum++;
            modelSList.Add(copyWall);
            #if !UNITY_EDITOR && UNITY_WEBGL
                GetWallsList();
            #endif
        }
    }

    // Reset the event for drawing with mouse after a certain amount of time
    private void ResetMouseClickTimer()
    {
        _drawRect = false;
        mousePositions[0] = new Vector3();
        mousePositions[1] = new Vector3();
        Timer = 1.2f;
        _draggingMouse = false;
    }
    
    void EditWall(string selectedWall)
    {
        var model = JsonUtility.FromJson<SzModel>(selectedWall);
        foreach (var item in modelSList.Where(x => x.Name == model.modelName))
        {
            var size = item.Model.GetComponent<Renderer>().bounds.size;
            var rescale = item.Model.transform.localScale;
            var newSize = new Vector3(GetLengthFromPage(), GetHeightFromPage(), GetWidthFromPage());
            rescale.x = newSize.x * rescale.x / size.x;
            rescale.y = newSize.y * rescale.y / size.y;
            rescale.z = newSize.z * rescale.z / size.z;
            item.Model.transform.localScale = rescale;
            item.Size = newSize;
            #if !UNITY_EDITOR && UNITY_WEBGL
                GetWallsList();
            #endif
        }
    }
 
    //Destroy the Wall selected
    void RemoveWall(string selectedWall)
    {
        var model = JsonUtility.FromJson<SzModel>(selectedWall);
        foreach (var item in modelSList.Where(x => x.Name == model.modelName))
        {
            var hiddenWall = item.Model;
            hiddenWall.SetActive(false);
            modelSList.Remove(item);
            #if !UNITY_EDITOR && UNITY_WEBGL
                    GetWallsList();
            #endif
        }
    }

    //Method that takes our C# walls list and send it back to our webpage using pointers to the adress of the list
    public void GetWallsList()
    {
        //We need to have a simple serializable object
        var szModelList = new List<SzModel>();
        foreach (var item in modelSList)
        {
            var newWall = new SzModel {modelName = item.Name, modelSize = item.Size};
            szModelList.Add(newWall);
        }

        //We serialize our list of simple objects and pass it back to our html
        GetModelsList(JsonHelper.ToJson(szModelList.ToArray(), true));
    }
}