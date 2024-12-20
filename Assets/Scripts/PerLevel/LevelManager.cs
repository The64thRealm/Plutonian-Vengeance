using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string musicString;

    private void Start()
    {
        if (musicString != null)
        {
            SoundHandler.PlayMusic(musicString);
        }
    }

    private void OnDestroy()
    {
        //SoundManager.instance.StopLooping(musicString);
    }
}
