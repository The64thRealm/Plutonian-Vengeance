#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

// A very dumb script that I spent like 3 hours on and then realized
// I didn't need
//[CustomEditor(typeof(SoundsList))]
public class SoundsListEditor : Editor
{
    private List<bool> individualSoundTypeFoldouts;
    private List<string> currentKeyOrder;
    private SoundsList soundList;

    private void OnEnable()
    {
        Setup();
    }

    public override void OnInspectorGUI()
    {
        // for each key we need to draw a panel to add more audios
        for (int keyCount = 0; keyCount < soundList.entries.Count; ++keyCount)
        {
            EditorGUILayout.Space();
            individualSoundTypeFoldouts[keyCount] = EditorGUILayout.Foldout(individualSoundTypeFoldouts[keyCount], soundList.entries[keyCount].name, true);
            if (individualSoundTypeFoldouts[keyCount])
            {
                EditorGUILayout.BeginHorizontal();

                currentKeyOrder[keyCount] = EditorGUILayout.DelayedTextField(new GUIContent("Sound type name:", "The name of the string to be passed in when this sound type is to be called"), currentKeyOrder[keyCount]);
                if (currentKeyOrder[keyCount] != soundList.entries[keyCount].name)
                {
                    if (!ReplaceSoundType(keyCount, currentKeyOrder[keyCount]))
                    {
                        currentKeyOrder[keyCount] = soundList.entries[keyCount].name;
                    }
                    
                }

                if (soundList.entries.Count > 1)
                {
                    if (keyCount > 0)
                    {
                        if (GUILayout.Button("Up"))
                        {
                            SwapOrderOfTwoSoundTypes(keyCount, keyCount - 1);
                        }
                    }
                    if (keyCount < soundList.entries.Count - 1)
                    {
                        if (GUILayout.Button("Down"))
                        {
                            SwapOrderOfTwoSoundTypes(keyCount, keyCount + 1);
                        }
                    }
                }

                if (GUILayout.Button("Remove Audio Type"))
                {
                    RemoveSoundType(keyCount);
                }

                EditorGUILayout.EndHorizontal();

                for (int audioCount = 0; audioCount < soundList.entries[keyCount].audioClips.Count; ++audioCount)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    soundList.entries[keyCount].audioClips[audioCount] = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", soundList.entries[keyCount].audioClips[audioCount], typeof(AudioClip), true);
                    if (GUILayout.Button("Remove"))
                    {
                        soundList.entries[keyCount].audioClips.RemoveAt(audioCount);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if (GUILayout.Button("Add New Audio Clip"))
                {
                    soundList.entries[keyCount].audioClips.Add(null);
                }
            }
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Add Item"))
        {
            AddSoundType(GetUnduplicatedName());
        }

        // DEBUG SECTION
        EditorGUILayout.Space(800);
        EditorGUILayout.LabelField("Debug");

        //if (GUILayout.Button("Clear Sounds (ONLY CLICK IF YOU ARE SURE)"))
        //{
        //    preset.audioClipMap.Clear();
        //    preset.keyOrder.Clear();
        //    individualSoundTypeFoldouts.Clear();
        //    currentKeyOrder.Clear();
        //}

        if (GUILayout.Button("Resync (if the thing is being wonky)"))
        {
            Setup();
        }

        if (GUILayout.Button("Print all"))
        {
            soundList.PrintContents();
        }

        if (GUI.changed) { EditorUtility.SetDirty(this); }
    }

    private bool ReplaceSoundType(int currentItemIndex, string newSoundType)
    {
        int index = soundList.FindIndex(newSoundType);

        if (index == -1)
        {
            return false;
        }

        soundList.entries[index].name = newSoundType;
        
        return true;
    }

    // DOES NOT DO ERROR CHECKING
    private void SwapOrderOfTwoSoundTypes(int index1, int index2)
    {
        // swap
        AudioSetEntry tempEntry = soundList.entries[index2];
        soundList.entries[index2] = soundList.entries[index1];
        soundList.entries[index1] = tempEntry;

        // reflect the changes
        currentKeyOrder[index1] = soundList.entries[index1].name;
        currentKeyOrder[index2] = soundList.entries[index2].name;

        // swap which one is opened
        bool tempBool = individualSoundTypeFoldouts[index1];
        individualSoundTypeFoldouts[index1] = individualSoundTypeFoldouts[index2];
        individualSoundTypeFoldouts[index2] = tempBool;
    }

    private void AddSoundType(string soundType)
    {
        soundList.entries.Add(new AudioSetEntry(soundType));
        currentKeyOrder.Add(soundType);
        individualSoundTypeFoldouts.Add(false);
    }

    // does not do error checking
    private void RemoveSoundType(int indexToRemove)
    {
        soundList.entries.RemoveAt(indexToRemove);
        currentKeyOrder.RemoveAt(indexToRemove);
        individualSoundTypeFoldouts.RemoveAt(indexToRemove);
    }

    private string GetUnduplicatedName(string soundName = "New Sound Type")
    {
        if (soundList.FindIndex(soundName) == -1)
        {
            return soundName;
        }

        string uniqueName = soundName;
        int iterations = 1;

        while (soundList.FindIndex($"{uniqueName} ({iterations})") != -1) { ++iterations; }

        return $"{uniqueName} ({iterations})";
    }

    private void Setup()
    {
        soundList = (SoundsList)target;
        individualSoundTypeFoldouts = new List<bool>();
        for (int i = 0; i < soundList.entries.Count; ++i)
        {
            individualSoundTypeFoldouts.Add(false);
        }

        currentKeyOrder = new();

        foreach (AudioSetEntry entry in soundList.entries)
        {
            currentKeyOrder.Add(entry.name);
        }
    }
}
#endif