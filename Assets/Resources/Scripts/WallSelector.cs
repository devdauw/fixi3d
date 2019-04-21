using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class WallSelector : MonoBehaviour
{
    private List<GameObject> lineRenderers = new List<GameObject>();

    public void Select()
    {
        GetComponent<Outline>().enabled = true;
        var bounds = GetComponent<MeshFilter>().sharedMesh.bounds;
        var color = Color.black;

        var line1 = Instantiate(Settings.Instance.lineRendererPrefab).GetComponent<LineRenderer>();
        var pos1 = transform.position + new Vector3(bounds.min.x, bounds.max.y + 0.5f, bounds.center.z);
        var pos2 = transform.position + new Vector3(bounds.max.x, bounds.max.y + 0.5f, bounds.center.z);
        line1.SetPositions(new Vector3[] {pos1, pos2});
        var textMesh = line1.gameObject.GetComponentInChildren<TextMesh>();
        textMesh.transform.position = Vector3.Lerp(pos1, pos2, 0.5f);
        textMesh.text = bounds.size.x.ToString();
        textMesh.color = color;

        var line2 = Instantiate(Settings.Instance.lineRendererPrefab).GetComponent<LineRenderer>();
        var pos1l2 = transform.position + new Vector3(bounds.min.x - 0.5f, bounds.min.y, bounds.center.z);
        var pos2l2 = transform.position + new Vector3(bounds.min.x - 0.5f, bounds.max.y, bounds.center.z);
        line2.SetPositions(new Vector3[] {pos1l2, pos2l2});
        var textMesh2 = line2.gameObject.GetComponentInChildren<TextMesh>();
        textMesh2.transform.position = Vector3.Lerp(pos1l2, pos2l2, 0.5f);
        textMesh2.transform.rotation = new Quaternion(textMesh2.transform.rotation.x, textMesh2.transform.rotation.y, textMesh2.transform.rotation.z + 1f, textMesh2.transform.rotation.w);
        textMesh2.text = bounds.size.y.ToString();
        textMesh2.color = color;

        lineRenderers.Add(line1.gameObject);
        lineRenderers.Add(line2.gameObject);
    }

    public void unSelect()
    {
        GetComponent<Outline>().enabled = false;
        foreach (var item in lineRenderers)
        {
            Destroy(item);
        }

        lineRenderers = new List<GameObject>();
    }
}
