using System.Collections;
using System.Collections.Generic;
using MeshMakerNamespace;
using UnityEngine;

public class RenfortMaker : MonoBehaviour
{
    private int time = 60;
    public void Init()
    {
        gameObject.layer = Settings.Instance.renfortLayer;
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        
        var colliderOfObject = gameObject.GetComponent<MeshCollider>();
        colliderOfObject.isTrigger = true;

        colliderOfObject.enabled = false;
        colliderOfObject.enabled = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Add(other.gameObject);
    }
    
    private void Add(GameObject toAdd)
    {
        var mesh = CSG.Union(toAdd, gameObject, false, false);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        toAdd.GetComponent<MeshFilter>().sharedMesh = mesh;
        //Recalculate box collider
        toAdd.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    
    private void Update()
    {
        time -= 1;
        if (time <= 0)
            Destroy(gameObject);
    }
}
