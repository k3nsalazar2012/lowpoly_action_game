﻿using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Color defaultColor = Color.white, touchedColor = Color.magenta;
    [SerializeField] private HexCell cellPrefab;
    [SerializeField] private Text cellLabelPrefab;
    
    private HexCell[] cells;
    private Canvas gridCanvas;
    private HexMesh hexMesh;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        GenerateGrid();
    }

    private void Start()
    {
        hexMesh.Triangulate(cells);
    }

    public void GenerateGrid()
    {
        cells = new HexCell[width * height];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
                CreateCell(x, z, i++);
        }
    }

    public void ClearGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i != 0)
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < gridCanvas.transform.childCount; i++)
            DestroyImmediate(gridCanvas.transform.GetChild(i).gameObject);
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z/ 2) * (HexMetrics.innderRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        if (x > 0)
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
            }
        }

        /*Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLine();*/
    }

    public void ColorCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
    }
}
