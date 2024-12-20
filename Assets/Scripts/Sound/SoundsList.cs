using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewSoundList", menuName = "Sound/Sound List")]
public class SoundsList : ScriptableObject
{
    [SerializeField] public List<AudioSetEntry> entries = new();

    public List<AudioClip> TryGetValue(string key)
    {
        var entry = entries.Find(e => e.name == key);
        return entry == null ? null : entry.audioClips;
    }

    public int FindIndex(string key)
    {
        return entries.FindIndex(e => e.name == key);
    }

    public void PrintContents()
    {
        foreach (AudioSetEntry entry in entries)
        {
            Debug.Log(
                $"{entry.name}" +
                $"  {entry.audioClips}"
                );
        }
    }
}

[Serializable]
public class AudioSetEntry
{
    public string name;
    public List<AudioClip> audioClips;
    public AudioSetEntry(string name)
    {
        this.name = name;
        audioClips = new();
    }
}

//[Serializable]
//public class SetOfAudioSets
//{
//    public string setName;
//    public List<AudioSetEntry> audioSetEntries;
//}