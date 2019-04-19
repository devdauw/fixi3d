using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Resources.Scripts;
using UnityEditor;
using UnityEngine;

public class ShapeEditor : MonoBehaviour
{
    private ShapeCreator _shapeCreator;
    private Camera _mCamera;
    private Vector3 mousePosition;

    private void Start()
    {
        _mCamera = Camera.main;
        _shapeCreator = gameObject.AddComponent<ShapeCreator>();
    }



    void OnGUI()
    {
        Event guiEvent = Event.current;
        mousePosition = _mCamera.ScreenToWorldPoint(guiEvent.mousePosition);     
        
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            _shapeCreator.points.Add(mousePosition);
            Debug.Log("Add " + mousePosition);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(mousePosition, .5f);
    }
}
