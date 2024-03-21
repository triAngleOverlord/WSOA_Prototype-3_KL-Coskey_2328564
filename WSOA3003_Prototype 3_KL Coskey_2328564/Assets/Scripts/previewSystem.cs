using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class previewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f; // does not clip  with planes

    [SerializeField] private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField] private Material previewMaterialPrefab;//transparent material reference
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;
    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.y, size.x ,1);
            //cellIndicatorRenderer.material.mainTextureScale = size;//
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            Material[] mat = renderer.materials;
            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = previewMaterialInstance;
            }
            renderer.materials = mat;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        Destroy(previewObject);
    }

    public void UpdatePostion(Vector3 pos, bool validity)
    {
        MovePreview(pos);
        MoveCursor(pos);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void MoveCursor(Vector3 pos)
    {
        cellIndicator.transform.position = pos;
    }

    private void MovePreview(Vector3 pos)
    {
        previewObject.transform.position = new Vector3(pos.x, pos.y + previewYOffset, pos.z);
    }
}
