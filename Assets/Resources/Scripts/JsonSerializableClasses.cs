using System;
using System.Collections.Generic;
using UnityEngine;

public class JsonSerializableClasses : MonoBehaviour
{
}

public class SzFixations
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;

    [NonSerialized]
    public GameObject gameObject;
}

[Serializable]
public class SzModel
{
    public string modelName;
    public Vector3 modelSize;
    public Vector3 modelPosition;
    public string[] modelFixationsName;
    public Vector3[] modelFixationsPosition;
    public Vector3[] vertices;
    public int[] triangles;
}

[Serializable]
public class SzProject
{
    public string projectName;
    public int projectNum;
    public string customerName;
    public string userName;
    public List<SzModel> wallList;
}