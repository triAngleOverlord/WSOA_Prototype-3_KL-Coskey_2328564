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

    private void Update()
    {
        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mouseposition);
        mouseIndicator.transform.position = mouseposition;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
    }
}
