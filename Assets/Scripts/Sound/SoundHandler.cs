using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    private static string currentMusic = null;
    // yes I could've just used a default parameter but for some reason that makes the method inaccessable to
    // events in the inspector
    public static void PlaySound(string sound)
    {
        PlaySound(sound, -1);
    }

    public static void PlaySound(string sound, float volume)
    {
        string[] paths = sound.Split('.');
        SoundsContainer.instance.PlaySound(paths[0], paths[1], volume);
    }

    // yes I could've just used a default parameter but for some reason that makes the method inaccessable to
    // events in the inspector
    public static void PlayLooping(string sound)
    {
        PlayLooping(sound, -1);
    }

    public static void PlayLooping(string sound, float volume)
    {
        string[] paths = sound.Split(".");
        SoundsContainer.instance.PlayLooping(paths[0], paths[1], volume);
    }

    public static void StopLooping(string sound)
    {
        string[] paths = sound.Split(".");
        SoundsContainer.instance.StopLooping(paths[0], paths[1]);
    }

    public static void PlayMusic(string musicString)
    {
        if (currentMusic == musicString) return;
        if (currentMusic != null)
        {
            StopLooping(currentMusic);
        }
        PlayLooping(musicString);
        currentMusic = musicString;
    }

    // TODO animation events cannot use static members so I have to do this instead which is really scuffed
    // I should probably fix this in the future cuz it's just spaghetti
    public void PlaySoundInstance(string sound)
    {
        PlaySound(sound);
    }
}
