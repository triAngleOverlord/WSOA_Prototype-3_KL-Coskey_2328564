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
    private int selectedObjectIndex = -1;//if it is -1 (by default) no item is selected

    [SerializeField] private GameObject gridVisualisation;

    private GridData furnitureData;

    private Renderer previewRender;

    private List<GameObject> placedObjects = new();
    private void Start()
    {
        StopPlacement();
        furnitureData = new ();
        previewRender = cellIndicator.GetComponentInChildren<Renderer>();
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
        }
        //Debug.Log("Place");
        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mouseposition);

        //bool placementValidity = CheckPlacementValidity(gridPos, selectedObjectIndex);
        bool placementValidity =furnitureData.CanPlaceObjectAt(gridPos, database.objectData[selectedObjectIndex].Size);
        if (placementValidity == false)
        {
            return;
        }
        GameObject newObject = Instantiate(database.objectData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPos);
        placedObjects.Add(newObject);
        //GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        furnitureData.AddObjectAT(gridPos, database.objectData[selectedObjectIndex].Size, 
                                          database.objectData[selectedObjectIndex].ID,
                                          placedObjects.Count - 1);
        Debug.Log(grid.CellToWorld(gridPos));

    } //when press mouse button and over the grid going to instantiate new object place it in the position of the cell

    /*
    private bool CheckPlacementValidity(Vector3Int gridPos, int selectedObjectIndex)
    {
        //GridData selectedData = furnitureData;
        GridData selectedData =database.objectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;////
        //if checking if we can place furniture onto floor
        //Debug.Log(selectedData.ToString());
        return selectedData.CanPlaceObjectAt(gridPos, database.objectData[selectedObjectIndex].Size);
    }*/

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
        
        bool placementValidity = furnitureData.CanPlaceObjectAt(gridPos, database.objectData[selectedObjectIndex].Size);
        previewRender.material.color = placementValidity ? Color.white : Color.red;//changes the color if true or false

        mouseIndicator.transform.position = mouseposition;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
    }
}
