using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridSystem))]
public class GridSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridSystem gridSystem = (GridSystem)target;

        GUI.enabled = Application.isPlaying;
        
        if (GUILayout.Button("Show Dict Grid Position To Unit"))
        {
            gridSystem.ShowDictGridPositionToUnit();
        }
    }
}
