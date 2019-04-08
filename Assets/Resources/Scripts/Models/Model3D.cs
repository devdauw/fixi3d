using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Model3D
{
    private Vector3 _position;
    private Vector3 _size;
    private string _name;
    private Material _material;
    private GameObject _suspente = (GameObject)UnityEngine.Resources.Load("Fixations/Suspente");

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

        Vector3[] vertices = GetVertices();
        int[] triangles = GetTriangles();
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        Mesh mesh = new Mesh { vertices = vertices, triangles = triangles, uv = uvs };

        mesh.RecalculateNormals();

        Model = new GameObject(Name, typeof(MeshFilter), typeof(MeshRenderer));
        Model.GetComponent<MeshRenderer>().material = Material;
        Model.GetComponent<MeshFilter>().mesh = mesh;
        //Using a BoxCollider give us the option to select our object later
        Model.AddComponent<BoxCollider>();
        Model.transform.position = Position;

        var fixPosX = Model.gameObject.transform.position.x;
        // +0.35f == la moitier de la taille d'une suspente pour que celle-ci se retrouve bien centrer
        var fixLeftPosX = fixPosX + ((sizeX * 0.2f) + 0.35f);
        var fixRightPosX = fixPosX + ((sizeX * 0.8f) + 0.35f);
        var fixPosY = Model.gameObject.transform.position.y;
        var fixTopPosY = fixPosY + sizeY - 0.25f - 0.4f;
        var fixCenterPosY = fixPosY + ((sizeY * 0.5f) - 0.4f);
        var fixBottomPosY = fixPosY + 0.25f;
        var fixPosZ = Model.gameObject.transform.position.z - 0.23f;

        //Dist Top Left
        var distTopLeftX = fixPosX + 0.15f;
        var distTopLeftY = fixPosY + 0.2f;
        //Susp Left
        var suspLeftX = fixPosX + (sizeX * 0.2f);
        if (suspLeftX - distTopLeftX < 15 || suspLeftX - distTopLeftX > 20)
            suspLeftX = distTopLeftX + 0.15f;
        var suspLeftY = fixPosY + sizeY - 0.25f;
        //Dist Top Right
        var distTopRightX = fixPosX + sizeX - 0.15f;
        var distTopRightY = fixPosY + sizeY - 0.2f;
        //Susp Right
        var suspRightX = fixPosX + (sizeX * 0.8f);
        if (suspRightX - distTopLeftX < 15 || suspRightX - distTopLeftX > 20)
            suspRightX = distTopRightX - 0.15f;
        var suspRightY = fixPosY + sizeY - 0.25f;
        //Dist Bottom Left
        var distBottomLeftX = fixPosX + (sizeX * 0.2f) - 0.2f;
        if (distBottomLeftX - fixPosX < 15)
            distBottomLeftX = fixPosX + 0.15f;
        var distBottomLeftY = fixPosY + 0.15f;
        //Dist Bottom Right
        var distBottomRightX = fixPosX + (sizeX * 0.8f) + 0.2f;
        if ((fixPosX + sizeX) - distBottomRightX < 15)
            distBottomRightX = fixPosX + sizeX - 0.15f;
        var distBottomRightY = fixPosY + 0.15f;

        //Top Left Fixation
        var Suspente_TopLeft = (GameObject)Object.Instantiate(_suspente);
        Suspente_TopLeft.transform.position = new Vector3(fixLeftPosX, fixTopPosY, fixPosZ);
        Suspente_TopLeft.name = "TopLeft Suspente " + Model.name;
        Suspente_TopLeft.transform.parent = Model.transform;

        //Top Right Fixation
        var Suspente_TopRight = (GameObject)Object.Instantiate(_suspente);
        Suspente_TopRight.transform.position = new Vector3(fixRightPosX, fixTopPosY, fixPosZ);
        Suspente_TopRight.name = "TopRight Suspente " + Model.name;
        Suspente_TopRight.transform.parent = Model.transform;

        //Left Fixation
        var Suspente_Left = (GameObject)Object.Instantiate(_suspente);
        Suspente_Left.transform.position = new Vector3(fixLeftPosX, fixCenterPosY, fixPosZ);
        Suspente_Left.name = "Left Suspente " + Model.name;
        Suspente_Left.transform.parent = Model.transform;

        //Right Fixation
        var Suspente_Right = (GameObject)Object.Instantiate(_suspente);
        Suspente_Right.transform.position = new Vector3(fixRightPosX, fixCenterPosY, fixPosZ);
        Suspente_Right.name = "Right Suspente " + Model.name;
        Suspente_Right.transform.parent = Model.transform;

        //BottomLeft Fixation
        var Suspente_BottomLeft = (GameObject)Object.Instantiate(_suspente);
        Suspente_BottomLeft.transform.position = new Vector3(fixLeftPosX, fixBottomPosY, fixPosZ);
        Suspente_BottomLeft.name = "BottomLeft Suspente " + Model.name;
        Suspente_BottomLeft.transform.parent = Model.transform;

        //BottomRight Fixation
        var Suspente_BottomRight = (GameObject)Object.Instantiate(_suspente);
        Suspente_BottomRight.transform.position = new Vector3(fixRightPosX, fixBottomPosY, fixPosZ);
        Suspente_BottomRight.name = "BottomRight Suspente " + Model.name;
        Suspente_BottomRight.transform.parent = Model.transform;
    }

    private Vector3[] GetVertices()
    {
        Vector3[] vert = new Vector3[]
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
        int[] triangles = new int[]
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
