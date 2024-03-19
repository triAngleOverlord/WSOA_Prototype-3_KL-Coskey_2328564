using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private Vector3 lastPosition; //of mouse

    [SerializeField] private LayerMask placementLayer; // which layer takes part in detection

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _camera.nearClipPlane;
        Ray ray = _camera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayer))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }//Get mouse position and translate the z coordinate onto a layer that is rendered by the camera
     //and create a ray to the position to detecct the selected position
}
