using MeshMakerNamespace;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestCSG : MonoBehaviour
{
    public bool substract;
    public GameObject cube1;
    public GameObject cube2;

    private void Update()
    {
        if (substract)
        {
            substract = false;
            Mesh newMesh = CSG.Union(cube1, cube2, false, false);
            newMesh.RecalculateNormals();
            newMesh.RecalculateBounds();

            List<Vector3> vertices = newMesh.vertices.ToList();
            List<int> triangles = newMesh.triangles.ToList();

            Mesh newMesh2 = new Mesh();
            newMesh2.vertices = vertices.ToArray();
            newMesh2.triangles = triangles.ToArray();
            newMesh2.RecalculateBounds();
            newMesh2.RecalculateNormals();

            cube1.GetComponent<MeshFilter>().sharedMesh = newMesh;
        }
    }
}
