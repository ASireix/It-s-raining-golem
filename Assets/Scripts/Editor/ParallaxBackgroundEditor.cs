using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParallaxBackground))]
public class ParallaxBackgroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        

        ParallaxBackground myScript = (ParallaxBackground)target;
        if (GUILayout.Button("Initialize Array"))
        {
            myScript.InitLayers();
        }

        if (GUILayout.Button("Invert Parallax"))
        {
            myScript.InvertParallax();
        }

        DrawDefaultInspector();
    }
}
