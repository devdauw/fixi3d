using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixationCreator : MonoBehaviour
{
    public void CreateFix()
    {
        Object prefab = UnityEngine.Resources.Load("Fixations/YFix");
        GameObject t = (GameObject)Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Camera cam = t.AddComponent<Camera>();
        cam.transform.position = new Vector3(11.16f, 6.59f, -2.62f);
        cam.transform.Rotate(new Vector3(18.141f, -33.983f, 1.991f));
    }
}
