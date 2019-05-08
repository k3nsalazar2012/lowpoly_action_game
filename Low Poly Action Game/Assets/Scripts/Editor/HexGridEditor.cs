using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        HexGrid hexGrid = (HexGrid)target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Grid"))
            hexGrid.GenerateGrid();
        if (GUILayout.Button("Clear Grid"))
            hexGrid.ClearGrid();
        GUILayout.EndHorizontal();
    }
}
