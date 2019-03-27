using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Resources.Scripts;
using UnityEngine;

public class WallCreator : MonoBehaviour
{
    #region UnityWebGLCom
    //We import our methods from js_cross. Somes are pure js calls to grab data from the page. Others are calls sent from our C# to get data back in our page
    [DllImport("__Internal")]
    private static extern float GetFloatValueFromInput(string input_name);
    //This method is used to get data back in our page, in this case we pass back the list of objects we created
    [DllImport("__Internal")]
    private static extern void SendWallsToPage(string cSharpList);
    #endregion
    
    #region MouseCreation
    private float _posZ = 0;
    private readonly Vector3[] _mousePositions = new Vector3[2];
    private bool _draggingMouse = false;
    #endregion

    public List<Model3D> modelSList = new List<Model3D>();
    private static int _wallNum = 0;

    private void Start() {
        //We disable the capture keyboard function from the WebGL plugin, otherwise we would not be able to communicate with our webpage using JS (our inputs would not take keyboard)
        #if !UNITY_EDITOR && UNITY_WEBGL
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;
        #endif
    }

    void Update()
    {
        //Check pour verifier que l'on est sur le plan en 2D afin d'eviter les null exception
        if (Camera.main != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _draggingMouse = true;
                _mousePositions[0] = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _draggingMouse = false;
            
                //On transform les donnees de notre souris pour les faire correspondre aux coordonnees de notre monde
                var beginPos = Camera.main.ScreenToWorldPoint(_mousePositions[0]);
                _mousePositions[1] = Input.mousePosition;
                var endPos = Camera.main.ScreenToWorldPoint(_mousePositions[1]);

                if (Math.Abs(beginPos.x - endPos.x) > 1)
                {
                    var topCorner = Math.Min(beginPos.x, endPos.x);
                    var bottomCorner = Math.Min(beginPos.y, endPos.y);
                    var width = Math.Max(beginPos.x, endPos.x) - topCorner;
                    var height = Math.Max(beginPos.y, endPos.y) - bottomCorner;
            
                    CreateWall(width, height, topCorner, bottomCorner); 
                }      
            } 
        } 
    }

    //Nous permet de tracer un rectangle de selection par dessus notre canvas pour voir ce que l'on trace
    void OnGUI()
    {
        if (_draggingMouse)
        {
            var rect = Utils.GetMousePositions(_mousePositions[0], Input.mousePosition);
            Utils.DrawMouseRect(rect, new Color( 0.8f, 0.8f, 0.95f, 0.25f ));
            Utils.DrawMouseRectBorder( rect, 2, new Color( 0.8f, 0.8f, 0.95f ) ); 
        }
    }
   
    //Create a wall using our page input fields
    private void CreateWall()
    {
        var model = new Model3D();
        model.CreateModel(0, 0, 0, GetFloatValueFromInput("input_length"), GetFloatValueFromInput("input_height"), GetFloatValueFromInput("input_width"), "Wall" + _wallNum, "Green");
        model.Model.gameObject.tag = "FixiWalls";
        modelSList.Add(model);
        _wallNum++;
        #if !UNITY_EDITOR && UNITY_WEBGL
            SendWallsList();
        #endif
    }

    //Create a wall with our mouse
    private void CreateWall(float width, float height, float topCornerPos, float bottomCornerPos)
    {
        var model = new Model3D();
        model.CreateModel(topCornerPos, bottomCornerPos, _posZ, width, height, 2, "Wall" + _wallNum, "Green");
        model.Model.gameObject.tag = "FixiWalls";
        modelSList.Add(model);
        _wallNum++;
        _posZ += 10;
        #if !UNITY_EDITOR && UNITY_WEBGL
            SendWallsList();
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
                item.Size.x, item.Size.y, item.Size.z, "Wall" + _wallNum, "Green");
            _wallNum++;
            copyWall.Model.gameObject.tag = "FixiWalls";
            modelSList.Add(copyWall);
            #if !UNITY_EDITOR && UNITY_WEBGL
                SendWallsList();
            #endif
        }
    }

    private void EditWall(string selectedWall)
    {
        var model = JsonUtility.FromJson<SzModel>(selectedWall);
        foreach (var item in modelSList.Where(x => x.Name == model.modelName))
        {
            var size = item.Model.GetComponent<Renderer>().bounds.size;
            var rescale = item.Model.transform.localScale;
            var newSize = new Vector3(GetFloatValueFromInput("input_edit_length"), GetFloatValueFromInput("input_edit_height"), GetFloatValueFromInput("input_edit_width"));
            rescale.x = newSize.x * rescale.x / size.x;
            rescale.y = newSize.y * rescale.y / size.y;
            rescale.z = newSize.z * rescale.z / size.z;
            item.Model.transform.localScale = rescale;
            item.Size = newSize;
            #if !UNITY_EDITOR && UNITY_WEBGL
                SendWallsList();
            #endif
        }
    }
 
    //Destroy the Wall selected
    private void RemoveWall(string selectedWall)
    {
        var model = JsonUtility.FromJson<SzModel>(selectedWall);
        foreach (var item in modelSList.Where(x => x.Name == model.modelName))
        {
            var hiddenWall = item.Model;
            hiddenWall.SetActive(false);
            hiddenWall.tag = "Untagged";
            modelSList.Remove(item);
            #if !UNITY_EDITOR && UNITY_WEBGL
                    SendWallsList();
            #endif
        }
    }

    //Method that takes our C# walls list and send it back to our webpage using pointers to the adress of the list
    public void SendWallsList()
    {
        //We need to have a simple serializable object
        var szModelList = new List<SzModel>();
        foreach (var item in modelSList)
        {
            var newWall = new SzModel {modelName = item.Name, modelSize = item.Size};
            szModelList.Add(newWall);
        }

        //We serialize our list of simple objects and pass it back to our html
        SendWallsToPage(JsonHelper.ToJson(szModelList.ToArray()));
    }
}