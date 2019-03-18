using UnityEngine;

namespace Resources.Scripts.Controllers
{
    public class ObjectSelector : MonoBehaviour
    {
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
        }
    }
}