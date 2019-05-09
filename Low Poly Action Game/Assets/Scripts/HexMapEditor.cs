using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private HexGrid hexGrid;

    private Color activeColor;
    private int activeElevation;
    private bool applyColor;
    private bool applyElevation = true;
    private int brushSize;

    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(inputRay, out RaycastHit hit))
        {
            EditCells(hexGrid.GetCell(hit.point));
        }
    }

    private void EditCell(HexCell cell)
    {
        if (cell)
        {
            if (applyColor)
                cell.Color = activeColor;

            if (applyElevation)
                cell.Elevation = activeElevation;
        }
    }

    public void SelectColor(int index)
    {
        applyColor = index >= 0;

        if (applyColor)
            activeColor = colors[index];
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    public void SetApplyElevation(bool toogle)
    {
        applyElevation = toogle;
    }

    public void SetBrushSize(float size)
    {
        brushSize = (int) size;
    }

    private void EditCells(HexCell center)
    {
        int centerX = center.coordinates.X;
        int centerZ = center.coordinates.Z;

        for(int r=0, z = centerZ - brushSize; z <= centerZ; z++, r++)
        {
            for (int x = centerX - r; x <= centerX + brushSize; x++)
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
        }

        for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
        {
            for (int x = centerX - brushSize; x <= centerX + r; x++)
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
        }
    }

    public void ShowUI(bool visible)
    {
        hexGrid.ShowUI(visible);
    }
}