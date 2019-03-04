using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEditor : MonoBehaviour
{
    public void EditWall(string wallName, float? newLength, float? newHeigth)
    {
        GameObject gameObject_edit = GameObject.Find(wallName);

        float x = (newLength.HasValue) ? newLength.Value : gameObject_edit.transform.localScale.x;
        float y = (newHeigth.HasValue) ? newHeigth.Value : gameObject_edit.transform.localScale.y;

        gameObject_edit.transform.localScale = new Vector3(x, y, 1f);
    }
}
