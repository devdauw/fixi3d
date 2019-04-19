using Fixi3d.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Settings : Singleton<Settings>    
{
    public int wallLayer;
    public int holeLayer;
    public LineRenderer lineRendererPrefab;
    //public LineRenderer lineRendererPrefab;


    //private void Start()
    //{
    //    var lineRenderer = Instantiate(Settings.Instance.lineRendererPrefab).GetComponent<LineRenderer>();
    //    var vectorsArray = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1) };
    //    lineRenderer.SetPositions(vectorsArray);
    //}
}
