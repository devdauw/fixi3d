using UnityEngine;
using System.ComponentModel;
using cakeslice;
using Object = UnityEngine.Object;

public class Model3D
{
    private Vector3 _position;
    private Vector3 _size;
    private string _name;
    private Material _material;
    private readonly GameObject _suspente = (GameObject)UnityEngine.Resources.Load("Fixations/Suspente");
    private readonly GameObject _distM20 = (GameObject)UnityEngine.Resources.Load("Fixations/DistanceurM20");
    private bool first = true;

    private enum FixationType
    {
        [Description("Suspente")]
        Suspente,
        DistanceurM20
    }

    public Vector3 Position { get; set; }
    public Vector3 Size { get; set; }
    public string Name { get; set; }
    public Material Material { get; set; }
    public GameObject Model { get; set; }

    #region Vertices
    public Vector3 FrontLeftBottom { get; set; }
    public Vector3 FrontLeftTop { get; set; }
    public Vector3 FrontRightTop { get; set; }
    public Vector3 FrontRightBottom { get; set; }
    public Vector3 BackRightBottom { get; set; }
    public Vector3 BackLeftBottom { get; set; }
    public Vector3 BackLeftTop { get; set; }
    public Vector3 BackRightTop { get; set; }
    #endregion

    public void CreateModel(float posX, float posY, float posZ, float sizeX, float sizeY, float sizeZ, string name, string materialName)
    {
        Position = new Vector3(posX, posY, posZ);
        Size = new Vector3(sizeX, sizeY, sizeZ);
        Name = name;
        Material = UnityEngine.Resources.Load<Material>("Materials/" + materialName);
        FrontLeftBottom = new Vector3(0, 0, 0);
        FrontLeftTop = new Vector3(0, Size.y, 0);
        FrontRightTop = new Vector3(Size.x, Size.y, 0);
        FrontRightBottom = new Vector3(Size.x, 0, 0);
        BackRightBottom = new Vector3(0, 0, Size.z);
        BackLeftBottom = new Vector3(Size.x, 0, Size.z);
        BackLeftTop = new Vector3(Size.x, Size.y, Size.z);
        BackRightTop = new Vector3(0, Size.y, Size.z);

        var vertices = GetVertices();
        var triangles = GetTriangles();
        var uvs = new Vector2[vertices.Length];
        for (var i = 0; i < uvs.Length; i++)
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        var mesh = new Mesh { vertices = vertices, triangles = triangles, uv = uvs };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        Model = new GameObject(Name, typeof(MeshFilter), typeof(MeshRenderer));
        Model.GetComponent<MeshRenderer>().material = Material;
        Model.GetComponent<MeshFilter>().mesh = mesh;
        //Using a MeshCollider give us the option to select our object later
        Model.AddComponent<MeshCollider>();
        Model.GetComponent<MeshCollider>().convex = true;
        var outline = Model.AddComponent<Outline>();
        outline.enabled = false;

        Model.transform.position = Position;

        var fixPosX = Model.gameObject.transform.position.x;
        var fixPosY = Model.gameObject.transform.position.y;
        var fixPosZ = Model.gameObject.transform.position.z;

        var suspXLenght = 0.60299994f;
        var suspYLenght = 0.88800513f;
        var distXLenght = 0.08148051f;
        var distYLenght = 0.28499997f;

        //Susp Left
        var suspLeftX = fixPosX + (sizeX * 0.2f) + suspXLenght;
        var suspLeftY = fixPosY + sizeY - 0.25f - suspYLenght;
        InstantiateFixation("Susp", "Left Suspente", suspLeftX, suspLeftY, fixPosZ);

        //Dist Top Left
        var distTopLeftX = suspLeftX - 0.15f - suspXLenght;
        var distTopLeftY = fixPosY + sizeY - 0.2f - (distXLenght / 2);
        InstantiateFixation("DistM20", "TopLeft Distanceur", distTopLeftX, distTopLeftY, fixPosZ);

        //Susp Right
        var suspRightX = fixPosX + (sizeX * 0.8f);
        var suspRightY = fixPosY + sizeY - 0.25f - suspYLenght;
        InstantiateFixation("Susp", "Right Suspente", suspRightX, suspRightY, fixPosZ);

        //Dist Top Right
        var distTopRightX = suspRightX + 0.15f;
        var distTopRightY = fixPosY + sizeY - 0.2f - (distXLenght / 2);
        InstantiateFixation("DistM20", "TopRight Distanceur", distTopRightX, distTopRightY, fixPosZ);

        //Dist Bottom Left
        var distBottomLeftX = fixPosX + (sizeX * 0.2f) - 0.2f;
        if (distBottomLeftX - fixPosX < 15)
            distBottomLeftX = fixPosX + 0.15f;
        var distBottomLeftY = fixPosY + 0.15f;
        InstantiateFixation("DistM20", "BottomLeft Distanceur", distBottomLeftX, distBottomLeftY, fixPosZ);

        //Dist Bottom Right
        var distBottomRightX = fixPosX + (sizeX * 0.8f) + 0.2f;
        if ((fixPosX + sizeX) - distBottomRightX < 15)
            distBottomRightX = fixPosX + sizeX - 0.15f - distXLenght;
        var distBottomRightY = fixPosY + 0.15f;
        InstantiateFixation("DistM20", "BottomRight Distanceur", distBottomRightX, distBottomRightY, 0f);
    }

    public void CreateModel(float posX, float posY, float posZ, float sizeX, float sizeY, float sizeZ, string name, string materialName, string[] fixName, Vector3[] fixPos)
    {
        Position = new Vector3(posX, posY, posZ);
        Size = new Vector3(sizeX, sizeY, sizeZ);
        Name = name;
        Material = UnityEngine.Resources.Load<Material>("Materials/" + materialName);
        FrontLeftBottom = new Vector3(0, 0, 0);
        FrontLeftTop = new Vector3(0, Size.y, 0);
        FrontRightTop = new Vector3(Size.x, Size.y, 0);
        FrontRightBottom = new Vector3(Size.x, 0, 0);
        BackRightBottom = new Vector3(0, 0, Size.z);
        BackLeftBottom = new Vector3(Size.x, 0, Size.z);
        BackLeftTop = new Vector3(Size.x, Size.y, Size.z);
        BackRightTop = new Vector3(0, Size.y, Size.z);

        var vertices = GetVertices();
        var triangles = GetTriangles();
        var uvs = new Vector2[vertices.Length];
        for (var i = 0; i < uvs.Length; i++)
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        var mesh = new Mesh { vertices = vertices, triangles = triangles, uv = uvs };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        Model = new GameObject(Name, typeof(MeshFilter), typeof(MeshRenderer));
        Model.GetComponent<MeshRenderer>().material = Material;
        Model.GetComponent<MeshFilter>().mesh = mesh;
        //Using a BoxCollider give us the option to select our object later
        Model.AddComponent<MeshCollider>();
        Model.GetComponent<MeshCollider>().convex = true;
        Model.AddComponent<Outline>();
        Model.transform.position = Position;

        var suspXLenght = 0.60299994f;
        var suspYLenght = 0.88800513f;
        var distXLenght = 0.08148051f;
        var distYLenght = 0.28499997f;

        for (var i = 0; i < fixName.Length; i++)
        {
            var type = fixName[i].Split(' ');
            switch (type[1])
            {
                case "Suspente":
                {
                    InstantiateFixation("Susp", fixName[i], fixPos[i].x, fixPos[i].y, fixPos[i].z + 0.23f);
                    break;
                }
                    
                case "Distanceur":
                    InstantiateFixation("DistM20", fixName[i], fixPos[i].x - 0.046f, fixPos[i].y - 0.0014f + 0.03f, fixPos[i].z - 0.086f);
                    break;
            }
        }
    }

    private void InstantiateFixation(string fixType, string name, float x , float y, float z)
    {
        GameObject fixation;
        switch (fixType)
        {
            case "DistM20":
                fixation = (GameObject) Object.Instantiate(_distM20);
                fixation.transform.Rotate(-60.69f, 91.186f, -92.762f);
                fixation.transform.position = new Vector3(x + 0.046f, y + 0.0014f - 0.03f, z + 0.086f); 
                break;
            case "Susp":
                fixation = (GameObject) Object.Instantiate(_suspente);
                fixation.transform.position = new Vector3(x, y, z - 0.23f);
                break;
            default:
                fixation = new GameObject();
                break;
        }
        fixation.name = name + " " + Model.name;
        fixation.transform.parent = Model.transform;
    }

    private Vector3[] GetVertices()
    {
        var vert = new Vector3[]
        {
            //Front Face
            FrontLeftBottom,
            FrontLeftTop,
            FrontRightTop,
            FrontRightBottom,

            //Back Face
            BackRightBottom,
            BackLeftBottom,
            BackLeftTop,
            BackRightTop,

            //Left Face
            FrontLeftBottom,
            BackRightBottom,
            BackRightTop,
            FrontLeftTop,

            //Right Face
            FrontRightBottom,
            FrontRightTop,
            BackLeftTop,
            BackLeftBottom,

            //Top Face
            FrontLeftTop,
            BackRightTop,
            BackLeftTop,
            FrontRightTop,

            //Bottom Face
            BackRightBottom,
            FrontLeftBottom,
            FrontRightBottom,
            BackLeftBottom
        };
        return vert;
    }

    private int[] GetTriangles()
    {
        var triangles = new int[]
        {
            // Front Face
            0,1,2, //First Triangle
            0,2,3, //Second Triangle

            // Back Face
            4,5,6, //First Triangle
            4,6,7, //Second Triangle

            // Left Face
            8,9,10, //First Triangle
            8,10,11, //Second Triangle

            // Right Face
            12,13,14, //First Triangle
            12,14,15, //Second Triangle

            // Top Face
            16,17,18, //First Triangle
            16,18,19, //Second Triangle

            // Bottom Face
            20,21,22, //First Triangle
            20,22,23 //Second Triangle
        };
        return triangles;
    }
}
