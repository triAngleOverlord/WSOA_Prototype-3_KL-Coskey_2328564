using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private Vector3 lastPosition; //of mouse

    [SerializeField] private LayerMask placementLayer; // which layer takes part in detection


    public event Action OnClicked, OnExit;//inform other classes that the mouse has been clicked & exit placement mode when ESC is pressed

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();//safely call action event and see if smth is listening for it

        if(Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
            =>EventSystem.current.IsPointerOverGameObject();//return true/ false if pointer is over a UI object so when hovering over UI we cannot click

    

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

    public void ReloadHouse()
    {
        SceneManager.LoadScene(0);
    }
}
