using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruler : MonoBehaviour
{
    private List<GameObject> lineRenderers = new List<GameObject>();

    public void ShowRuler()
    {
        var bounds = GetComponent<MeshFilter>().sharedMesh.bounds;

        var line1 = Instantiate(Settings.Instance.lineRendererPrefab).GetComponent<LineRenderer>();
        var pos1 = transform.position + new Vector3(bounds.min.x, bounds.max.y + 0.5f, bounds.center.z);
        var pos2 = transform.position + new Vector3(bounds.max.x, bounds.max.y + 0.5f, bounds.center.z);
        line1.SetPositions( new Vector3[] { pos1, pos2});
        var textMesh = line1.gameObject.GetComponentInChildren<TextMesh>();
        textMesh.transform.position = Vector3.Lerp(pos1, pos2, 0.5f);
        textMesh.text = "test";


        var line2 = Instantiate(Settings.Instance.lineRendererPrefab).GetComponent<LineRenderer>();
        line2.SetPositions(new Vector3[] { transform.position + new Vector3(bounds.min.x - 0.5f, bounds.min.y, bounds.center.z), transform.position + new Vector3(bounds.min.x - 0.5f, bounds.max.y, bounds.center.z)});


    }
}
