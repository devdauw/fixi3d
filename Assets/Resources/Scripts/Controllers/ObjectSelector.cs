using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Resources.Scripts.Controllers
{
    public class ObjectSelector : MonoBehaviour
    { 
        [DllImport("__Internal")]
        private static extern void SendClickedWallToPage(string wallObject);
        [DllImport("__Internal")]
        private static extern void SendClear();

        private List<GameObject> _inactiveObjects = new List<GameObject>();
        private bool lineRendererCreated = false;
        public GameObject selectedObject;
        public Color startingColor;

        private void Update()
        {
            if (Camera.main == null) return;
            if (!Input.GetMouseButtonDown(0)) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                var hitObject = hitInfo.transform.root.gameObject;
                SelectObject(hitObject);
            }
            else
                ClearSelection();
        }

        private void SelectObject(GameObject obj)
        {
            if (lineRendererCreated)
                return;
            var ruler = obj.GetComponent<WallSelector>();
            if (ruler != null)
                ruler.Select();
            lineRendererCreated = true;

            if(selectedObject != null) {
                if(obj == selectedObject) return;
                ClearSelection();
            }
            selectedObject = obj;
            SendClickedWallToPage();
        }
        
        private void ClearSelection()
        {
            if(selectedObject == null) return;
            selectedObject.GetComponent<WallSelector>().unSelect();
            selectedObject = null;
            foreach (var gameObject in _inactiveObjects)
                gameObject.SetActive(true);
            var cameraRotator = GameObject.Find("Camera Rotator");
            cameraRotator.transform.position = new Vector3(0,0,0);
            lineRendererCreated = false;
            #if !UNITY_EDITOR && UNITY_WEBGL
                SendClear();
            #endif
        }

        public void SendClickedWallToPage()
        {
            var wall = new SzModel
            {
                modelName = selectedObject.name,
                modelSize = selectedObject.GetComponent<Renderer>().bounds.size,
                modelPosition = selectedObject.GetComponent<Renderer>().transform.position,
                modelFixationsName = new string[selectedObject.transform.childCount],
                modelFixationsPosition = new Vector3[selectedObject.transform.childCount],
                triangles = selectedObject.GetComponent<MeshFilter>().sharedMesh.triangles,
                vertices = selectedObject.GetComponent<MeshFilter>().sharedMesh.vertices
            };
            for (var i = 0; i < selectedObject.transform.childCount; i++)
            {
                wall.modelFixationsName[i] = selectedObject.transform.GetChild(i).name;
                wall.modelFixationsPosition[i] = selectedObject.transform.GetChild(i).position;
            }
            SendClickedWallToPage(JsonUtility.ToJson(wall));
            ShowSelectedWall();
        }

        public void ShowSelectedWall()
        {
            var disable = new List<GameObject>();
            var fixiWalls = GameObject.FindGameObjectsWithTag("FixiWalls");

            _inactiveObjects.Clear();
            foreach (var gameObject in fixiWalls)
            {
                if (gameObject.name == selectedObject.name) continue;
                disable.Add(gameObject);
                _inactiveObjects.Add(gameObject);
                gameObject.SetActive(false);
            }
            //Placement du GameObject Camera Rotator dans le mur sélectionner
            var cameraRotator = GameObject.Find("Camera Rotator");
            var size = selectedObject.GetComponent<Renderer>().bounds.size;
            var centerX = selectedObject.transform.position.x + (size.x / 2);
            var centerY = selectedObject.transform.position.y + (size.y / 2);
            var centerZ = selectedObject.transform.position.z + (size.z / 2);
            cameraRotator.transform.position = new Vector3(centerX, centerY, centerZ);
        }

        public void EnableLineRenderer(string val)
        {
            if (selectedObject != null)
                selectedObject.GetComponent<WallSelector>().ShowLineRenderer(Boolean.Parse(val));
        }
    }
}