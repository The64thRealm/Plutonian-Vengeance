#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(TagsBasedDamager), true)]
public class DamagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TagsBasedDamager damager = (TagsBasedDamager)target;
        if (damager.GetComponent<TagsBasedDamager>() != damager)
        {
            EditorGUILayout.LabelField("Secondary TagsBasedDamager (for capturables)", EditorStyles.boldLabel);
        }
        else
        {
            EditorGUILayout.LabelField("TagsBasedDamager", EditorStyles.boldLabel);
        }

        DrawDefaultInspector();
    }
}
#endif