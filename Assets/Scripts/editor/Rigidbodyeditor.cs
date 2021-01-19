#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Rigidbody2D))]
//Only used for debugging jump
public class Rigidbodyeditor : Editor
{
    Vector2 force = new Vector2();
    ForceMode2D forceMode = ForceMode2D.Impulse;
    public override void OnInspectorGUI()
    {
        // allow defalt gui
        base.OnInspectorGUI();
        if (Application.isPlaying)
        {
            GUILayout.BeginHorizontal();
            force.x = EditorGUILayout.FloatField(force.x);
            force.y = EditorGUILayout.FloatField(force.y);
            forceMode = (ForceMode2D)EditorGUILayout.EnumPopup(forceMode);
            if (GUILayout.Button("Add Force"))
            {
                (target as Rigidbody2D).AddForce(force, forceMode);
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif