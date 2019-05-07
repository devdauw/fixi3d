using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Fixi3d.Utilities;
using Resources.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class WallCreator : Singleton<WallCreator>
{
    public bool substract;
    public bool renfort;

    #region Enum
    public enum MatColor
    {
       Green,
       Pink,
       Red
    }
    #endregion

    #region UnityWebGLCom
    //We import our methods from js_cross. Somes are pure js calls to grab data from the page. Others are calls sent from our C# to get data back in our page
    [DllImport("__Internal")]
    private static extern float GetFloatValueFromInput(string input_name);
    [DllImport("__Internal")]
    private static extern string Download(string json);
    //This method is used to get data back in our page, in this case we pass back the list of objects we created
    [DllImport("__Internal")]
    private static extern void SendWallsToPage(string cSharpList);
    //This method is used for sedding the project info to the Project Button
    [DllImport("__Internal")]
    private static extern void SendProjectInfo(string project);
    #endregion
    
    #region MouseCreation
    private const float PosZ = 0;
    private readonly Vector3[] _mousePositions = new Vector3[2];
    private bool _draggingMouse = false;
    #endregion

    public float grid = 0.1f;
    public List<Model3D> modelSList = new List<Model3D>();
    private static int _wallNum = 0;
    public SzProject[] save = new SzProject[1];

    private void Start() {
        //We disable the capture keyboard function from the WebGL plugin, otherwise we would not be able to communicate with our webpage using JS (our inputs would not take keyboard)    
        #if !UNITY_EDITOR && UNITY_WEBGL
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;
        #endif
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        //Check pour verifier que l'on est sur le plan en 2D afin d'eviter les null exception
        if (Camera.main == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            _draggingMouse = true;
            _mousePositions[0] = Input.mousePosition;
        }

        if (!Input.GetMouseButtonUp(0)) return;
        _draggingMouse = false;
            
        //On transform les donnees de notre souris pour les faire correspondre aux coordonnees de notre monde
        var beginPos = GetWorldPointSnappedMousePosition(Camera.main.ScreenToWorldPoint(_mousePositions[0]));
        _mousePositions[1] = Input.mousePosition;
        var endPos = GetWorldPointSnappedMousePosition(Camera.main.ScreenToWorldPoint(_mousePositions[1]));

        if (!(Math.Abs(beginPos.x - endPos.x) > 1)) return;
        var topCorner = Math.Min(beginPos.x, endPos.x);
        var bottomCorner = Math.Min(beginPos.y, endPos.y);
        var width = Math.Max(beginPos.x, endPos.x) - topCorner;
        var height = Math.Max(beginPos.y, endPos.y) - bottomCorner;

        if (substract)
            CreateHole(width, height, topCorner, bottomCorner);
        if (renfort)
        {
            CreateRenfort(width, height, topCorner, bottomCorner);
        }
        else
            CreateWall(width, height, topCorner, bottomCorner);
    }

    //Allow us to draw a selection square above our canvas
    void OnGUI()
    {
        if (!_draggingMouse) return;
        var beginPos = GetWorldPointSnappedMousePosition(Camera.main.ScreenToWorldPoint(_mousePositions[0]));
        beginPos = Camera.main.WorldToScreenPoint(beginPos);
        var endPos = GetWorldPointSnappedMousePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        endPos = Camera.main.WorldToScreenPoint(endPos);
        var rect = Utils.GetMousePositions(beginPos, endPos);
        Utils.DrawMouseRect(rect, new Color( 0.8f, 0.8f, 0.95f, 0.25f ));
        Utils.DrawMouseRectBorder( rect, 2, new Color( 0.8f, 0.8f, 0.95f ));
    }

    private Vector3 GetWorldPointSnappedMousePosition(Vector3 mousePos)
    {
        float x = 0f, y = 0f, z = mousePos.z;
        //5f == 0.1 to the screen
        var reciprocalGrid = 12.5f / grid;
        x = (float)Math.Round(mousePos.x / reciprocalGrid, 2) * reciprocalGrid;
        y = (float)Math.Round(mousePos.y / reciprocalGrid, 2) * reciprocalGrid;
        mousePos = new Vector3(x, y, z);
        return mousePos;
    }

    public void SaveProject(string val)
    {
        var json = JsonUtility.FromJson<SzProject>(val);
        save[0].projectName = json.projectName;
        save[0].projectNum = json.projectNum;
        save[0].customerName = json.customerName;
        save[0].userName = json.userName;
        Download(ProjectToJson());
    }

    public void LoadProject(string val)
    {
        RemoveWalls();
        _wallNum = 0;
        var json = JsonUtility.FromJson<SzProject>(val);
        save[0].projectName = json.projectName;
        save[0].projectNum = json.projectNum;
        save[0].customerName = json.customerName;
        save[0].userName = json.userName;
        foreach (var item in json.wallList)
            CreateWall(item.modelName, item.modelSize, item.modelPosition, item.modelFixationsName,
                item.modelFixationsPosition, item.vertices, item.triangles);
        #if !UNITY_EDITOR && UNITY_WEBGL
            SendWallsList();
        #endif
        SendProjectInfo(val);
    }

    //Create a wall using our page input fields
    private void CreateWall()
    {
        var model = new Model3D();
        try
        {
            model.CreateModel(0, 0, 0, GetFloatValueFromInput("input_length"), GetFloatValueFromInput("input_height"), GetFloatValueFromInput("input_width"), "Wall" + _wallNum, "Green");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        model.Model.gameObject.tag = "FixiWalls";
        model.Model.layer = Settings.Instance.wallLayer;
        model.Model.AddComponent<WallSelector>();
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
        model.CreateModel(topCornerPos, bottomCornerPos, PosZ, width, height, 2, "Wall" + _wallNum, MatColor.Green.ToString());
        model.Model.gameObject.tag = "FixiWalls";
        model.Model.layer = Settings.Instance.wallLayer;

        model.Model.AddComponent<WallSelector>();

        modelSList.Add(model);
        _wallNum++;
        #if !UNITY_EDITOR && UNITY_WEBGL
            SendWallsList();
        #endif
    }

    private void CreateRenfort(float width, float height, float topCornerPos, float bottomCornerPos)
    {
        var model = new Model3D();
        model.CreateModel(topCornerPos, bottomCornerPos, PosZ, width, height, 2, "Wall" + _wallNum, "Green");
        model.Model.AddComponent<RenfortMaker>().Init();
    }
    
    private void CreateHole(float width, float height, float topCornerPos, float bottomCornerPos)
    {
        var model = new Model3D();
        var wallName = "Wall" + _wallNum;
        model.CreateModel(topCornerPos, bottomCornerPos, PosZ, width, height, 2, wallName, MatColor.Green.ToString());
        model.Model.AddComponent<HoleMaker>().Init();
        var newHole = modelSList.Where(x => x.Name == wallName);
        newHole.First().Model = model.Model;
        #if !UNITY_EDITOR && UNITY_WEBGL
            SendWallsList();
        #endif
    }

    private void CreateWall(string name, Vector3 size, Vector3 position, string[] fixName, Vector3[] fixPos, Vector3[] vertices, int[] triangles)
    {
        var model = new Model3D();

        model.CreateModel(position.x, position.y, position.z, size.x, size.y, size.z, name, MatColor.Green.ToString(), fixName, fixPos);

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        model.Model.GetComponent<MeshFilter>().sharedMesh = mesh;

        model.Model.gameObject.tag = "FixiWalls";
        model.Model.layer = Settings.Instance.wallLayer;
        model.Model.AddComponent<WallSelector>();
        modelSList.Add(model);
        _wallNum++;
        #if !UNITY_EDITOR && UNITY_WEBGL
            SendWallsList();
        #endif
    }

    public void PlaceFixation(string wallName)
    {
        var wall = modelSList.Where(x => x.Name == wallName);
        if (wall.First().Model.transform.childCount == 0)
            wall.First().InitFixations();
        else
            wall.First().EditFixations();

        #if !UNITY_EDITOR && UNITY_WEBGL
            SendWallsList();
        #endif
    }

    public void Substract(string value)
    {
        substract = Boolean.Parse(value);
    }

    public void AddRenfort(string value)
    {
        renfort = Boolean.Parse(value);
    }

    public Model3D GetWall(string name)
    {
        return modelSList.FirstOrDefault(wall => wall.Name == name);
    }

    //Copy Paste function
    public void CopyWall(string selectedWall)
    {
        var model = JsonUtility.FromJson<SzModel>(selectedWall);
        var selector = GameObject.Find("MouseManager");
        selector.SendMessage("ClearSelection");
        var newPos = model.modelPosition;
        newPos.x = newPos.x + model.modelSize.x + GetFloatValueFromInput("input_edit_distance");
        var newFixPos = model.modelFixationsPosition;
        for (var i = 0; i < model.modelFixationsPosition.Length; i++)
        {
            model.modelFixationsPosition[i].x =  model.modelFixationsPosition[i].x + model.modelSize.x + GetFloatValueFromInput("input_edit_distance");
        }
        var wallName = "Wall" + _wallNum;
        CreateWall(wallName, model.modelSize, newPos, model.modelFixationsName, model.modelFixationsPosition, model.vertices, model.triangles);
        var newWall = modelSList.Where(x => x.Name == wallName).ToList();
        selector.SendMessage("SelectObject", newWall.First().Model);
        selector.SendMessage("ClearSelection");
    }

    private void EditWall(string selectedWall)
    {
        var model = JsonUtility.FromJson<SzModel>(selectedWall);
        var walls = modelSList.Where(x => x.Name == model.modelName).ToList();
        var size = walls.First().Model.GetComponent<Renderer>().bounds.size;
        var rescale = walls.First().Model.transform.localScale;
        var newSize = new Vector3(GetFloatValueFromInput("input_edit_length"), GetFloatValueFromInput("input_edit_height"), GetFloatValueFromInput("input_edit_width"));
        rescale.x = newSize.x * rescale.x / size.x;
        rescale.y = newSize.y * rescale.y / size.y;
        rescale.z = newSize.z * rescale.z / size.z;
        var newPosition = new Vector3(GetFloatValueFromInput("input_edit_posX"), GetFloatValueFromInput("input_edit_posY"), GetFloatValueFromInput("input_edit_posZ"));
        walls.First().Model.transform.position = newPosition;
        var baseVertices = walls.First().Model.GetComponent<MeshFilter>().sharedMesh.vertices;
        var vertices = new Vector3[baseVertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = baseVertices[i];
            vertex.x = vertex.x * rescale.x;
            vertex.y = vertex.y * rescale.y;
            vertex.z = vertex.z * rescale.z;
            vertices[i] = vertex;
        }
        walls.First().Model.GetComponent<MeshFilter>().sharedMesh.vertices = vertices;
        walls.First().Model.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
        walls.First().Model.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
        walls.First().Size = newSize;    
        walls.First().Model.GetComponent<WallSelector>().unSelect();
        walls.First().Model.GetComponent<WallSelector>().Select();
        if (walls.First().Model.transform.childCount != 0)
            PlaceFixation(walls.First().Name);
        #if !UNITY_EDITOR && UNITY_WEBGL
            SendWallsList();
        #endif
    }

    //Destroy the Wall selected
    public void RemoveWall(string selectedWall)
    {
        var model = JsonUtility.FromJson<SzModel>(selectedWall);
        RemoveWallWithName(model.modelName);
        Debug.Log("Removed");
    }

    public void RemoveWallWithName(string wallName)
    {
        var selector = GameObject.Find("MouseManager");
        var walls = modelSList.Where(x => x.Name == wallName).ToList();
        selector.SendMessage("ClearSelection");
        var hiddenWall = walls.First().Model;
        Destroy(hiddenWall);
        modelSList.Remove(walls.First());
        #if !UNITY_EDITOR && UNITY_WEBGL
            SendWallsList();
        #endif
    }

    private void RemoveWalls()
    {
        foreach (var t in modelSList)
            Destroy(t.Model);
        modelSList.Clear();
    }

    public void UpdateWallBounds(string name)
    {
        var wallToUpdate = modelSList.FirstOrDefault(wall => wall.Name == name);
        if (wallToUpdate == null)
            return;

        var bounds = wallToUpdate.Model.GetComponent<MeshFilter>().sharedMesh.bounds;
        wallToUpdate.BackLeftBottom = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
    }

    //Method that takes our C# walls list and send it back to our webpage using pointers to the address of the list
    public void SendWallsList()
    {
        SendWallsToPage(ProjectToJson());
    }

    public string ProjectToJson()
    {
        //We need to have a simple serializable object
        var szModelList = new List<SzModel>();
        var project = new SzProject();
        foreach (var item in modelSList)
        {
            Debug.Log(item.Name);
            var mesh = item.Model.GetComponent<MeshFilter>().mesh;
            var newWall = new SzModel
            {
                modelName = item.Name,
                modelSize = item.Size,
                modelPosition = item.Model.transform.position,
                modelFixationsName = new string[item.Model.transform.childCount],
                modelFixationsPosition = new Vector3[item.Model.transform.childCount],
                vertices = mesh.vertices,
                triangles = mesh.triangles
            };
            for (var i = 0; i < item.Model.transform.childCount; i++)
            {
                newWall.modelFixationsName[i] = item.Model.transform.GetChild(i).name;
                newWall.modelFixationsPosition[i] = item.Model.transform.GetChild(i).position;
            }
            szModelList.Add(newWall);
        }

        project.projectName = save[0].projectName;
        project.projectNum = save[0].projectNum;
        project.customerName = save[0].customerName;
        project.userName = save[0].userName;
        project.wallList = szModelList;


        //We serialize our list of simple objects and pass it back to our html
        var json = JsonUtility.ToJson(project);
        Debug.Log("json");
        Debug.Log(json);
        return json;
    }
}