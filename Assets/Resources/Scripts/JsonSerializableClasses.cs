using System;
using System.Collections.Generic;
using UnityEngine;

public class JsonSerializableClasses : MonoBehaviour
{
}

[Serializable]
public class SzModel
{
    public string modelName;
    public Vector3 modelSize;
    public Vector3 modelPosition;
    public string[] modelFixationsName;
    public Vector3[] modelFixationsPosition;
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