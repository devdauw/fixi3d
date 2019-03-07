using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model3D
{
    private Vector3 _position;
    private Vector3 _size;
    private string _name;
    private Material _material;

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
        Material = Resources.Load("Materials/" + materialName, typeof(Material)) as Material;
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
        Model.transform.position = Position;
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
