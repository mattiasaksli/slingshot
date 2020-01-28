using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MainTilemap))]
public class MainTilemapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MainTilemap myTarget = (MainTilemap)target;

        if (GUILayout.Button("Merge Tilemaps"))
        {
            myTarget.Merge();
        }
    }
}