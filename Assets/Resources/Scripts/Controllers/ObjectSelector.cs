using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Resources.Scripts.Controllers
{
    public class ObjectSelector : MonoBehaviour
    { 
        [DllImport("__Internal")]
        private static extern void SendClickedWallToPage(string wallObject);

        private List<GameObject> inactiveObjects = new List<GameObject>();
        public GameObject selectedObject;
        public Color startingColor;
        private void Update()
        {
            if (Camera.main != null)
            {
                if (!Input.GetMouseButtonDown(0)) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    GameObject hitObject = hitInfo.transform.root.gameObject;
                    SelectObject(hitObject);
                }
                else
                {
                    ClearSelection();
                }
            } 
        }

        private void SelectObject(GameObject obj)
        {
            if(selectedObject != null) {
                if(obj == selectedObject)
                    return;

                ClearSelection();
            }

            selectedObject = obj;

            Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer>();
            foreach(Renderer r in rs) {
                Material m = r.material;
                startingColor = m.color;
                m.color = Color.magenta;
                r.material = m;
            }
            SendClickedWallToPage();
        }
        
        private void ClearSelection()
        {
            if(selectedObject == null)
                return;

            Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer>();
            foreach(Renderer r in rs) {
                Material m = r.material;
                m.color = startingColor;
                r.material = m;
            }
            selectedObject = null;
            foreach (GameObject gameObject in inactiveObjects)
            {
                gameObject.SetActive(true);
            }
        }

        public void SendClickedWallToPage()
        {
            SzModel wall = new SzModel();
            wall.modelName = selectedObject.name;
            wall.modelSize = selectedObject.GetComponent<Renderer>().bounds.size;
            wall.modelPosition = selectedObject.GetComponent<Renderer>().transform.position;
            
            SendClickedWallToPage(JsonUtility.ToJson(wall));
            ShowSelectedWall();
        }

        public void ShowSelectedWall()
        {
            List<GameObject> disable = new List<GameObject>();
            GameObject[] fixiWalls;
            fixiWalls = GameObject.FindGameObjectsWithTag("FixiWalls");
            inactiveObjects.Clear();
            
            foreach (GameObject gameObject in fixiWalls)
            {
                if (gameObject.name != selectedObject.name)
                {
                    disable.Add(gameObject);
                    inactiveObjects.Add(gameObject);
                    gameObject.SetActive(false);  
                }
            }
        }
    }
}