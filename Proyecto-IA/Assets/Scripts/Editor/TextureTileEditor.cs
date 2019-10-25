using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicTerrainTiling))]
public class TextureTileEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        DynamicTerrainTiling myScript = (DynamicTerrainTiling)target;
        if (GUILayout.Button("Update Tiling")) {
            myScript.updateTiling();
        }
    }
}
