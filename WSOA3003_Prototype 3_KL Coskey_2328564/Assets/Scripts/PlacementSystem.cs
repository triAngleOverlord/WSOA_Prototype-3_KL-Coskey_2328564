using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator;
    //visualize which position on the plane selected
    [SerializeField] GameObject cellIndicator;

    [SerializeField] private InputManager inputManager;

    [SerializeField] Grid grid;

    [SerializeField] private ObjectsDatabase database;
    public int selectedObjectIndex = -1;//if it is -1 (by default) no item is selected

    [SerializeField] private GameObject gridVisualisation;

    private void Start()
    {
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        //for loop when the data is one itam for the for loop and pass if the data.ID == int ID
        //this is a Lambda expression is =>
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");// the {} makes it an interpolated string
            return;
        }
        gridVisualisation.SetActive(true);
        cellIndicator.SetActive(true); //provide a preview od where to place this selected object
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }Debug.Log("Place");

        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mouseposition);
        GameObject newObject = Instantiate(database.objectData[selectedObjectIndex].Prefab);
        Debug.Log(newObject);
        newObject.transform.position = grid.CellToWorld(gridPos);
    } //when press mouse button and over the grid going to instantiate new object place it in the position of the cell

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualisation.SetActive(false);
        cellIndicator.SetActive(false); //provide a preview od where to place this selected object
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)//not in placement mode, don't check for position
             return; 
        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mouseposition);
        mouseIndicator.transform.position = mouseposition;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
    }
}
