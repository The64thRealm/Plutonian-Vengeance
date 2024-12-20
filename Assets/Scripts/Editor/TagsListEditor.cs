#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(TagsList))]
public class TagsListEditor : Editor
{
    private string[] tags;
    private bool[] tagToggles;

    private void OnEnable()
    {
        // get all tag names and initialize toggles
        tags = InternalEditorUtility.tags;
        tagToggles = new bool[tags.Length];

        List<string> selectedTags = ((TagsList) target).tags;

        for (int i = 0; i < tags.Length; ++i)
        {
            if (selectedTags.Contains(tags[i]))
            {
                tagToggles[i] = true;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        TagsList tagsList = (TagsList)target;
        EditorGUILayout.LabelField("TagsList", EditorStyles.boldLabel);
        
        // toggles for each field
        for (int i = 0; i < tags.Length; ++i)
        {
            tagToggles[i] = EditorGUILayout.Toggle(tags[i], tagToggles[i]);
        }

        tagsList.tags.Clear();
        for (int i = 0; i < tags.Length; ++i)
        {
            if (tagToggles[i])
            {
                tagsList.tags.Add(tags[i]);
            }
        }

        EditorGUILayout.LabelField("Selected Tags:", string.Join(", ", tagsList.tags));

        DrawDefaultInspector();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(tagsList);
        }
    }
}
#endif