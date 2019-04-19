﻿using MeshMakerNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMaker : MonoBehaviour
{
    private int time = 60;
    public void Init()
    {
        GetComponent<MeshRenderer>().enabled = false;
        gameObject.layer = Settings.Instance.holeLayer;
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        
        var collider = gameObject.GetComponent<BoxCollider>();
        collider.isTrigger = true;

        collider.enabled = false;
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        Substract(other.gameObject);
    }

    private void Substract(GameObject toSubstract)
    {
        var mesh = CSG.Subtract(toSubstract, gameObject, false, false);
        if (mesh == null || mesh.vertices == null || mesh.vertices.Length < 3)
        {
            WallCreator.Instance.RemoveWallWithName(toSubstract.gameObject.name);
            return;
        }

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        toSubstract.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        time -= 1;
        if (time <= 0)
            Destroy(gameObject);
    }
}
