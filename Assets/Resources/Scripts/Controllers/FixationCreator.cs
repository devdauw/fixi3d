using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixationCreator : MonoBehaviour
{
    public void CreateFix(Vector3 wallSize, Vector3 wallPosition)
    {
        var prefab = UnityEngine.Resources.Load("Fixations/Suspente");
        var fixPosition = new Vector3(wallPosition.x, wallPosition.y, wallPosition.z - 3);
        var gbFix = (GameObject)Instantiate(prefab, fixPosition, Quaternion.identity);
        /*
        Camera cam = t.AddComponent<Camera>();
        cam.transform.position = new Vector3(11.16f, 6.59f, -2.62f);
        cam.transform.Rotate(new Vector3(18.141f, -33.983f, 1.991f));
        */
    }
}
